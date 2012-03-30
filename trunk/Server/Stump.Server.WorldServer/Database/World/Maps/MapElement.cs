using System;

namespace Stump.Server.WorldServer.Database.World.Maps
{
    public struct MapElement
    {
        public const int Size = 2 + 4;

        public short CellId;
        public uint ElementId;

        public MapElement(uint elementId, short cellId)
        {
            ElementId = elementId;
            CellId = cellId;
        }

        public byte[] Serialize()
        {
            var data = new byte[6];

            data[0] = (byte) (CellId >> 8);
            data[1] = (byte) (CellId & 0xFF);

            data[2] = (byte)( ElementId >> 24 );
            data[3] = (byte)( ( ElementId >> 16) & 0xFF);
            data[4] = (byte)( ( ElementId >> 8 ) & 0xFF );
            data[5] = (byte)( ElementId & 0xFF );

            return data;
        }

        public void Deserialize(byte[] data, int index)
        {
            CellId = (short)( data[index + 0] << 8 | data[index + 1] );
            ElementId = unchecked((uint)( data[index + 2] << 24 | data[index + 3] << 16 | data[index + 4] << 8 | data[index + 5] ));
        }
    
    }
}