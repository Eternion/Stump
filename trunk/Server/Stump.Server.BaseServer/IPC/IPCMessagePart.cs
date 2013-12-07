using System;
using System.IO;
using Stump.Core.IO;

namespace Stump.Server.BaseServer.IPC
{
    public class IPCMessagePart
    {
        private byte[] m_data;
        private bool m_dataMissing;

        /// <summary>
        ///     Set to true when the message is whole
        /// </summary>
        public bool IsValid
        {
            get
            {
                return LengthBytesCount.HasValue && Length.HasValue && Data != null &&
                       Length == Data.Length;
            }
        }


        public byte? LengthBytesCount
        {
            get;
            private set;
        }

        public int? Length
        {
            get;
            private set;
        }

        /// <summary>
        ///     Set only if ReadData or ExceedBufferSize is true
        /// </summary>
        public byte[] Data
        {
            get { return m_data; }
            private set { m_data = value; }
        }

        /// <summary>
        ///     Build or continue building the message. Returns true if the resulted message is valid and ready to be parsed
        /// </summary>
        public bool Build(byte[] buffer, int count, int length)
        {
            var reader = new BinaryReader(new MemoryStream(buffer, count, length));

            if (length <= 0)
                return false;

            if (IsValid)
                return true;

            if (!LengthBytesCount.HasValue && length < 1)
                return false;

            if (length >= 1 && !LengthBytesCount.HasValue)
                LengthBytesCount = reader.ReadByte();

            if (LengthBytesCount.HasValue &&
                length >= LengthBytesCount && !Length.HasValue)
            {
                Length = 0;

                for (int i = LengthBytesCount.Value - 1; i >= 0; i--)
                {
                    Length |= reader.ReadByte() << (i*8);
                }
            }

            // first case : no data read
            if (Length.HasValue && !m_dataMissing)
            {
                if (Length == 0)
                {
                    Data = new byte[0];
                    return true;
                }

                // enough bytes in the buffer to build a complete message
                if (length >= Length)
                {
                    Data = reader.ReadBytes(Length.Value);

                    return true;
                }
                    // not enough bytes, so we read what we can
                if (Length > length)
                {
                    Data = reader.ReadBytes((int) length);

                    m_dataMissing = true;
                    return false;
                }
            }

                //second case : the message was split and it missed some bytes
            else if (Length.HasValue && m_dataMissing)
            {
                // still miss some bytes ...
                if (Data.Length + length < Length)
                {
                    int lastLength = m_data.Length;
                    Array.Resize(ref m_data, (int) (Data.Length + length));
                    byte[] array = reader.ReadBytes((int) length);

                    Array.Copy(array, 0, Data, lastLength, array.Length);

                    m_dataMissing = true;
                }
                // there is enough bytes in the buffer to complete the message :)
                if (Data.Length + length >= Length)
                {
                    int bytesToRead = Length.Value - Data.Length;


                    Array.Resize(ref m_data, Data.Length + bytesToRead);
                    byte[] array = reader.ReadBytes(bytesToRead);

                    Array.Copy(array, 0, Data, Data.Length - bytesToRead, bytesToRead);
                }
            }

            return IsValid;
        }
    }
}