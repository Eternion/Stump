
using System;
using ProtoBuf;
using Stump.BaseCore.Framework.IO;

namespace Stump.Server.DataProvider.Data.Map
{
    [Serializable,ProtoContract]
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