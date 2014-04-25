using System.Collections.Generic;
using System.Diagnostics;
using System;
using PcapDotNet.Packets.Ethernet;
using PcapDotNet.Packets.IpV4;
using PcapDotNet.Packets.Transport;
using Stump.Core.IO;
using Stump.DofusProtocol.Messages;

namespace Stump.Tools.Sniffer
{
    public class IdentifiedClient
    {
        #region Delegates

        public delegate void NewMessage(Message message, string sender);

        #endregion

        private const byte BIT_RIGHT_SHIFT_LEN_PACKET_ID = 2;
        private const byte BIT_MASK = 3;

        private readonly BigEndianReader m_buffer = new BigEndianReader();
        private MessagePart m_currentMessage;

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
            Sended = new List<uint>();
        }

        public string Name
        {
            get { return m_name; }
        }

        public MacAddress Mac
        {
            get;
            set;
        }

        public IpV4Address Ip
        {
            get;
            set;
        }

        public ushort Ipv4Id
        {
            get;
            set;
        }

        public uint TcpId
        {
            get;
            set;
        }

        public uint AckId
        {
            get;
            set;
        }

        public ushort Port
        {
            get;
            set;
        }

        public List<uint> Sended
        {
            get;
            set;
        }

        public static event NewMessage OnNewMessage;

        internal void ProcessReceive(byte[] data, int offset, int count)
        {
            m_buffer.Add(data, offset, count);
            Receive();
        }

        private void Receive()
        {
            if (m_buffer.BytesAvailable <= 0)
                return;

            if (m_currentMessage == null)
                m_currentMessage = new MessagePart();

            // if message is complete
            if (m_currentMessage.Build(m_buffer))
            {
                var messageDataReader = new BigEndianReader(m_currentMessage.Data);
                Message message;
                try
                {
                    message = MessageReceiver.BuildMessage((uint)m_currentMessage.MessageId.Value, messageDataReader);

                    if (message.MessageId == 5632)
                        Debug.Print("Message = {0}", m_currentMessage.Data);

                    if (OnNewMessage != null)
                    {
                        OnNewMessage(message, m_name);
                    }
                }
                catch (Exception)
                {
                    if (m_currentMessage.MessageId == 5632)
                        Debug.Print("Message = {0}", m_currentMessage.Data);
                }

                m_currentMessage = null;

                Receive(); // there is maybe a second message in the buffer
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