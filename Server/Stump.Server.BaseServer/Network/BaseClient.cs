using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.Core.IO;
using Stump.Core.Pool;
using Stump.DofusProtocol.Messages;
using Stump.Core.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.BaseServer.Network
{
    public abstract class BaseClient : IPacketReceiver, IDisposable, IClient
    {
        [Variable(DefinableRunning = true)]
        public static bool LogPackets = false;

        [Variable(DefinableRunning = true)]
        public static int MessagesEntriesLimit = 20;

        [Variable(DefinableRunning = true)]
        public static bool FloodCheck = true;

        [Variable(DefinableRunning = true)]
        public static double FloodMinTime = 3.0;

        [Variable(DefinableRunning = true)]
        public readonly static List<string> MessagesWhitelist = new List<string>
            {
                "GameActionAcknowledgementMessage",
                "GameFightTurnReadyMessage"
            };

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private MessagePart m_currentMessage;
        private bool m_disconnecting;

        private bool m_onDisconnectCalled;
        // sub offset until where we can write in the segment
        private int m_writeOffset;
        // sub offset until where we can read in the segment
        private int m_readOffset;
        private int m_remainingLength;
        private BufferSegment m_bufferSegment;
        private long m_totalBytesReceived;

        private readonly LimitedStack<Pair<DateTime, Message>> m_messagesHistory = new LimitedStack<Pair<DateTime, Message>>(MessagesEntriesLimit);

        protected BaseClient(Socket socket)
        {
            Socket = socket;
            IP = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();
            m_bufferSegment = BufferManager.GetSegment(ClientManager.BufferSize);
#if DEBUG
            m_bufferSegment.Token = this;
#endif
        }

        public Socket Socket
        {
            get;
            private set;
        }

        public bool CanReceive
        {
            get;
            protected set;
        }

        public bool Connected
        {
            get { return Socket != null && Socket.Connected && !m_disconnecting; }
        }

        public string IP
        {
            get;
            private set;
        }

        /// <summary>
        /// Last activity as a socket client (last received packets)
        /// </summary>
        public DateTime LastActivity
        {
            get { return m_messagesHistory.Peek().First; }
        }

        #region IPacketReceiver Members

        public virtual void Send(Message message)
        {
            var stream = BufferManager.GetSegmentStream(ClientManager.BufferSize);

            var writer = new BigEndianWriter(stream);
            try
            {
                message.Pack(writer);
            }
            catch (Exception ex)
            {
                stream.Dispose();
                throw new Exception(ex.Message + "(" + message + ")", ex);
            }

            Send(stream);

            OnMessageSent(message);
        }

        // a bit dirty. only used by WorldClientCollection
        public void Send(SegmentStream stream)
        {
            if (Socket == null || !Connected)
            {
                stream.Dispose();
                return;
            }

            var args = ObjectPoolMgr.ObtainObject<PoolableSocketArgs>();

            try
            {
                args.Completed += OnSendCompleted;
                args.SetBuffer(stream.Segment.Buffer.Array, stream.Segment.Offset, (int)(stream.Position));
                args.UserToken = stream;

                if (!Socket.SendAsync(args))
                {
                    args.Completed -= OnSendCompleted;
                    stream.Dispose();
                    ObjectPoolMgr.ReleaseObject(args);
                }
            }
            catch
            {
                args.Dispose();
                stream.Dispose();
                // args could be disposed if an error occured
                throw;
            }
        }

        private static void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            args.Completed -= OnSendCompleted;
            var stream = args.UserToken as SegmentStream;
            if (stream != null)
                stream.Dispose();

            ObjectPoolMgr.ReleaseObject((PoolableSocketArgs)args);
        }

        #endregion

        public event Action<BaseClient, Message> MessageReceived;
        public event Action<BaseClient, Message> MessageSent;

        protected virtual void OnMessageReceived(Message message)
        {
            if (MessageReceived != null)
                MessageReceived(this, message);
        }

        public virtual void OnMessageSent(Message message)
        {
            if (LogPackets)
                Console.WriteLine("(SEND) {0} : {1}", this, message);

            if (MessageSent != null)
                MessageSent(this, message);
        }

        public void BeginReceive()
        {
            if (!CanReceive)
                throw new Exception("Cannot receive packet : CanReceive is false");

            ResumeReceive();
        }

        private void ResumeReceive()
        {
            if (Socket == null || !Socket.Connected)
                return;

            var socketArgs = ClientManager.Instance.PopSocketArg();

            socketArgs.SetBuffer(m_bufferSegment.Buffer.Array, m_bufferSegment.Offset + m_writeOffset, m_bufferSegment.Length - m_writeOffset);
            socketArgs.UserToken = this;
            socketArgs.Completed += ProcessReceive;

            var willRaiseEvent = Socket.ReceiveAsync(socketArgs);
            if (!willRaiseEvent)
            {
                ProcessReceive(this, socketArgs);
            }
        }

        private void ProcessReceive(object sender, SocketAsyncEventArgs args)
        {
            try
            {
                args.Completed -= ProcessReceive;
                var bytesReceived = args.BytesTransferred;

                if (args.LastOperation != SocketAsyncOperation.Receive || bytesReceived == 0)
                {
                    Disconnect();
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
                logger.Error("Forced disconnection " + ToString() + " : " + ex);

                Disconnect();
            }
            finally
            {
                ClientManager.Instance.PushSocketArg((PoolableSocketArgs)args);
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

                if (!MessagesWhitelist.Contains(message.ToString()))
                    m_messagesHistory.Push(new Pair<DateTime, Message>(DateTime.Now, message));

                var time = m_messagesHistory.Last.Value.First.Subtract(m_messagesHistory.First.Value.First);

                //Flood check, 
                if (FloodCheck && (m_messagesHistory.Count == m_messagesHistory.MaxItems && time.TotalSeconds < FloodMinTime))
                {
                    logger.Error($"Forced disconnection {this}: Flood: {m_messagesHistory.Count} messages in {time.TotalSeconds} seconds ! - LastMessages: {m_messagesHistory.Select(x => x.Second).ToCSV(",")}");
                    Disconnect();

                    return false;
                }

                if (LogPackets)
                    Console.WriteLine($"(RECV) {this} : {message}");

                OnMessageReceived(message);

                m_remainingLength -= (int)(reader.Position - (buffer.Offset + m_readOffset));
                m_writeOffset = m_readOffset = (int)reader.Position - buffer.Offset;
                m_currentMessage = null;

                return m_remainingLength <= 0 || BuildMessage(buffer);
            }

            logger.Debug("Message truncated, ensure buffer is big enough ...");

            m_remainingLength -= (int)(reader.Position - (buffer.Offset + m_readOffset));
            m_readOffset = (int)reader.Position - buffer.Offset;
            m_writeOffset = m_readOffset + m_remainingLength;

            EnsureBuffer(m_currentMessage.Length.HasValue ? m_currentMessage.Length.Value : 5); // 5 is the max header size

            return false;
        }

        /// <summary>
        ///     Makes sure the underlying buffer is big enough
        /// </summary>
        protected bool EnsureBuffer(int length)
        {
            if (m_bufferSegment.Length - m_writeOffset < length + m_remainingLength)
            {
                var newSegment = BufferManager.GetSegment(Math.Max(length + m_remainingLength, ClientManager.BufferSize));

                // the data before m_readOffset are deprecated, we don't need them anymore
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

        /// <summary>
        ///   Disconnect the Client. Cannot reuse the socket.
        /// </summary>
        public void Disconnect()
        {
            if (m_onDisconnectCalled)
                return;

            m_onDisconnectCalled = true;
            m_disconnecting = true;

            try
            {
                OnDisconnect();
            }
            catch (Exception ex)
            {
                logger.Error("Exception occurs while disconnecting client {0} : {1}", ToString(), ex);
            }
            finally
            {
                ClientManager.Instance.OnClientDisconnected(this);
                Dispose();
            }
        }

        /// <summary>
        ///   Disconnect the Client after a time
        /// </summary>
        /// <param name = "timeToWait"></param>
        public void DisconnectLater(int timeToWait = 0)
        {
            if (timeToWait == 0)
                ServerBase.InstanceAsBase.IOTaskPool.AddMessage(Disconnect);
            else
                ServerBase.InstanceAsBase.IOTaskPool.CallDelayed(timeToWait, Disconnect);
        }

        protected virtual void OnDisconnect()
        {
        }

        ~BaseClient()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Socket != null && Socket.Connected)
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
            }
            if (m_bufferSegment != null)
            {
                m_bufferSegment.DecrementUsage();
            }
        }

        public override string ToString()
        {
            return string.Concat("<", IP, ">");
        }
    }
}