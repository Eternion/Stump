using NLog;
using Stump.Core.Extensions;
using Stump.Core.IO;
using Stump.Core.Pool;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Network;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FakeClient
{
    public class FakeClient : IClient
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private Socket m_socket;

        private MessagePart m_currentMessage;

        private int m_writeOffset;
        private int m_readOffset;
        private int m_remainingLength;
        private BufferSegment m_bufferSegment;
        private long m_totalBytesReceived;

        public event Action<FakeClient> Connected;

        public event Action<FakeClient, bool> Disconnected;

        public FakeClient(int id)
        {
            Id = id;
            m_bufferSegment = BufferManager.GetSegment(ClientManager.BufferSize);
        }

        public string AccountName
        {
            get;
            set;
        }

        public string AccountPassword
        {
            get;
            set;
        }

        public string WorldIp
        {
            get;
            set;
        }

        public int WorldPort
        {
            get;
            set;
        }

        public bool KamasTransferred
        {
            get;
            set;
        }

        public bool IsDisconnectionPlanned
        {
            get;
            set;
        }

        public bool ConnectingToWorld
        {
            get;
            set;
        }

        public bool IsInGame
        {
            get;
            set;
        }

        public string Ticket
        {
            get;
            set;
        }

        public int Id
        {
            get;
            private set;
        }

        public bool IsConnected
        {
            get { return m_socket != null && m_socket.Connected; }
        }

        public void Connect(string ip, int port)
        {
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            var args = new SocketAsyncEventArgs();
            args.Completed += OnConnected;
            args.AcceptSocket = m_socket;
            args.RemoteEndPoint = new DnsEndPoint(ip, port);
            if (!m_socket.ConnectAsync(args))
                OnConnected(this, args);
        }

        public void Disconnect()
        {
            Disconnect(false);
        }

        public void Disconnect(bool planned)
        {
            if (IsConnected)
            {
                IsDisconnectionPlanned = planned;
                if (!planned)
                    Log("Connection interrupted unexpectedly");

                m_socket.Disconnect(false);
                OnDisconnected(true);
            }
        }

        private void OnDisconnected(bool planned)
        {
            var evnt = Disconnected;
            if (evnt != null)
                evnt(this, planned);
        }

        private void OnConnected(object sender, SocketAsyncEventArgs e)
        {
            e.Completed -= OnConnected;
            e.Dispose();

            var evnt = Connected;
            if (evnt != null)
                evnt(this);

            ResumeReceive();
        }

        public void Send(Message msg)
        {
            if (!IsConnected)
                return;

            var args = new SocketAsyncEventArgs();
            args.Completed += OnSendCompleted;

            var stream = BufferManager.GetSegmentStream(ClientManager.BufferSize);

            var writer = new BigEndianWriter(stream);
            try
            {
                msg.Pack(writer);
            }
            catch (Exception ex)
            {
                stream.Dispose();
                throw new Exception(ex.Message + "(" + msg + ")", ex);
            }

            try
            {
                args.SetBuffer(stream.Segment.Buffer.Array, stream.Segment.Offset, (int)(stream.Position));
                args.UserToken = stream;

                if (!m_socket.SendAsync(args))
                {
                    args.Completed -= OnSendCompleted;
                    args.Dispose();
                    stream.Dispose();
                }
            }
            catch
            {
                args.Dispose();
                stream.Dispose();
                throw;
            }
        }

        private void OnSendCompleted(object sender, SocketAsyncEventArgs e)
        {
            e.Dispose();
        }

        private void ResumeReceive()
        {
            if (m_socket == null || !m_socket.Connected)
                return;

            var socketArgs = new SocketAsyncEventArgs();

            socketArgs.SetBuffer(m_bufferSegment.Buffer.Array, m_bufferSegment.Offset + m_writeOffset, m_bufferSegment.Length - m_writeOffset);
            socketArgs.UserToken = this;
            socketArgs.Completed += ProcessReceive;

            var willRaiseEvent = m_socket.ReceiveAsync(socketArgs);
            if (!willRaiseEvent)
            {
                ProcessReceive(this, socketArgs);
            }
        }

        private void ProcessReceive(object sender, SocketAsyncEventArgs args)
        {
            try
            {
                var bytesReceived = args.BytesTransferred;

                if (bytesReceived == 0)
                {
                    Disconnect(ConnectingToWorld);
                }
                else
                {
                    Interlocked.Add(ref m_totalBytesReceived, bytesReceived);

                    m_remainingLength += bytesReceived;
                    if (BuildMessage(m_bufferSegment))
                    {
                        m_writeOffset = m_readOffset = 0;
                        if (m_bufferSegment.Length != ClientManager.BufferSize)
                        {
                            m_bufferSegment.DecrementUsage();
                            m_bufferSegment = BufferManager.GetSegment(ClientManager.BufferSize);
                        }
                    }

                    ResumeReceive();
                }
            }
            catch (Exception ex)
            {
                logger.Error("Fake client : Forced disconnection " + ToString() + " : " + ex);

                Disconnect();
            }
            finally
            {
                args.Completed -= ProcessReceive;
                args.Dispose();
            }
        }

        protected virtual bool BuildMessage(BufferSegment buffer)
        {
            if (m_currentMessage == null)
                m_currentMessage = new MessagePart(false);

            var reader = new FastBigEndianReader(buffer)
            {
                Position = buffer.Offset + m_readOffset,
                MaxPosition = buffer.Offset + m_readOffset + m_remainingLength,
            };
            // if message is complete
            if (m_currentMessage.Build(reader))
            {
                var dataPos = reader.Position;
                // prevent to read above
                reader.MaxPosition = dataPos + m_currentMessage.Length.Value;

                Message message;
                try
                {
                    message = MessageReceiver.BuildMessage((uint)m_currentMessage.MessageId.Value, reader);
                }
                catch (Exception)
                {
                    if (m_currentMessage.ReadData)
                        logger.Debug("Message = {0}", m_currentMessage.Data.ToString(" "));
                    else
                    {
                        reader.Seek(dataPos, SeekOrigin.Begin);
                        logger.Debug("Message = {0}", reader.ReadBytes(m_currentMessage.Length.Value).ToString(" "));
                    }
                    throw;
                }

                OnMessageReceived(message);

                m_remainingLength -= (int)(reader.Position - (buffer.Offset + m_readOffset));
                m_writeOffset = m_readOffset = (int)reader.Position - buffer.Offset;
                m_currentMessage = null;

                return m_remainingLength <= 0 || BuildMessage(buffer);
            }

            m_remainingLength -= (int)(reader.Position - (buffer.Offset + m_readOffset));
            m_readOffset = (int)reader.Position - buffer.Offset;
            m_writeOffset = m_readOffset + m_remainingLength;

            EnsureBuffer(m_currentMessage.Length.HasValue ? m_currentMessage.Length.Value : 3);

            return false;
        }

        private void OnMessageReceived(Message message)
        {
            Log("Received " + message.GetType());
            Program.Handler.Dispatch(this, message);
        }

        protected bool EnsureBuffer(int length)
        {
            if (m_bufferSegment.Length - m_writeOffset < length + m_remainingLength)
            {
                var newSegment = BufferManager.GetSegment(length + m_remainingLength);

                Array.Copy(m_bufferSegment.Buffer.Array,
                           m_bufferSegment.Offset + m_readOffset,
                           newSegment.Buffer.Array,
                           newSegment.Offset,
                           m_remainingLength);

                m_bufferSegment.DecrementUsage();
                m_bufferSegment = newSegment;
                m_writeOffset = m_remainingLength;
                m_readOffset = 0;

                return true;
            }

            return false;
        }

        public void Log(string message)
        {
            logger.Info($"Client {Id} ({AccountName}) : {message}");
        }

        public override string ToString()
        {
            return "Fake Client #" + Id;
        }
    }
}