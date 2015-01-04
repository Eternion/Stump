using System;
using Stump.Core.IO;
using NLog;

namespace Stump.DofusProtocol.Messages
{
    public abstract class Message
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private const byte BIT_RIGHT_SHIFT_LEN_PACKET_ID = 2;
        private const byte BIT_MASK = 3;

        public abstract uint MessageId
        {
            get;
        }

        public void Unpack(IDataReader reader)
        {
            Deserialize(reader);
        }

        public void Pack(IDataWriter writer)
        {
            byte typeLen = 3;
            var header = (short)SubComputeStaticHeader(MessageId, typeLen);

            writer.WriteShort(header);

            for (int i = typeLen - 1; i >= 0; i--)
            {
                writer.WriteByte(0);
            }

            Serialize(writer);
            var len = writer.Position - 5;
            writer.Seek(2);

            for (int i = typeLen - 1; i >= 0; i--)
            {
                writer.WriteByte((byte)(len >> 8 * i & 255));
            }

            writer.Seek((int)len + 5);
        }

        public abstract void Serialize(IDataWriter writer);
        public abstract void Deserialize(IDataReader reader);

        private static byte ComputeTypeLen(int param1)
        {
            if (param1 > 65535)
            {
                return 3;
            }
            if (param1 > 255)
            {
                return 2;
            }
            if (param1 > 0)
            {
                return 1;
            }
            return 0;
        }

        private static uint SubComputeStaticHeader(uint id, byte typeLen)
        {
            return id << BIT_RIGHT_SHIFT_LEN_PACKET_ID | typeLen;
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}