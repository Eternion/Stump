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
using Stump.BaseCore.Framework.IO;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Global.Maps
{
    public static class CellExtensions
    {
        public static void WriteCell(this BigEndianWriter writer, CellData cell)
        {
            writer.WriteByte((byte) (cell.Floor/10));

            if (cell.Floor == -1280)
                return;

            writer.WriteByte(cell.LosMov);
            writer.WriteByte(cell.Speed);
            writer.WriteByte(cell.MapChangeData);
        }

        public static CellData ReadCell(this BigEndianReader reader)
        {
            var cell = new CellData {Floor = (short) (reader.ReadByte()*10)};

            if (cell.Floor == -1280)
            {
                return cell;
            }

            cell.LosMov = reader.ReadByte();
            cell.Speed = reader.ReadByte();
            cell.MapChangeData = reader.ReadByte();

            return cell;
        }
    }

    public class CellData
    {
        /// <summary>
        /// Occurs when the cell has been reached.
        /// </summary>
        public event Action<CellData, Character> CellReached;

        public void NotifyCellReached(Character character)
        {
            Action<CellData, Character> handler = CellReached;

            if (handler != null)
                handler(this, character);
        }

        public short Floor;
        public byte LosMov;
        public byte MapChangeData;
        public byte Speed;

        public Map ParrentMap
        {
            get;
            set;
        }

        public ushort Id
        {
            get;
            set;
        }

        public Boolean Los
        {
            get { return (LosMov & 2) >> 1 == 1; }
        }

        public Boolean Mov
        {
            get { return (LosMov & 1) == 1 && !NonWalkableDuringFight && !FarmCell; }
        }

        public Boolean NonWalkableDuringFight
        {
            get { return (LosMov & 4) >> 2 == 1; }
        }

        public Boolean Red
        {
            get { return (LosMov & 8) >> 3 == 1; }
        }

        public Boolean Blue
        {
            get { return (LosMov & 16) >> 4 == 1; }
        }

        public Boolean FarmCell
        {
            get { return (LosMov & 32) >> 5 == 1; }
        }

        public Boolean Visible
        {
            get { return (LosMov & 64) >> 6 == 1; }
        }
    }
}