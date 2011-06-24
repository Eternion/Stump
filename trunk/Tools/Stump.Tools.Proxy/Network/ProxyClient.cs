
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using NLog;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;

namespace Stump.Tools.Proxy.Network
{
    public class ProxyClient : BaseClient
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly List<Message> m_receivedMessagesStack = new List<Message>(20);

        private readonly List<Message> m_sendedMessagesStack = new List<Message>(20);
        private readonly object m_syncLock = new object();
        protected bool m_isInCriticalZone;

        private ServerConnection m_serverConnection;

        public ProxyClient(Socket socket)
            : base(socket)
        {
            MessageReceived += OnClientMessageReceived;

            CanReceive = true;
        }

        public ServerConnection Server
        {
            get { return m_serverConnection; }
        }

        public bool IsInCriticalZone
        {
            get { return m_isInCriticalZone; }
            set { m_isInCriticalZone = value; }
        }

        public List<Message> ReceivedMessagesStack
        {
            get { return m_receivedMessagesStack; }
        }

        public List<Message> SendedMessagesStack
        {
            get { return m_sendedMessagesStack; }
        }

        public bool IsBinded
        {
            get;
            private set;
        }

        public void BindToServer(IPEndPoint ipEndPoint)
        {
            m_serverConnection = new ServerConnection(ipEndPoint);

            m_serverConnection.ServerConnected += OnServerConnected;
            m_serverConnection.ServerDisconnected += OnServerDisconnected;
            m_serverConnection.MessageReceived += OnServerMessageReceived;

            m_serverConnection.Connect();
        }

        public bool HasSent(Type message)
        {
            return HasSent(message, 1);
        }

        public bool HasSent(Type message, int range)
        {
            return m_sendedMessagesStack.FindIndex(0, range, entry => entry.GetType() == message) != -1;
        }

        public bool HasReceive(Type message)
        {
            return HasReceive(message, 1);
        }

        public bool HasReceive(Type message, int range)
        {
            return m_receivedMessagesStack.FindIndex(0, range, entry => entry.GetType() == message) != -1;
        }

        protected override void BuildMessage()
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

                m_splittedPacket = false;

                NotifyMessageReceived(message);
                goto replay;
            }
        }

        private void OnServerConnected(ServerConnection connection)
        {
            IsBinded = true;
        }

        private void OnServerDisconnected(ServerConnection connection)
        {
            IsBinded = false;
            if (Socket != null)
                Disconnect();
        }

        private void OnServerMessageReceived(ServerConnection connection, Message message)
        {
            bool mustExitCriticalZone = false;

            if (IsInCriticalZone)
            {
                Monitor.Enter(m_syncLock);
                mustExitCriticalZone = true;
            }

            try
            {
                if (!(message is BasicNoOperationMessage))
                {
                    m_receivedMessagesStack.TrimExcess();
                    m_receivedMessagesStack.Insert(0, message);
                }

                if (Proxy.Instance.HandlerManager.IsRegister(message.GetType()))
                    Proxy.Instance.HandlerManager.Dispatch(this, message);
                else
                    Send(message);
            }
            catch (Exception e)
            {
                logger.Error("Forced disconnection of client {0} : {1}", this, e.Message);

                if (Server.Socket != null)
                    Server.Disconnect();

                if (Socket != null)
                    Disconnect();
            }
            finally
            {
                if (mustExitCriticalZone)
                    Monitor.Exit(m_syncLock);
            }
        }

        private void OnClientMessageReceived(BaseClient client, Message message)
        {
            try
            {
                if (!(message is BasicNoOperationMessage))
                {
                    m_sendedMessagesStack.TrimExcess();
                    m_sendedMessagesStack.Insert(0, message);
                }

                if (Proxy.Instance.HandlerManager.IsRegister(message.GetType()))
                    Proxy.Instance.HandlerManager.Dispatch(this, message);
                else
                {
                    if (!IsBinded)
                        throw new Exception("Attempt to send a packet to the server but the client is not bind to it");

                    m_serverConnection.Send(message);
                }
            }
            catch (Exception e)
            {
                logger.Error("Forced disconnection of client {0} : {1}", this, e.Message);

                if (Server.Socket != null)
                    Server.Disconnect();

                if (Socket != null)
                    Disconnect();
            }
        }

        protected override void OnDisconnect()
        {
            if (m_serverConnection.Socket != null && m_serverConnection.Socket.Connected)
                m_serverConnection.Disconnect();

            base.OnDisconnect();
        }
    }
}