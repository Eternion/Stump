using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.Core.Extensions;
using Stump.Core.IO;
using Stump.Core.Pool.New;
using Stump.Core.Pool.Task;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.BaseServer.Network
{
    public abstract class BaseClient : IPacketReceiver, IDisposable
    {
        private readonly ClientManager m_clientManager;

        [Variable(DefinableRunning = true)]
        public static bool LogPackets = false;

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly BigEndianReader m_buffer = new BigEndianReader();
        private readonly object m_lock = new object();
        private MessagePart m_currentMessage;
        private bool m_disconnecting;

        private bool m_onDisconnectCalled = false;
        private int m_offset;
        private int m_remainingLength;
        private BufferSegment m_bufferSegment;
        private long m_bytesReceived;
        private long m_totalBytesReceived;

        protected BaseClient(Socket socket, ClientManager clientManager)
        {
            Socket = socket;
            IP = ( (IPEndPoint)socket.RemoteEndPoint ).Address.ToString();
            m_clientManager = clientManager;
            m_bufferSegment = BufferManager.Default.CheckOut();
        }

        protected BaseClient(Socket socket)
            : this (socket, ClientManager.Instance)
        {
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
            if (Socket == null || !Connected)
            {
                return;
            }

            var args = ObjectPoolMgr.ObtainObject<SocketAsyncEventArgs>();
            args.Completed += OnSendCompleted;

            byte[] data;
            using (var writer = new BigEndianWriter())
            {
                message.Pack(writer);
                data = writer.Data;
            }

            Console.WriteLine("SEND : " + data.ToString(" "));
            args.SetBuffer(data, 0, data.Length);

            if (!Socket.SendAsync(args))
            {
                args.Completed -= OnSendCompleted;
                ObjectPoolMgr.ReleaseObject(args);
            }

            if (LogPackets)
                Console.WriteLine(string.Format("(SEND) {0} : {1}", this, message));

            LastActivity = DateTime.Now;
            OnMessageSended(message);
        }

        private void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            args.Completed -= OnSendCompleted;
            ObjectPoolMgr.ReleaseObject(args);
        }

        #endregion

        public event Action<BaseClient, Message> MessageReceived;
        public event Action<BaseClient, Message> MessageSended;

        protected virtual void OnMessageReceived(Message message)
        {
            if (MessageReceived != null)
                MessageReceived(this, message);
        }

        protected virtual void OnMessageSended(Message message)
        {
            if (MessageSended != null)
                MessageSended(this, message);
        }

        public void BeginReceive()
        {
            if (!CanReceive)
                throw new Exception("Cannot receive packet : CanReceive is false");

            ResumeReceive();
        }

        private void ResumeReceive()
        {
            if (Socket != null && Socket.Connected)
            {
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
                    // increment our counters
                    unchecked
                    {
                        m_bytesReceived += (uint)bytesReceived;
                    }

                    Interlocked.Add(ref m_totalBytesReceived, bytesReceived);

                    m_remainingLength += bytesReceived;

                    Console.WriteLine("RECV : " + m_bufferSegment.SegmentData.Take(bytesReceived).ToString(" "));
                    if (BuildMessage(m_bufferSegment))
                    {
                        m_offset = 0;
                        m_bufferSegment.DecrementUsage();
                        m_bufferSegment = BufferManager.Default.CheckOut();
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

                if (m_remainingLength > 0)
                    return BuildMessage(buffer); // there is maybe a second message in the buffer
                else
                    return true;
            }
            else
            {
                m_offset = (int) reader.Position;
                m_remainingLength = (int) (reader.MaxPosition - m_offset);
            }

            return false;
        }

        /// <summary>
        ///     Makes sure the underlying buffer is big enough (but will never exceed BufferSize)
        /// </summary>
        protected void EnsureBuffer()
        {
            BufferSegment newSegment = BufferManager.Default.CheckOut();
            Array.Copy(m_bufferSegment.Buffer.Array,
                       m_bufferSegment.Offset + m_offset,
                       newSegment.Buffer.Array,
                       newSegment.Offset,
                       m_remainingLength);
            m_bufferSegment.DecrementUsage();
            m_bufferSegment = newSegment;
            m_offset = 0;
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