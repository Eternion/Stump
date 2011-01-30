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
using System.Globalization;
using System.Linq;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Classes.Custom;

namespace Stump.DofusProtocol.Classes.Extensions
{
    public static class MapExtension
    {

        public static void WriteMap(this BigEndianWriter writer, Map map)
        {
            writer.WriteByte(0x96);
            writer.WriteByte((byte)map.Version);
            writer.WriteInt(map.Id);
            writer.WriteInt((int)map.RelativeId);

            writer.WriteByte(map.MapType);
            writer.WriteInt(map.SubAreaId);

            writer.WriteInt(map.TopNeighbourId);
            writer.WriteInt(map.BottomNeighbourId);
            writer.WriteInt(map.LeftNeighbourId);
            writer.WriteInt(map.RightNeighbourId);

            writer.WriteInt(map.ShadowBonusOnEntities);

            writer.WriteBoolean(map.UseLowpassFilter);

            writer.WriteBoolean(map.UseReverb);

            if (map.UseReverb)
            {
                writer.WriteInt(map.PresetId);
            }

            foreach (CellData cell in map.CellsData)
            {
                writer.WriteCell(cell);
            }

            writer.WriteInt(map.MapElementsPositions.Count);
            foreach (var elementsPosition in map.MapElementsPositions)
            {
                writer.WriteUInt(elementsPosition.Key);
                writer.WriteUShort(elementsPosition.Value.Id);
            }
        }

        public static Map ReadMap(this BigEndianReader reader)
        {
            int header = reader.ReadByte();

            if (header != 0x96)
                throw new FileLoadException("Wrong header file");

            var map = new Map
            {
                Version = reader.ReadByte(),
                Id = reader.ReadInt(),
                RelativeId = (uint)reader.ReadInt(),
                MapType = (MapTypeEnum)reader.ReadByte(),
                SubAreaId = (uint)reader.ReadInt(),
                TopNeighbourId = reader.ReadInt(),
                BottomNeighbourId = reader.ReadInt(),
                LeftNeighbourId = reader.ReadInt(),
                RightNeighbourId = reader.ReadInt(),
                ShadowBonusOnEntities = reader.ReadInt(),
                UseLowpassFilter = reader.ReadByte() == 1,
                UseReverb = reader.ReadByte() == 1
            };

            map.PresetId = map.UseReverb ? reader.ReadInt() : -1;

            for (ushort i = 0; i < MaximumCellsCount; i++)
            {
                CellData celldata = reader.ReadCell();
                celldata.Id = i;
                celldata.MapId = map;

                map.CellsData.Add(celldata);
            }

            int count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                uint key = reader.ReadUInt();
                ushort cell = reader.ReadUShort();

                // objects can be superposed, so we ignore it
                if (map.Elements.ContainsKey(key) && map.MapElementsPositions[key].Id == cell)
                    continue;

                if (key > 0)
                    map.MapElementsPositions.Add(key, map.CellsData.Where(c => c.Id == cell));
            }

            return map;
        }

    }
}