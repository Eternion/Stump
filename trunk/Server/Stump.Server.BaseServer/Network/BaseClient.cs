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
        private int m_offset;
        private int m_remainingLength;
        private BufferSegment m_bufferSegment;
        private long m_totalBytesReceived;

        protected BaseClient(Socket socket)
        {
            Socket = socket;
            IP = ( (IPEndPoint)socket.RemoteEndPoint ).Address.ToString();
            m_bufferSegment = BufferManager.Default.CheckOut();
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
            var stream = BufferManager.Default.CheckOutStream();

            var writer = new BigEndianWriter(stream);
            message.Pack(writer);

            Send(stream);

            OnMessageSent(message);
        }
        
        // a bit dirty. only used by WorldClientCollection
        public void Send(SegmentStream stream)
        {   
            if (Socket == null || !Connected)
            {
                return;
            }

            var args = ObjectPoolMgr.ObtainObject<SocketAsyncEventArgs>();
            args.Completed += OnSendCompleted;
            args.SetBuffer(stream.Segment.Buffer.Array, stream.Segment.Offset, (int) (stream.Position));
            args.UserToken = stream;

            if (!Socket.SendAsync(args))
            {
                args.Completed -= OnSendCompleted;
                stream.Segment.DecrementUsage();
                ObjectPoolMgr.ReleaseObject(args);
            }
            LastActivity = DateTime.Now;
        }

        private static void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            args.Completed -= OnSendCompleted;
            var stream = args.UserToken as SegmentStream;
            if (stream != null)
                stream.Segment.DecrementUsage();

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

            socketArgs.SetBuffer(m_bufferSegment.Buffer.Array, m_bufferSegment.Offset + m_offset, ClientManager.BufferSize - m_offset);
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
                        m_offset = 0;
                    }
                    else
                    {
                        EnsureBuffer();
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
                    Position = buffer.Offset + m_offset,
                    MaxPosition = buffer.Offset + m_offset + m_remainingLength,
                };
            // if message is complete
            if (m_currentMessage.Build(reader))
            {
                var dataPos = reader.Position;
                if (m_currentMessage.ExceedBufferSize)
                {
                    reader = new FastBigEndianReader(m_currentMessage.Data);
                }
                else
                {
                    // prevent to read above
                    reader.MaxPosition = buffer.Offset + dataPos + m_currentMessage.Length.Value;
                }

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

                m_remainingLength -= (int)reader.Position - buffer.Offset - m_offset;
                m_offset = (int)reader.Position - buffer.Offset;
                m_currentMessage = null;

                return m_remainingLength <= 0 || BuildMessage(buffer);
            }

            m_offset = (int) reader.Position;
            m_remainingLength = (int) (reader.MaxPosition - m_offset);

            return false;
        }

        /// <summary>
        ///     Makes sure the underlying buffer is big enough (but will never exceed BufferSize)
        /// </summary>
        protected void EnsureBuffer()
        {
            Array.Copy(m_bufferSegment.Buffer.Array,
                       m_bufferSegment.Offset + m_offset,
                       m_bufferSegment.Buffer.Array,
                       m_bufferSegment.Offset,
                       m_remainingLength);
            m_offset = m_remainingLength;
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