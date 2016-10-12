using System;

namespace Stump.Server.WorldServer.Database.World
{
    [Serializable]
    public class Cell
    {
        public const int StructSize = 2 + 2 + 2 + 1 + 1 + 4;

        public short Floor;
        public short Id;
        public short Data;
        public byte MapChangeData;
        public uint MoveZone;
        public byte Speed;

        public bool Walkable => (Data & 1) != 0;

        public bool NonWalkableDuringFight => (Data & 2) != 0;

        public bool NonWalkableDuringRP => (Data & 4) != 0;

        public bool LineOfSight => (Data & 8) != 0;

        public bool Blue => (Data & 16) != 0;

        public bool Red => (Data & 32) != 0;

        public bool FarmCell => (Data & 64) != 0;

        public bool Visible => (Data & 128) != 0;

        public bool HavenbagCell => (Data & 256) != 0;
        
        public byte[] Serialize()
        {
            var bytes = new byte[StructSize];

            bytes[0] = (byte) (Id >> 8);
            bytes[1] = (byte) (Id & 0xFF);

            bytes[2] = (byte) (Floor >> 8);
            bytes[3] = (byte) (Floor & 0xFF);
            
            bytes[4] = (byte)(Data >> 8);
            bytes[5] = (byte)(Data & 0xFF);
            bytes[6] = MapChangeData;
            bytes[7] = Speed;

            bytes[8] = (byte) (MoveZone >> 24);
            bytes[9] = (byte) (MoveZone >> 16);
            bytes[10] = (byte) (MoveZone >> 8);
            bytes[11] = (byte) (MoveZone & 0xFF);

            return bytes;
        }

        public void Deserialize(byte[] data, int index = 0)
        {
            Id = (short) ((data[index + 0] << 8) | data[index + 1]);

            Floor = (short) ((data[index + 2] << 8) | data[index + 3]);

            Data = (short) ((data[index + 4] << 8) | data[index + 5]);
            MapChangeData = data[index + 6];
            Speed = data[index + 7];

            MoveZone =
                (uint) ((data[index + 8] << 24) | (data[index + 9] << 16) | (data[index + 10] << 8) | (data[index + 11]));
        }
    }
}