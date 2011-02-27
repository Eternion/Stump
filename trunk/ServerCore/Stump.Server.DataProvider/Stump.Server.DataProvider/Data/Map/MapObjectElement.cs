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