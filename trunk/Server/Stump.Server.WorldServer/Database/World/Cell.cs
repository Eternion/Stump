using System;
using System.IO;

namespace Stump.Server.WorldServer.Database.World
{
    [Serializable]
    public struct Cell
    {
        public const int StructSize = 2 + 2 + 1 + 1 + 1;

        public short Id;
        public short Floor;
        public byte LosMov;
        public byte MapChangeData;
        public byte Speed;

        public byte[] Serialize()
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            writer.Write(Id);
            writer.Write(Floor);
            writer.Write(LosMov);
            writer.Write(MapChangeData);
            writer.Write(Speed);

            return stream.ToArray();
        }

        public void Deserialize(byte[] data, int index = 0, int count = StructSize)
        {
            var stream = new MemoryStream(data, index, count);
            var reader = new BinaryReader(stream);

            Id = reader.ReadInt16();
            Floor = reader.ReadInt16();
            LosMov = reader.ReadByte();
            MapChangeData = reader.ReadByte();
            Speed = reader.ReadByte();
        }
    }
}