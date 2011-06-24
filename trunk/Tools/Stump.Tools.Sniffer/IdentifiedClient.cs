
using System;
using Stump.Core.IO;
using Stump.DofusProtocol.Messages.Framework.IO;
using Stump.DofusProtocol.Messages;

namespace Stump.Tools.Sniffer
{
    internal class IdentifiedClient
    {
        #region Delegates

        public delegate void NewMessage(Message message, string sender);

        #endregion

        private const byte BIT_RIGHT_SHIFT_LEN_PACKET_ID = 2;
        private const byte BIT_MASK = 3;

        private readonly BigEndianReader m_buffer = new BigEndianReader();

        private readonly string m_name;

        //Indique si le Paquet est Tronqué
        private bool m_splittedPacket;
        //L'header du paquet
        // L'ID du Paquet tronqué
        private uint m_splittedPacketId;
        // La taille du paquet tronqué
        private uint m_splittedPacketLength;
        private int m_staticHeader;

        public IdentifiedClient(string name)
        {
            m_name = name;
        }

        public string Name
        {
            get { return m_name; }
        }

        public static event NewMessage OnNewMessage;

        internal void ProcessReceive(byte[] data, int offset, int count)
        {
            m_buffer.Add(data, offset, count);
            Receive();
        }

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
                        Message message = MessageReceiver.BuildMessage(id,m_buffer.ReadBytesInNewBigEndianReader((int) len));
                        if (OnNewMessage != null)
                            OnNewMessage(message, m_name);

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
                if (OnNewMessage != null)
                    OnNewMessage(message, m_name);

                m_splittedPacket = false;

                goto replay;
            }
        }

        ///<summary>
        ///  Obtient l'ID du Message grâce à son entête
        ///</summary>
        ///<param name = "header">Header du Paquet</param>
        ///<returns>Id Du Message</returns>
        private uint GetMessageIdByHeader(uint header)
        {
            return header >> BIT_RIGHT_SHIFT_LEN_PACKET_ID;
        }

        /// <summary>
        ///   Obtient la taille du paquet grâce à son entête
        /// </summary>
        /// <param name = "header">Header du Paquet</param>
        /// <returns>Taille du message</returns>
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
    }
}