using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Worlds.Maps.Cells
{
    /// <summary>
    /// Represents the position of an object relative to the global world
    /// </summary>
    public class ObjectPosition
    {
        public event Action<ObjectPosition> PositionChanged;

        private void NotifyPositionChanged()
        {
            Action<ObjectPosition> handler = PositionChanged;
            if (handler != null)
                handler(this);
        }
        
        public ObjectPosition(Map map, Cell cell, DirectionsEnum direction)
        {
            m_map = map;
            m_cell = cell;
            m_direction = direction;
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
    }
}