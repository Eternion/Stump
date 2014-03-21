using System;
using Stump.Core.IO;

namespace Stump.Server.BaseServer.Network
{
    public class MessagePart
    {
        private readonly bool m_readData;
        private long m_availableBytes;

        public MessagePart(bool readData)
        {
            m_readData = readData;
        }

        private bool m_dataMissing;

        /// <summary>
        /// Set to true when the message is whole
        /// </summary>
        public bool IsValid
        {
            get
            {
                return Header.HasValue && Length.HasValue && (!ReadData || Data != null) &&
                    Length == (ReadData ? Data.Length : m_availableBytes);
            }
        }

        public int? Header
        {
            get;
            private set;
        }

        public int? MessageId
        {
            get
            {
                if (!Header.HasValue)
                    return null;

                return Header >> 2; // xxxx xx??
            }
        }

        public int? LengthBytesCount
        {
            get
            {
                if (!Header.HasValue)
                    return null;

                return Header & 0x3; // ???? ??xx
            }
        }

        public int? Length
        {
            get;
            private set;
        }

        private byte[] m_data;

        /// <summary>
        /// Set only if ReadData or ExceedBufferSize is true
        /// </summary>
        public byte[] Data
        {
            get { return m_data; }
            private set { m_data = value; }
        }

        public bool ExceedBufferSize
        {
            get { return Length.HasValue && Length.Value > (ClientManager.BufferSize - 2 - LengthBytesCount.Value); }
        }

        public bool ReadData
        {
            get { return m_readData || ExceedBufferSize; }
        }

        /// <summary>
        /// Build or continue building the message. Returns true if the resulted message is valid and ready to be parsed
        /// </summary>
        public bool Build(IDataReader reader)
        {
            if (reader.BytesAvailable <= 0)
                return false;

            if (IsValid)
                return true;

            if (!Header.HasValue && reader.BytesAvailable < 2)
                return false;

            if (reader.BytesAvailable >= 2 && !Header.HasValue)
            {
                Header = reader.ReadShort();
            }

            if (LengthBytesCount.HasValue &&
                reader.BytesAvailable >= LengthBytesCount && !Length.HasValue)
            {
                if (LengthBytesCount < 0 || LengthBytesCount > 3)
                    throw new Exception("Malformated Message Header, invalid bytes number to read message length (inferior to 0 or superior to 3)");

                Length = 0;
                
                // 3..0 or 2..0 or 1..0
                for (var i = LengthBytesCount.Value - 1; i >= 0; i--)
                {
                    Length |= reader.ReadByte() << (i * 8);
                }
            }

            // first case : no data read
            if (Length.HasValue && !m_dataMissing)
            {
                if (Length == 0)
                {
                    if (ReadData)
                        Data = new byte[0];
                    return true;
                }

                // enough bytes in the buffer to build a complete message
                if (reader.BytesAvailable >= Length)
                {
                    if (ReadData)
                        Data = reader.ReadBytes(Length.Value);
                    else
                        m_availableBytes = reader.BytesAvailable;

                    return true;
                }

                // not enough bytes, so we read what we can
                if (!(Length > reader.BytesAvailable))
                    return IsValid;

                if (ReadData)
                    Data = reader.ReadBytes((int) reader.BytesAvailable);
                else
                    m_availableBytes = reader.BytesAvailable;

                m_dataMissing = true;
                return false;
            }

            //second case : the message was split and it missed some bytes
            if (!Length.HasValue || !m_dataMissing)
                return IsValid;

            // still miss some bytes ...
            if ((ReadData ? Data.Length : 0) + reader.BytesAvailable < Length)
            {
                if (ReadData)
                {
                    var lastLength = m_data.Length;
                    Array.Resize(ref m_data, (int) (Data.Length + reader.BytesAvailable));
                    var array = reader.ReadBytes((int) reader.BytesAvailable);

                    Array.Copy(array, 0, Data, lastLength, array.Length);
                }
                else
                    m_availableBytes = reader.BytesAvailable;

                m_dataMissing = true;
            }

            // there is enough bytes in the buffer to complete the message :)
            if (!((ReadData ? Data.Length : 0) + reader.BytesAvailable >= Length))
                return IsValid;

            if (ReadData)
            {
                var bytesToRead = Length.Value - Data.Length;


                Array.Resize(ref m_data, Data.Length + bytesToRead);
                var array = reader.ReadBytes(bytesToRead);

                Array.Copy(array, 0, Data, Data.Length - bytesToRead, bytesToRead);
            }
            else
                m_availableBytes = reader.BytesAvailable;

            return IsValid;
        }
    }
}