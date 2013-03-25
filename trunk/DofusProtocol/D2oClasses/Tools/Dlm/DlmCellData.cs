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

using Stump.Core.IO;

namespace Stump.DofusProtocol.D2oClasses.Tools.Dlm
{
    public struct DlmCellData
    {
        private short? m_floor;
        private short m_id;

        private byte m_losMov;

        private byte m_mapChangeData;
        private byte m_moveZone;
        private sbyte m_rawFloor;
        private byte m_speed;

        public DlmCellData(short id)
        {
            m_id = id;
            m_losMov = 3;
            m_rawFloor = 0;
            m_floor = 0;
            m_speed = 0;
            m_mapChangeData = 0;
            m_moveZone = 0;
        }

        public short Floor
        {
            get { return m_floor ?? (m_floor = (short) (m_rawFloor*10)).Value; }
        }

        public bool Los
        {
            get { return (m_losMov & 2) >> 1 == 1; }
        }

        public bool Mov
        {
            get { return (m_losMov & 1) == 1 && !NonWalkableDuringFight && !FarmCell; }
        }

        public bool NonWalkableDuringFight
        {
            get { return (m_losMov & 4) >> 2 == 1; }
        }

        public bool Red
        {
            get { return (m_losMov & 8) >> 3 == 1; }
        }

        public bool Blue
        {
            get { return (m_losMov & 16) >> 4 == 1; }
        }

        public bool FarmCell
        {
            get { return (m_losMov & 32) >> 5 == 1; }
        }

        public bool Visible
        {
            get { return (m_losMov & 64) >> 6 == 1; }
        }

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

        public byte LosMov
        {
            get { return m_losMov; }
            set { m_losMov = value; }
        }


        public static DlmCellData ReadFromStream(short id, byte version, IDataReader reader)
        {
            var cell = new DlmCellData(id);

            cell.m_rawFloor = reader.ReadSByte();

            if (cell.m_rawFloor == -128)
            {
                return cell;
            }


            cell.m_losMov = reader.ReadByte();
            cell.m_speed = reader.ReadByte();
            cell.m_mapChangeData = reader.ReadByte();

            if (version > 5)
            {
                cell.m_moveZone = reader.ReadByte();
            }

            return cell;
        }
    }
}