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
using ProtoBuf;
using Stump.BaseCore.Framework.IO;

namespace Stump.Server.DataProvider.Data.Map
{
    [Serializable,ProtoContract]
    public class CellData
    {

        public CellData()
        {
            
        }

        public CellData(BigEndianReader reader,ushort id,int mapId)
        {
            Id = id;
            MapId=mapId;

            Floor = reader.ReadByte();

            if (Floor * 10 == -1280)
                return;

            LosMov = reader.ReadByte();
            Speed = reader.ReadByte();
            MapChangeData = reader.ReadByte();
        }


        [ProtoMember(1)]
        public ushort Id
        {
            get;
            set;
        }

        [ProtoMember(2)]
        public short Floor
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public byte LosMov
        {
            get;
            set;
        }

        [ProtoMember(4)]
        public byte Speed
        {
            get;
            set;
        }

        [ProtoMember(5)]
        public byte MapChangeData
        {
            get;
            set;
        }

        [ProtoMember(6)]
        public int MapId
        {
            get;
            set;
        }

        public bool Los
        {
            get { return (LosMov & 2) >> 1 == 1; }
        }

        public bool Mov
        {
            get { return (LosMov & 1) == 1 && !NonWalkableDuringFight && !FarmCell; }
        }

        public bool NonWalkableDuringFight
        {
            get { return (LosMov & 4) >> 2 == 1; }
        }

        public bool FarmCell
        {
            get { return (LosMov & 32) >> 5 == 1; }
        }

        public bool Visible
        {
            get { return (LosMov & 64) >> 6 == 1; }
        }

    }
}