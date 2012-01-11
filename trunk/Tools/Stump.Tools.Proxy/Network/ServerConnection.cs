
using System;
using System.Net;
using System.Net.Sockets;
using NLog;
using Stump.Core.IO;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;

namespace Stump.Tools.Proxy.Network
{
    public class ServerConnection
    {
        #region Delegates

        public delegate void ConnectionHandler(ServerConnection connection);

        public delegate void MessageHandler(ServerConnection connection, Message message);

        #endregion

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly BigEndianReader m_buffer = new BigEndianReader();
        private readonly byte[] m_localBuffer = new byte[1024];
        private readonly Socket m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private int m_bytesReaded;
        private int m_bytesSended;

        private MessagePart m_currentMessage;

        private object m_sync = new object();

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
            if (!m_socket.Connected)
                return;

            try
            {
                int availableBytes = m_socket.EndReceive(ar);

                if (availableBytes > 0)
                {
                    lock (m_sync)
                        m_buffer.Add(m_localBuffer, 0, availableBytes);

                    m_bytesReaded += availableBytes;

                    BuildMessage();

                    if (m_socket.Connected)
                        AsyncReceive();
                }
                else
                {
                    Disconnect();
                }
            }
            catch (Exception ex)
            {
                logger.Error(string.Format("Error on receive : {0}", ex));
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

        private void BuildMessage()
        {
            if (m_buffer.BytesAvailable <= 0)
                return;

            if (m_currentMessage == null)
                m_currentMessage = new MessagePart();

            // if message is complete
            lock (m_sync)
                if (!m_currentMessage.Build(m_buffer))
                    return;

            var messageDataReader = new BigEndianReader(m_currentMessage.Data);
            Message message = MessageReceiver.BuildMessage((uint)m_currentMessage.MessageId.Value, messageDataReader);

            NotifyMessageReceived(message);

            m_currentMessage = null;

            BuildMessage(); // there is maybe a second message in the buffer
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
            message.Pack(writer);

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