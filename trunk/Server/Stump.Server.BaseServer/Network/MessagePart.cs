using System;
using System.Diagnostics;
using Stump.Core.Extensions;
using Stump.Core.IO;
using System.Linq;

namespace Stump.Server.BaseServer.Network
{
    public class MessagePart
    {
        /// <summary>
        /// Set to true when the message is whole
        /// </summary>
        public bool IsValid
        {
            get
            {
                return Header.HasValue && Length.HasValue &&
                       Length == Data.Length;
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

        public byte[] Data
        {
            get { return m_data; }
            private set { m_data = value; }
        }

        /// <summary>
        /// Build or continue building the message. Returns true if the resulted message is valid and ready to be parsed
        /// </summary>
        public bool Build(BigEndianReader reader)
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
                for (int i = LengthBytesCount.Value - 1; i >= 0; i--)
                {
                    Length |= reader.ReadByte() << (i * 8);
                }
            }

            // first case : no data read
            if (Data == null && Length.HasValue)
            {
                if (Length == 0)
                    Data = new byte[0];

                // enough bytes in the buffer to build a complete message
                if (reader.BytesAvailable >= Length)
                {
                    Data = reader.ReadBytes(Length.Value);
                    return true;
                }
                // not enough bytes, so we read what we can
                else if (Length > reader.BytesAvailable)
                {
                    Data = reader.ReadBytes((int) reader.BytesAvailable);
                    return false;
                }
            }
            //second case : the message was split and it missed some bytes
            if (Data != null && Length.HasValue && Data.Length < Length)
            {
                // still miss some bytes ...
                if (Data.Length + reader.BytesAvailable < Length)
                {
                    //Debug.WriteLine("Data.Length + reader.BytesAvailable < Length");
                    //Debug.WriteLine("BytesAvailable = " + reader.BytesAvailable + " ; Data.Length = " + Data.Length);

                    int lastLength = m_data.Length;
                    Array.Resize(ref m_data, (int)( Data.Length + reader.BytesAvailable ));
                    var array = reader.ReadBytes((int)reader.BytesAvailable);
                    //Debug.WriteLine("array content : " + String.Join(" ", array.Select(entry => entry.ToString("X"))));

                    Array.Copy(array, 0, Data, lastLength, array.Length);
                }
                // there is enough bytes in the buffer to complete the message :)
                if (Data.Length + reader.BytesAvailable >= Length)
                {
                    //Debug.WriteLine("Data.Length + reader.BytesAvailable >= Length");
                    int bytesToRead = Length.Value - Data.Length;

                    //Debug.WriteLine("bytesToRead = " + bytesToRead + " ; Data.Length = " + Data.Length);

                    Array.Resize(ref m_data, Data.Length + bytesToRead);
                    var array = reader.ReadBytes(bytesToRead);

                    //Debug.WriteLine("array content : " + String.Join(" ", array.Select(entry => entry.ToString("X"))));

                    Array.Copy(array, 0, Data, Data.Length - bytesToRead, bytesToRead);

                    //Debug.WriteLine("Data content : " + String.Join(" ", array.Select(entry => entry.ToString("X"))));
                }
            }

            return IsValid;
        }
    }
}