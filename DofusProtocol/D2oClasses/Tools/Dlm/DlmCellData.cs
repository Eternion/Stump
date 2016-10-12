#region License GNU GPL

// DlmCellData.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

#endregion

using System;
using Stump.Core.IO;
using Stump.Core.Mathematics;

namespace Stump.DofusProtocol.D2oClasses.Tools.Dlm
{
    public struct DlmCellData
    {
        private short? m_floor;
        private short m_id;

        private short m_data;

        private byte m_mapChangeData;
        private byte m_moveZone;
        private sbyte m_rawFloor;
        private byte m_speed;

        public DlmCellData(short id)
        {
            m_id = id;
            m_data = 3;
            m_rawFloor = 0;
            m_floor = 0;
            m_speed = 0;
            m_mapChangeData = 0;
            m_moveZone = 0;
        }

        public short Floor => m_floor ?? (m_floor = (short) (m_rawFloor*10)).Value;

        public bool Mov => (m_data & 1) != 0 && !NonWalkableDuringFight && !FarmCell;

        public bool NonWalkableDuringFight => (m_data & 2) != 0;

        public bool NonWalkableDuringRP => (m_data & 4) != 0;

        public bool Los => (m_data & 8) != 0;

        public bool Blue => (m_data & 16) != 0;

        public bool Red => (m_data & 32) != 0;

        public bool FarmCell => (m_data & 64) != 0;

        public bool Visible => (m_data & 128) != 0;

        public bool HavenbagCell => (m_data & 256) != 0;


        public short Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        public byte MapChangeData
        {
            get { return m_mapChangeData; }
            set { m_mapChangeData = value; }
        }

        public byte MoveZone
        {
            get { return m_moveZone; }
            set { m_moveZone = value; }
        }

        public byte Speed
        {
            get { return m_speed; }
            set { m_speed = value; }
        }

        public short Data
        {
            get { return m_data; }
            set { m_data = value; }
        }
        

        public bool UseTopArrow
        {
            get { return (m_data & 512) != 0; }
        }

        public bool UseBottomArrow
        {
            get { return (m_data & 1024) != 0; }
        }   
     
        public bool UseRightArrow
        {
            get { return (m_data & 2048) != 0; }
        }     
   
        public bool UseLeftArrow
        {
            get { return (m_data & 4096) != 0; }
        }
        public static DlmCellData ReadFromStream(short id, byte version, IDataReader reader)
        {
            var cell = new DlmCellData(id);

            cell.m_rawFloor = reader.ReadSByte();

            if (cell.m_rawFloor == -128)
            {
                return cell;
            }

            if (version >= 9)
            {
                var data = reader.ReadShort();
                // invert first bit
                data = data.FlipBit(0);
                data = data.FlipBit(3);

                if (version < 10)
                {
                    // havenbag bit (8th) not used
                    data = data.ShiftBitsLeft(8, 1);
                }
                
                cell.m_data = data;
            }
            else
            {
                var data = reader.ReadByte();
                data = data.ShiftBitsLeft(1, 1);
                data = data.SwapBits(7, 1);
                data = data.SwapBits(2, 3);
                data = data.SwapBits(4, 5);

                cell.m_data = data;
            }

            cell.m_speed = reader.ReadByte();
            cell.m_mapChangeData = reader.ReadByte();

            if (version > 5)
            {
                cell.m_moveZone = reader.ReadByte();
            }

            if (version > 7 && version < 9)
            {
                cell.m_data |= (byte)(0xF & reader.ReadByte() << 9);
            }

            return cell;
        }
    }
}