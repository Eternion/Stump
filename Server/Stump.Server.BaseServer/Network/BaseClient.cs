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

namespace Stump.Server.BaseServer.Network
{
    public abstract class BaseClient : IPacketReceiver, IDisposable
    {
        [Variable(DefinableRunning = true)]
        public static bool LogPackets = false;

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private MessagePart m_currentMessage;
        private bool m_disconnecting;

        private bool m_onDisconnectCalled;
        private int m_writeOffset;
        private int m_readOffset;
        private int m_remainingLength;
        private BufferSegment m_bufferSegment;
        private long m_totalBytesReceived;

        protected BaseClient(Socket socket)
        {
            Socket = socket;
            IP = ( (IPEndPoint)socket.RemoteEndPoint ).Address.ToString();
            m_bufferSegment = BufferManager.GetSegment(ClientManager.BufferSize);
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
        /// Last activity as a socket client (last received packet or sent packet)
        /// </summary>
        public DateTime LastActivity
        {
            get;
            private set;
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
            catch
            {
                stream.Dispose();
                throw;
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
                            
            var args = ObjectPoolMgr.ObtainObject<SocketAsyncEventArgs>();
            try
            {
                args.Completed += OnSendCompleted;
                args.SetBuffer(stream.Segment.Buffer.Array, stream.Segment.Offset, (int) (stream.Position));
                args.UserToken = stream;

                if (!Socket.SendAsync(args))
                {
                    args.Completed -= OnSendCompleted;
                    stream.Dispose();
                    ObjectPoolMgr.ReleaseObject(args);
                }
                LastActivity = DateTime.Now;
            }
            catch
            {
                stream.Dispose();
                ObjectPoolMgr.ReleaseObject(args);
                throw;
            }
        }

        private static void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            args.Completed -= OnSendCompleted;
            var stream = args.UserToken as SegmentStream;
            if (stream != null)
                stream.Dispose();

            ObjectPoolMgr.ReleaseObject(args);
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
                var bytesReceived = args.BytesTransferred;

                if (bytesReceived == 0)
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
                args.Completed -= ProcessReceive;
                ClientManager.Instance.PushSocketArg(args);
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

                LastActivity = DateTime.Now;

                if (LogPackets)
                    Console.WriteLine("(RECV) {0} : {1}", this, message);

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

        /// <summary>
        ///     Makes sure the underlying buffer is big enough
        /// </summary>
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