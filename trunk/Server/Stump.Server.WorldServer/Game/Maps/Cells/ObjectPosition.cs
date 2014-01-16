using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Game.Maps.Cells
{
    /// <summary>
    /// Represents the position of an object relative to the global world
    /// </summary>
    public class ObjectPosition
    {
        public event Action<ObjectPosition> PositionChanged;

        private void NotifyPositionChanged()
        {
            var handler = PositionChanged;
            if (handler != null)
                handler(this);
        }

        public ObjectPosition(ObjectPosition position)
        {
            m_map = position.Map;
            m_cell = position.Cell;
            m_direction = position.Direction;
        }

        public ObjectPosition(Map map, Cell cell)
        {
            m_map = map;
            m_cell = cell;
            m_direction = DirectionsEnum.DIRECTION_EAST;
        }

        public ObjectPosition(Map map, short cellId)
        {
            m_map = map;
            m_cell = map.Cells[cellId];
            m_direction = DirectionsEnum.DIRECTION_EAST;
        }

        public ObjectPosition(Map map, Cell cell, DirectionsEnum direction)
        {
            m_map = map;
            m_cell = cell;
            m_direction = direction;
        }

        public ObjectPosition(Map map, short cellId, DirectionsEnum direction)
        {
            m_map = map;
            m_cell = map.Cells[cellId];
            m_direction = direction;
        }

        public bool IsValid
        {
            get
            {
                return (m_cell.Id > 0 && m_cell.Id < MapPoint.MapSize) &&
                    (m_direction > DirectionsEnum.DIRECTION_EAST && m_direction < DirectionsEnum.DIRECTION_NORTH_EAST) &&
                    m_map != null;
            }
        }

        private DirectionsEnum m_direction;

        public DirectionsEnum Direction
        {
            get { return m_direction; }
            set
            {
                m_direction = value;

                NotifyPositionChanged();
            }
        }

        private Cell m_cell;

        public Cell Cell
        {
            get { return m_cell; }
            set
            {
                m_cell = value;
                m_point = null;

                NotifyPositionChanged();
            }
        }

        private Map m_map;

        public Map Map
        {
            get { return m_map; }
            set
            {
                m_map = value;

                NotifyPositionChanged();
            }
        }

        private MapPoint m_point;

        public MapPoint Point
        {
            get { return m_point ?? (m_point = MapPoint.GetPoint(Cell)); }
        }

        public ObjectPosition Clone()
        {
            return new ObjectPosition(Map, Cell, Direction);
        }
    }
}