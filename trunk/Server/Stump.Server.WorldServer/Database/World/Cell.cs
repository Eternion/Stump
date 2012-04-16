using System;
using System.IO;

namespace Stump.Server.WorldServer.Database.World
{
    [Serializable]
    public struct Cell
    {
        /// <summary>
        /// Give a cell with Id = -1. This avoid using a class that take more memory space
        /// </summary>
        public static Cell Null = new Cell
        {
            Id = -1
        };

        public const int StructSize = 2 + 2 + 1 + 1 + 1 + 4;

        public short Floor;
        public short Id;
        public byte LosMov;
        public byte MapChangeData;
        public byte Speed;
        public uint MoveZone;

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
            var bytes = new byte[StructSize];

            bytes[0] = (byte)( Id >> 8 );
            bytes[1] = (byte)( Id & 0xFF );

            bytes[2] = (byte)( Floor >> 8 );
            bytes[3] = (byte)( Floor & 0xFF );

            bytes[4] = LosMov;
            bytes[5] = MapChangeData;
            bytes[6] = Speed;

            bytes[7] = (byte)( MoveZone >> 24 );
            bytes[8] = (byte)( MoveZone >> 16 );
            bytes[9] = (byte)( MoveZone >> 8 );
            bytes[10] = (byte)( MoveZone & 0xFF );

            return bytes;
        }

        public void Deserialize(byte[] data, int index = 0)
        {
            Id = (short) (( data[index + 0] << 8 ) | data[index + 1]);

            Floor = (short)( ( data[index + 2] << 8 ) | data[index + 3] );

            LosMov = data[index + 4];
            MapChangeData = data[index + 5];
            Speed = data[index + 6];

            MoveZone = (uint) ( ( data[index + 7] << 24 ) | ( data[index + 8] << 16 ) | ( data[index + 9] << 8 ) | ( data[index + 10] ) );
        }
    }
}