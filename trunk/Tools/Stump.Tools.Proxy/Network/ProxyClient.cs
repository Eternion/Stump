
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

        public bool HasSent(uint messageId)
        {
            return HasSent(messageId, 1);
        }

        public bool HasSent(uint messageId, int range)
        {
            return m_sendedMessagesStack.FindIndex(0, range, entry => entry.MessageId == messageId) != -1;
        }

        public bool HasReceive(uint messageId)
        {
            return HasReceive(messageId, 1);
        }

        public bool HasReceive(uint messageId, int range)
        {
            return m_receivedMessagesStack.FindIndex(0, range, entry => entry.MessageId == messageId) != -1;
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
                    m_receivedMessagesStack.Insert(0, message);
                }

                if (!Dispatch(message))
                    Proxy.Instance.IOTaskPool.AddMessage(() => Send(message));
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

        protected virtual bool Dispatch(Message message)
        {
            return false;
        }

        protected override void OnMessageReceived(Message message)
        {
            try
            {
                if (!( message is BasicNoOperationMessage ))
                {
                    m_sendedMessagesStack.Insert(0, message);
                }

                if (!Dispatch(message))
                {
                    if (!IsBinded)
                        throw new Exception("Attempt to send a packet to the server but the client is not bind to him");

                    Proxy.Instance.IOTaskPool.AddMessage(() =>m_serverConnection.Send(message));
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
            if (m_serverConnection != null && m_serverConnection.Socket != null && m_serverConnection.Socket.Connected)
                m_serverConnection.Disconnect();

            base.OnDisconnect();
        }
    }
}