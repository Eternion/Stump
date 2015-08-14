using System;
using System.IO;
using NLog;
using Stump.Core.IO;

namespace Stump.Server.BaseServer.IPC
{
    public class IPCMessagePart
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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
        public bool Build(IDataReader reader)
        {
            if (reader.BytesAvailable <= 0)
                return false;

            if (IsValid)
                return true;

            if (!LengthBytesCount.HasValue && reader.BytesAvailable < 1)
                return false;

            if (reader.BytesAvailable >= 1 && !LengthBytesCount.HasValue)
            {
                LengthBytesCount = reader.ReadByte();

                if (LengthBytesCount > 3)
                    logger.Error("Invalid message LengthBytesCount = {0}", LengthBytesCount);
            } 

            if (LengthBytesCount.HasValue &&
                reader.BytesAvailable >= LengthBytesCount && !Length.HasValue)
            {
                Length = 0;

                for (var i = LengthBytesCount.Value - 1; i >= 0; i--)
                {
                    Length |= reader.ReadByte() << (i*8);
                }

                if (Length < 0)
                    logger.Error("Invalid message Length = {0}", LengthBytesCount);
            }

            if (reader.BytesAvailable <= 0)
                return IsValid;

            // first case : no data read
            if (Length.HasValue && !m_dataMissing)
            {
                if (Length == 0)
                {
                    Data = new byte[0];
                    return true;
                }

                // enough bytes in the buffer to build a complete message
                if (reader.BytesAvailable >= Length)
                {
                    Data = reader.ReadBytes(Length.Value);

                    return true;
                }

                // not enough bytes, so we read what we can
                Data = reader.ReadBytes((int) reader.BytesAvailable);

                m_dataMissing = true;
                return false;
            }

            //second case : the message was split and it missed some bytes
            if (!Length.HasValue || !m_dataMissing)
                return IsValid;

            // still miss some bytes ...
            if (Data.Length + reader.BytesAvailable < Length)
            {
                var lastLength = m_data.Length;
                Array.Resize(ref m_data, (int) (Data.Length + reader.BytesAvailable));

                if (reader.BytesAvailable < 0)
                    return false;

                var array = reader.ReadBytes((int) reader.BytesAvailable);

                Array.Copy(array, 0, Data, lastLength, array.Length);

                m_dataMissing = true;
                return false;
            }

            // there is enough bytes in the buffer to complete the message :)
            if (Data.Length + reader.BytesAvailable >= Length)
            {
                var bytesToRead = Length.Value - Data.Length;

                Array.Resize(ref m_data, Data.Length + bytesToRead);

                if (bytesToRead < 0)
                    return false;

                var array = reader.ReadBytes(bytesToRead);

                Array.Copy(array, 0, Data, Data.Length - bytesToRead, bytesToRead);
            }

            return IsValid;
        }
    }
}