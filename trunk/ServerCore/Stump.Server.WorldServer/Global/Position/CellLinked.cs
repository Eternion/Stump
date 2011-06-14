
using System;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Messages.Framework.IO;
using Stump.Database.Data.World;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Global.Maps
{
    public static class CellExtensions
    {
        public static void WriteCell(this BigEndianWriter writer, CellLinked cell)
        {
            writer.WriteByte((byte) (cell.Floor/10));

            if (cell.Floor == -1280)
                return;

            writer.WriteByte(cell.LosMov);
            writer.WriteByte(cell.Speed);
            writer.WriteByte(cell.MapChangeData);
        }

        public static CellLinked ReadCell(this BigEndianReader reader)
        {
            var cell = new CellLinked {Floor = (short) (reader.ReadByte()*10)};

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

    public class CellLinked
    {
        private Cell m_cell;

        public CellLinked()
        {
        }

        public CellLinked(Map map, ref Cell cell)
        {
            ParrentMap = map;

            m_cell = cell;
            Point = new MapPoint(this);
        }

        public Map ParrentMap
        {
            get;
            set;
        }

        public MapPoint Point
        {
            get;
            private set;
        }

        public ushort Id
        {
            get { return m_cell.Id; }
        }

        public short Floor
        {
            get { return m_cell.Floor; }
            set { m_cell.Floor = value; }
        }

        public byte LosMov
        {
            get { return m_cell.LosMov; }
            set { m_cell.LosMov = value; }
        }

        public byte MapChangeData
        {
            get { return m_cell.MapChangeData; }
            set { m_cell.MapChangeData = value; }
        }

        public byte Speed
        {
            get { return m_cell.Speed; }
            set { m_cell.Speed = value; }
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

        /// <summary>
        /// Occurs when the cell has been reached.
        /// </summary>
        public event Action<CellLinked, Character> CellReached;

        public void NotifyCellReached(Character character)
        {
            Action<CellLinked, Character> handler = CellReached;

            if (handler != null)
                handler(this, character);
        }
    }
}