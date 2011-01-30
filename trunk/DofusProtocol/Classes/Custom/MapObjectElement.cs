using Stump.BaseCore.Framework.IO;

namespace Stump.DofusProtocol.Classes.Custom
{
    public class MapObjectElement
    {
        public MapObjectElement(ushort cell, BigEndianReader reader)
        {
            Cell = cell;

            ElementId = reader.ReadUInt();

            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();

            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();

            reader.ReadByte();
            reader.ReadByte();
            reader.ReadByte();
            Identifier = reader.ReadUInt();
        }

        public uint ElementId
        {
            get;
            private set;
        }

        public uint Identifier
        {
            get;
            private set;
        }

        public ushort Cell
        {
            get;
            private set;
        }
    }
}