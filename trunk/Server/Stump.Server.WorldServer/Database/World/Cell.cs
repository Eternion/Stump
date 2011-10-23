using System;
using System.IO;

namespace Stump.Server.WorldServer.Database.World
{
    [Serializable]
    public struct Cell
    {
        public const int StructSize = 2 + 2 + 1 + 1 + 1;

        public short Floor;
        public short Id;
        public byte LosMov;
        public byte MapChangeData;
        public byte Speed;

        public bool Walkable
        {
            get { return (LosMov & 1) == 1; }
        }

        public bool LineOfSight
        {
            get { return (LosMov & 2) == 2; }
        }

        public bool NonWalkableDuringFight
        {
            get { return (LosMov & 4) == 4; }
        }

        public bool Red
        {
            get { return (LosMov & 8) == 8; }
        }

        public bool Blue
        {
            get { return (LosMov & 16) == 16; }
        }

        public bool FarmCell
        {
            get { return (LosMov & 32) == 32; }
        }

        public bool Visible
        {
            get { return (LosMov & 64) == 64; }
        }

        public bool NonWalkableDuringRP
        {
            get { return (LosMov & 128) == 128; }
        }

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