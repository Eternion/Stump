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
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Stump.BaseCore.Framework.IO;
using Stump.Server.BaseServer;
using Stump.DofusProtocol.Messages;
using NLog;

namespace Stump.Tools.Proxy
{
    class ServerConnexion
    {

        #region Properties

        private const byte BIT_RIGHT_SHIFT_LEN_PACKET_ID = 2;
        private const byte BIT_MASK = 3;

        public IPEndPoint IpEndPoint
        {
            get;
            set;
        }

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public Socket Socket
        {
            get { return socket; }
        }

        public delegate void Connected();
        public event Connected OnConnected;

        public delegate void MessageReceived(Message message);
        public event MessageReceived onMessageReceived;

        public delegate void Disconnected();
        public event Disconnected OnDisconnected;

        private byte[] buffer = new byte[1000];
        private readonly BigEndianReader m_buffer = new BigEndianReader();
        private bool m_splittedPacket;
        private uint m_splittedPacketId;
        private uint m_splittedPacketLength;
        private int m_staticHeader;

        private Int32 bytesReaded = 0;

        public Int32 BytesReaded
        { get { return bytesReaded; } }

        private Int32 bytesSended = 0;

        public Int32 BytesSended
        { get { return bytesSended; } }

        #endregion

        #region Initialisation

        public ServerConnexion(String ip, Int16 port)
        {
            this.IpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        public ServerConnexion(IPEndPoint ipEndPoint)
        {
            this.IpEndPoint = ipEndPoint;
        }

        #endregion

        #region CallBacks

        private void Connect_CallBack(IAsyncResult ar)
        {
            try
            {
                this.socket.EndConnect(ar);
                if (OnConnected != null) OnConnected();
                AsyncReceive();
            }
            catch (SocketException ex)
            {
              Console.WriteLine(ex.Message);
            }
        }

        private void Receive_CallBack(IAsyncResult ar)
        {
            try
            {
                int bytesReaded = this.socket.EndReceive(ar);

                if (bytesReaded > 0)
                {
                    m_buffer.Add(buffer, 0, bytesReaded);

                    this.bytesReaded += bytesReaded;

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
               Console.WriteLine( ex.Message);
                Disconnect();
            }

        }

        private void Send_CallBack(IAsyncResult ar)
        {
            try
            {
                bytesSended += this.socket.EndSend(ar);
            }
            catch (SocketException ex)
            {
               Console.WriteLine( ex.Message);
                Disconnect();
            }
        }

        #endregion

        #region Private Methods

        private void AsyncReceive()
        {
            this.socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(Receive_CallBack), null);
        }

        private void AsyncSend(byte[] data)
        {
            this.socket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(Send_CallBack), null);
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
                        if (onMessageReceived != null) onMessageReceived(
                                                 MessageReceiver.GetMessage(id,
                                                                            m_buffer.ReadBytesInNewBigEndianReader((int)len)));
                        goto replay;
                    }

                    m_staticHeader = -1;
                    m_splittedPacketLength = len;
                    m_splittedPacketId = id;
                    m_splittedPacket = true;

                    return;
                }

                m_staticHeader = (int)header;
                m_splittedPacketLength = len;
                m_splittedPacketId = id;
                m_splittedPacket = true;
                return;
            }

            if (m_staticHeader != -1)
            {
                m_splittedPacketLength = GetMessageLengthByHeader((uint)m_staticHeader);
                m_staticHeader = -1;
            }
            if (m_buffer.BytesAvailable >= m_splittedPacketLength)
            {
                if (onMessageReceived != null) onMessageReceived(
                                     MessageReceiver.GetMessage(m_splittedPacketId,
                                                                m_buffer.ReadBytesInNewBigEndianReader(
                                                                    (int)m_splittedPacketLength)));
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
            socket.BeginConnect(IpEndPoint, new AsyncCallback(Connect_CallBack), null);
        }

        public void Send(Message message)
        {
            var writer = new BigEndianWriter();
            message.pack(writer);
            AsyncSend(writer.Data);
        }

        public void Send(byte[] data)
        {
            AsyncSend(data);
        }


        public void Disconnect()
        {
            try
            {
                this.socket.Disconnect(true);
            }
            catch (SocketException)
            {
            }
            if (OnDisconnected != null) OnDisconnected();
        }

        #endregion

    }
}
