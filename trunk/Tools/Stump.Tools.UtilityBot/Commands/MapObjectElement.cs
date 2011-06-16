using ProtoBuf;
using Stump.Core.IO;

namespace Stump.Server.BaseServer.Data.MapTool
{
    [ProtoContract]
    public class MapObjectElement
    {

        public MapObjectElement()
        {
            
        }
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

        [ProtoMember(1)]
        public uint ElementId
        {
            get;
            private set;
        }

         [ProtoMember(2)]
        public uint Identifier
        {
            get;
            private set;
        }

         [ProtoMember(3)]
        public ushort Cell
        {
            get;
            private set;
        }
    }
}