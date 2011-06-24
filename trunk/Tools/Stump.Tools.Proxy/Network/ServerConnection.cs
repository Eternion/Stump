
using System;
using System.Net;
using System.Net.Sockets;
using NLog;
using Stump.Core.IO;
using Stump.DofusProtocol.Messages.Framework.IO;
using Stump.DofusProtocol.Messages;

namespace Stump.Tools.Proxy.Network
{
    public class ServerConnection
    {
        #region Delegates

        public delegate void ConnectionHandler(ServerConnection connection);

        public delegate void MessageHandler(ServerConnection connection, Message message);

        #endregion

        private const byte BIT_RIGHT_SHIFT_LEN_PACKET_ID = 2;
        private const byte BIT_MASK = 3;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly BigEndianReader m_buffer = new BigEndianReader();
        private readonly byte[] m_localBuffer = new byte[1024];
        private readonly Socket m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private int m_bytesReaded;
        private int m_bytesSended;

        private bool m_splittedPacket;
        private uint m_splittedPacketId;
        private uint m_splittedPacketLength;
        private int m_staticHeader;

        public ServerConnection(String ip, Int16 port)
        {
            IpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        public ServerConnection(IPEndPoint ipEndPoint)
        {
            IpEndPoint = ipEndPoint;
        }

        #region CallBacks

        private void ConnectCallBack(IAsyncResult ar)
        {
            try
            {
                m_socket.EndConnect(ar);
                NotifyServerConnected();

                AsyncReceive();
            }
            catch (SocketException ex)
            {
                logger.Error(string.Format("Error on connection : {0}", ex.Message));
            }
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                int availableBytes = m_socket.EndReceive(ar);

                if (availableBytes > 0)
                {
                    m_buffer.Add(m_localBuffer, 0, availableBytes);

                    m_bytesReaded += availableBytes;

                    Receive();

                    AsyncReceive();
                }
                else
                {
                    Disconnect();
                }
            }
            catch (SocketException ex)
            {
                logger.Error(string.Format("Error on receive : {0}", ex.Message));
                Disconnect();
            }
        }

        private void SendCallBack(IAsyncResult ar)
        {
            try
            {
                m_bytesSended += m_socket.EndSend(ar);
            }
            catch (SocketException ex)
            {
                logger.Error(string.Format("Error on send : {0}", ex.Message));
                Disconnect();
            }
        }

        #endregion

        #region Private Methods

        private void AsyncReceive()
        {
            m_socket.BeginReceive(m_localBuffer, 0, m_localBuffer.Length, 0, new AsyncCallback(ReceiveCallBack), null);
        }

        private void AsyncSend(byte[] data)
        {
            m_socket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallBack), null);
        }

        #endregion

        #region Message Processor

        private void Receive()
        {
            uint len = 0;

            replay:
            if (!m_splittedPacket)
            {
                if (m_buffer.BytesAvailable < 2)
                {
                    return;
                }

                // get the header
                uint header = m_buffer.ReadUShort();
                uint id = GetMessageIdByHeader(header);

                if (m_buffer.BytesAvailable >= (header & BIT_MASK))
                {
                    len = GetMessageLengthByHeader(header);
                    if (m_buffer.BytesAvailable >= len)
                    {
                        Message message = MessageReceiver.BuildMessage(id,
                                                                     m_buffer.ReadBytesInNewBigEndianReader((int) len));

                        NotifyMessageReceived(message);
                        goto replay;
                    }

                    m_staticHeader = -1;
                    m_splittedPacketLength = len;
                    m_splittedPacketId = id;
                    m_splittedPacket = true;

                    return;
                }

                m_staticHeader = (int) header;
                m_splittedPacketLength = len;
                m_splittedPacketId = id;
                m_splittedPacket = true;
                return;
            }

            if (m_staticHeader != -1)
            {
                m_splittedPacketLength = GetMessageLengthByHeader((uint) m_staticHeader);
                m_staticHeader = -1;
            }
            if (m_buffer.BytesAvailable >= m_splittedPacketLength)
            {
                Message message = MessageReceiver.BuildMessage(m_splittedPacketId,
                                                             m_buffer.ReadBytesInNewBigEndianReader(
                                                                 (int) m_splittedPacketLength));

                NotifyMessageReceived(message);
                m_splittedPacket = false;

                goto replay;
            }
        }

        private static uint GetMessageIdByHeader(uint header)
        {
            return header >> BIT_RIGHT_SHIFT_LEN_PACKET_ID;
        }

        private uint GetMessageLengthByHeader(uint header)
        {
            uint lenType = header & BIT_MASK;
            uint len = 0;
            switch (lenType)
            {
                case 0:
                {
                    break;
                }
                case 1:
                {
                    len = m_buffer.ReadByte();
                    break;
                }
                case 2:
                {
                    len = m_buffer.ReadUShort();
                    break;
                }
                case 3:
                {
                    len =
                        (uint)
                        (((m_buffer.ReadByte() & 255) << 16) + ((m_buffer.ReadByte() & 255) << 8) +
                         (m_buffer.ReadByte() & 255));
                    break;
                }
                default:
                {
                    break;
                }
            }
            return len;
        }

        #endregion

        #region Public Methods

        public void Connect()
        {
            m_socket.BeginConnect(IpEndPoint, ConnectCallBack, null);
        }

        public void Send(Message message)
        {
            var writer = new BigEndianWriter();
            message.pack(writer);

            Send(writer.Data);
        }

        public void Send(byte[] data)
        {
            AsyncSend(data);
        }

        private object m_syncDisconnect = new object();
        public void Disconnect()
        {
            try
            {
                lock (m_syncDisconnect)
                {
                    if (m_socket.Connected)
                        m_socket.Disconnect(true);
                }

                NotifyServerDisconnected();
            }
            catch (SocketException ex)
            {
                logger.Error(string.Format("Error on disconnection : {0}", ex.Message));
            }
        }

        #endregion

        public IPEndPoint IpEndPoint
        {
            get;
            set;
        }

        public Socket Socket
        {
            get { return m_socket; }
        }

        public int BytesReaded
        {
            get { return m_bytesReaded; }
        }

        public int BytesSended
        {
            get { return m_bytesSended; }
        }

        public event ConnectionHandler ServerConnected;

        private void NotifyServerConnected()
        {
            ConnectionHandler handler = ServerConnected;
            if (handler != null) handler(this);
        }

        public event MessageHandler MessageReceived;

        private void NotifyMessageReceived(Message message)
        {
            MessageHandler handler = MessageReceived;
            if (handler != null) handler(this, message);
        }

        public event ConnectionHandler ServerDisconnected;

        private void NotifyServerDisconnected()
        {
            ConnectionHandler handler = ServerDisconnected;
            if (handler != null) handler(this);
        }
    }
}