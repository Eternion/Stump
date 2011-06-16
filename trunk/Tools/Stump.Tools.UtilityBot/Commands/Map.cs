
using System;
using System.Collections.Generic;
using ProtoBuf;
using Stump.Core.IO;

namespace Stump.Server.BaseServer.Data.MapTool
{
    [ProtoContract]
    public class Map
    {
        public const uint MaximumCellsCount = 560;

        public Map()
        {
            
        }
        public Map(BigEndianReader reader)
        {
            byte mapversion = reader.ReadByte();
            Id = reader.ReadInt();
            RelativeId = reader.ReadInt();
            MapType = reader.ReadByte();
            SubAreaId = reader.ReadInt();
            TopNeighbourId = reader.ReadInt();
            BottomNeighbourId = reader.ReadInt();
            LeftNeighbourId = reader.ReadInt();
            RightNeighbourId = reader.ReadInt();
            reader.ReadInt();

            if (mapversion >= 3)
            {
                reader.ReadByte();
                reader.ReadByte();
                reader.ReadByte();
            }
            if (mapversion >= 4)
            {
                reader.ReadUShort();
                reader.ReadShort();
                reader.ReadShort();
            }

            reader.ReadByte();

            if (reader.ReadByte() != 0)
                reader.ReadInt();

            int count = reader.ReadByte();
            for (int i = 0; i < count; i++)
            {
                reader.ReadInt();
                reader.ReadShort();
                reader.ReadShort();
                reader.ReadShort();
                reader.ReadShort();
                reader.ReadShort();
                reader.ReadByte();
                reader.ReadByte();
                reader.ReadByte();
                reader.ReadByte();
            }

            count = reader.ReadByte();
            for (int i = 0; i < count; i++)
            {
                reader.ReadInt();
                reader.ReadShort();
                reader.ReadShort();
                reader.ReadShort();
                reader.ReadShort();
                reader.ReadShort();
                reader.ReadByte();
                reader.ReadByte();
                reader.ReadByte();
                reader.ReadByte();
            }

            reader.ReadInt();

            int ground = reader.ReadInt(); // ground


            MapElementsPositions = new Dictionary<uint, MapObjectElement>();
            count = reader.ReadByte();
            for (int i = 0; i < count; i++)
            {
                reader.ReadInt();
                short cellscount = reader.ReadShort();
                for (int l = 0; l < cellscount; l++)
                {
                    ushort cell = reader.ReadUShort();
                    short elemcount = reader.ReadShort();
                    for (int k = 0; k < elemcount; k++)
                    {
                        int type = reader.ReadByte();
                        switch (type)
                        {
                            case 2: // GRAPICAL
                                var moe = new MapObjectElement(cell, reader);
                                if (!MapElementsPositions.ContainsKey(moe.Identifier))
                                    MapElementsPositions.Add(moe.Identifier, moe);
                                break;
                            case 33: // SOUND
                                reader.ReadInt();
                                reader.ReadShort();
                                reader.ReadInt();
                                reader.ReadInt();
                                reader.ReadShort();
                                reader.ReadShort();
                                break;
                            default:
                                throw new Exception("Wrong element type");
                        }
                    }
                }
            }

            CellsData = new List<CellData>((int)MaximumCellsCount);

            for (ushort i = 0; i < MaximumCellsCount; i++)
                CellsData.Add(new CellData(reader, i, Id));
        }

        [ProtoMember(1)]
        public int Id
        {
            get;
            internal set;
        }

        [ProtoMember(2)]
        public int RelativeId
        {
            get;
            internal set;
        }

        [ProtoMember(3)]
        public byte MapType
        {
            get;
            internal set;
        }

        [ProtoMember(4)]
        public int SubAreaId
        {
            get;
            internal set;
        }

        [ProtoMember(5)]
        public int TopNeighbourId
        {
            get;
            internal set;
        }

        [ProtoMember(6)]
        public int BottomNeighbourId
        {
            get;
            internal set;
        }

        [ProtoMember(7)]
        public int LeftNeighbourId
        {
            get;
            internal set;
        }

        [ProtoMember(8)]
        public int RightNeighbourId
        {
            get;
            internal set;
        }

        [ProtoMember(9)]
        public List<CellData> CellsData
        {
            get;
            internal set;
        }

        [ProtoMember(10)]
        public Dictionary<uint, MapObjectElement> MapElementsPositions
        {
            get;
            set;
        }

    }
}
