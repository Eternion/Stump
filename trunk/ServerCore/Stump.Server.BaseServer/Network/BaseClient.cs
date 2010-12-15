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
using System.Net.Sockets;
using NLog;
using Stump.BaseCore.Framework.IO;
using Stump.BaseCore.Framework.Pool;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.BaseServer.Network
{
    public abstract class BaseClient : IPacketReceiver
    {
        private const byte BIT_RIGHT_SHIFT_LEN_PACKET_ID = 2;
        private const byte BIT_MASK = 3;

        private static SocketAsyncEventArgsPool m_argsPool;
        private static QueueDispatcher m_dispatcher;

        private static bool m_isInitialized;

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly BigEndianReader m_buffer = new BigEndianReader();
        private bool m_splittedPacket;
        private uint m_splittedPacketId;
        private uint m_splittedPacketLength;
        private int m_staticHeader;

        protected BaseClient(Socket socket)
        {
            if (!m_isInitialized)
                throw new Exception("BaseClient is not initialized");

            Socket = socket;
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
            get { return Socket.Connected; }
        }

        public string IP
        {
            get { return Socket.RemoteEndPoint != null ? Socket.RemoteEndPoint.ToString() : "null"; }
        }

        internal static void Initialize(ref SocketAsyncEventArgsPool pool, ref QueueDispatcher dispatcher)
        {
            m_argsPool = pool;
            m_dispatcher = dispatcher;

            m_isInitialized = true;
        }

        internal void ProcessReceive(byte[] data, int offset, int count)
        {
            try
            {
                if (!CanReceive)
                    throw new Exception("Cannot receive packet : CanReceive is false");

                m_buffer.Add(data, offset, count);

                Receive();
            }
            catch (Exception ex)
            {
                logger.Error("Forced disconnection " + ToString() + " : " + ex.Message);

#if DEBUG
                logger.Error("Source : {0} Method : {1}", ex.Source, ex.TargetSite);
                logger.Error("Stack Trace : " + ex.StackTrace);
#endif
            }
        }

        // local temp variables

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
                        m_dispatcher.Enqueue(this,
                                             MessageReceiver.GetMessage(id,
                                                                        m_buffer.ReadBytesInNewBigEndianReader((int) len)));
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
                m_dispatcher.Enqueue(this,
                                     MessageReceiver.GetMessage(m_splittedPacketId,
                                                                m_buffer.ReadBytesInNewBigEndianReader(
                                                                    (int) m_splittedPacketLength)));
                m_splittedPacket = false;

                goto replay;
            }
        }

        public void Send(Message message)
        {
            /*if (!message._isInitialized)
			{
				logger.Error("Sending non-initialized packet !");
			}*/
            if (!Connected || Socket == null)
            {
                logger.Error("Attempt to send a packet when client isn't connected");
            }
            else
            {
                SocketAsyncEventArgs args = m_argsPool.Pop();

                var writer = new BigEndianWriter();

                message.pack(writer);
                byte[] data = writer.Data;

                args.SetBuffer(data, 0, data.Length);

                if (!Socket.SendAsync(args))
                {
                    m_argsPool.Push(args);
                }
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

        /// <summary>
        ///   Disconnect the Client. Cannot reuse the socket.
        /// </summary>
        public void Disconnect()
        {
            OnDisconnect();

            Close();
        }

        public virtual void OnDisconnect()
        {
            if (Socket != null && Socket.Connected)
                logger.Info("Client disconnected <{0}>.", IP);
        }

        public void Close()
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
            return "<" + IP + ">";
        }
    }
}