// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using NLog;
using Stump.BaseCore.Framework.Utils;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Tools.Proxy.Network.Server;

namespace Stump.Tools.Proxy.Network
{
    public class ProxyClient : BaseClient
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Stack<Message> m_receivedMessagesStack = new Stack<Message>(20);

        private readonly Stack<Message> m_sendedMessagesStack = new Stack<Message>(20);

        private ServerConnection m_serverConnection;

        public ServerConnection Server
        {
            get { return m_serverConnection; }
        }

        public ProxyClient(Socket socket)
            : base(socket)
        {
            MessageReceived += OnClientMessageReceived;

            CanReceive = true;
        }

        public Stack<Message> ReceivedMessagesStack
        {
            get { return m_receivedMessagesStack; }
        }

        public Stack<Message> SendedMessagesStack
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

        protected override void OnReceive()
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
                        Message message = MessageReceiver.GetMessage(id,
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
                Message message = MessageReceiver.GetMessage(m_splittedPacketId,
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
        }

        private void OnServerMessageReceived(ServerConnection connection, Message message)
        {
            try
            {
                m_receivedMessagesStack.Push(message);

                if (Proxy.Instance.HandlerManager.IsRegister(message.GetType()))
                    Proxy.Instance.QueueDispatcher.Enqueue(this, message);
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
        }

        private void OnClientMessageReceived(BaseClient client, Message message)
        {
            try
            {
                m_sendedMessagesStack.Push(message);

                if (Proxy.Instance.HandlerManager.IsRegister(message.GetType()))
                    Proxy.Instance.QueueDispatcher.Enqueue(this, message);
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
    }
}