using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Global
{
    public class VectorIso
    {
        public VectorIso(ushort cell, DirectionsEnum direction)
        {
            CellId = cell;
            Direction = direction;
        }

        public VectorIso(ushort cell, DirectionsEnum direction, Map map)
        {
            Direction = direction;
            CellId = cell;
            Map = map;
        }

        public DirectionsEnum Direction
        {
            get;
            set;
        }

        public Map Map
        {
            get;
            set;
        }

        private ushort m_cell;

        public ushort CellId
        {
            get { return m_cell; }
            set
            {
                if (value >= Map.MaximumCellsCount)
                    throw new ArgumentException("'value' must be lesser than " + Map.MaximumCellsCount);
                m_cell = value;
            }
        }

        public CellData Cell
        {
            get
            {
                return Map == null ? null : Map.CellsData[CellId];
            }
            set
            {
                if (Map != null && Map.CellsData.Contains(value))
                {
                    CellId = (ushort) Map.CellsData.FindIndex(entry => entry == value);
                }
            }
        }

        public void ChangeLocation(ushort cellId)
        {
            CellId = cellId;
        }

        public void ChangeLocation(DirectionsEnum direction)
        {
            Direction = direction;
        }

        public void ChangeLocation(VectorIso vectorIso)
        {
            CellId = vectorIso.CellId;
            Direction = vectorIso.Direction;
            Map = vectorIso.Map ?? Map;
        }
    }
}