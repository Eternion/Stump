using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
    public abstract class Message
    {
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
            var len = GetSerializationSize();
            byte typeLen = ComputeTypeLen(len);
            var header = (short)SubComputeStaticHeader(MessageId, typeLen);

            writer.WriteShort(header);

            for (int i = typeLen - 1; i >= 0; i--)
            {
                writer.WriteByte((byte)(len >> 8*i & 255));
            }
            Serialize(writer);
        }

        public abstract void Serialize(IDataWriter writer);
        public abstract void Deserialize(IDataReader reader);
        public abstract int GetSerializationSize();

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