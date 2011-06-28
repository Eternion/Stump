
using System;
using System.Net.Sockets;
using NLog;
using Stump.Core.IO;
using Stump.Core.Pool;
using Stump.Core.Pool.Task;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.BaseServer.Network
{
    public abstract class BaseClient : IPacketReceiver
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public event Action<BaseClient, Message> MessageReceived;
        public event Action<BaseClient, Message> MessageSended;

        private void NotifyMessageReceived(Message message)
        {
            if (MessageListener.MinMessageInterval.HasValue && DateTime.Now.Subtract(LastActivity).TotalMilliseconds < MessageListener.MinMessageInterval)
            {
                Close();
                return;
            }
            if (Settings.InactivityDisconnectionTime.HasValue || MessageListener.MinMessageInterval.HasValue)
                LastActivity = DateTime.Now;
            if (MessageReceived != null) MessageReceived(this, message);
        }

        private void NotifyMessageSended(Message message)
        {
            if (MessageSended != null) MessageSended(this, message);
        }

        private const byte BIT_RIGHT_SHIFT_LEN_PACKET_ID = 2;
        private const byte BIT_MASK = 3;

        private static SocketAsyncEventArgsPool m_argsPool;
        private static QueueDispatcher m_dispatcher;

        private static bool m_isInitialized;

        private readonly BigEndianReader m_buffer = new BigEndianReader();
        private object m_lock = new object();
        private bool m_splittedPacket;
        private uint m_splittedPacketId;
        private uint m_splittedPacketLength;
        private int m_staticHeader;


        internal static void Initialize(ref SocketAsyncEventArgsPool pool, ref QueueDispatcher dispatcher)
        {
            m_argsPool = pool;
            m_dispatcher = dispatcher;

            m_isInitialized = true;
        }

        protected BaseClient(Socket socket)
        {
            if (!m_isInitialized)
                throw new Exception("BaseClient is not initialized");

            Socket = socket;
            IP = socket.RemoteEndPoint.ToString();
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
            get { return Socket != null && Socket.Connected; }
        }

        public string IP
        {
            get;
            private set;
        }

        public DateTime LastActivity
        {
            get;
            private set;
        }

        public virtual void Send(Message message)
        {
            if (Socket == null || !Connected)
            {
                throw new Exception("Attempt to send a packet when client isn't connected");
            }

            SocketAsyncEventArgs args = m_argsPool.Pop();

            byte[] data;
            using (var writer = new BigEndianWriter())
            {
                message.Pack(writer);
                data = writer.Data;
            }

            args.SetBuffer(data, 0, data.Length);

            if (!Socket.SendAsync(args))
            {
                m_argsPool.Push(args);
            }

#if DEBUG
                Console.WriteLine(string.Format("{0} >> {1}", this, message.GetType().Name));
#endif

            NotifyMessageSended(message);
        }

        internal void ProcessReceive(byte[] data, int offset, int count)
        {
            try
            {
                if (!CanReceive)
                    throw new Exception("Cannot receive packet : CanReceive is false");

                m_buffer.Add(data, offset, count);

                BuildMessage();
            }
            catch (Exception ex)
            {
                logger.Error("Forced disconnection " + ToString() + " : " + ex.Message);

                Disconnect();
            }
        }

        protected virtual void BuildMessage()
        {
            uint len = 0;

            lock (m_lock)
            {
            replay:
                if (!m_splittedPacket)
                {
                    if (m_buffer.BytesAvailable < 2)
                    {
                        return;
                    }

                    var header = m_buffer.ReadUShort();
                    var id = GetMessageIdByHeader(header);
                    var lenType = header & BIT_MASK;

                    if (m_buffer.BytesAvailable >= lenType)
                    {
                        len = GetMessageLengthByLenType(lenType);
                        if (m_buffer.BytesAvailable >= len)
                        {
                            var message = MessageReceiver.BuildMessage(id, m_buffer.ReadBytesInNewBigEndianReader((int) len));

                            m_dispatcher.Enqueue(this, message);

                            NotifyMessageReceived(message);

                            goto replay;
                        }

                        m_staticHeader = -1;
                        m_splittedPacketLength = len;
                        m_splittedPacketId = id;
                        m_splittedPacket = true;

                        return;
                    }

                    m_staticHeader = header;
                    m_splittedPacketLength = len;
                    m_splittedPacketId = id;
                    m_splittedPacket = true;
                    return;
                }

                if (m_staticHeader != -1)
                {
                    m_splittedPacketLength = GetMessageLengthByLenType(m_staticHeader & BIT_MASK);
                    m_staticHeader = -1;
                }
                if (m_buffer.BytesAvailable >= m_splittedPacketLength)
                {
                    var message = MessageReceiver.BuildMessage(m_splittedPacketId, m_buffer.ReadBytesInNewBigEndianReader((int) m_splittedPacketLength));

                    m_dispatcher.Enqueue(this, message);
                    m_splittedPacket = false;

                    NotifyMessageReceived(message);
                    goto replay;
                }
            }
        }

        private static uint GetMessageIdByHeader(uint header)
        {
            return header >> BIT_RIGHT_SHIFT_LEN_PACKET_ID;
        }

        private uint GetMessageLengthByLenType(int lenType)
        {
            switch (lenType)
            {
                case 0:
                    {
                        return 0;
                    }
                case 1:
                    {
                        return m_buffer.ReadByte();
                    }
                case 2:
                    {
                        return m_buffer.ReadUShort();
                    }
                case 3:
                    {
                        return (uint)((m_buffer.ReadByte() << 16) | (m_buffer.ReadByte() << 8) | m_buffer.ReadByte());
                    }
                default:
                    {
                        return 0;
                    }
            }
        }


        /// <summary>
        ///   Disconnect the Client. Cannot reuse the socket.
        /// </summary>
        public void Disconnect()
        {
            if (Connected)
            {
                OnDisconnect();

                Close();
            }
        }

        /// <summary>
        ///   Disconnect the Client after a time
        /// </summary>
        /// <param name = "timeToWait"></param>
        public void DisconnectLater(int timeToWait = 0)
        {
            if (timeToWait == 0)
                TaskPool.Instance.EnqueueTask(Disconnect);
            else
                TaskPool.Instance.RegisterCyclicTask(Disconnect, timeToWait, null, null);
        }

        protected virtual void OnDisconnect()
        {

        }

        protected void Close()
        {
            if (Socket != null && Socket.Connected)
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();

                Socket = null;
            }
        }

        public override string ToString()
        {
            return string.Concat("<", IP, ">");
        }
    }
}