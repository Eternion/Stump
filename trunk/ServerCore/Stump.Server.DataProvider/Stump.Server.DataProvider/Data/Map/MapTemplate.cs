// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using ProtoBuf;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.D2oClasses;

namespace Stump.Server.DataProvider.Data.Map
{
    [Serializable, ProtoContract]
    public class MapTemplate
    {
        private const uint MaximumCellsCount = 560;

        public MapTemplate()
        {         
        }

        public MapTemplate(BigEndianReader reader)
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
            get; private set;
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
            set;
        }

        [ProtoMember(4)]
        public int SubAreaId
        {
            get;
            internal set;
        }

        [ProtoMember(5)]
        public uint TopNeighbourId
        {
            get;
             set;
        }

        [ProtoMember(6)]
        public uint BottomNeighbourId
        {
            get;
            internal set;
        }

        [ProtoMember(7)]
        public uint LeftNeighbourId
        {
            get;
            internal set;
        }

        [ProtoMember(8)]
        public uint RightNeighbourId
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

        public System.Drawing.Point Position { get; set; }

        public MapCapabilities Capabilities { get; set; }
    }
}
