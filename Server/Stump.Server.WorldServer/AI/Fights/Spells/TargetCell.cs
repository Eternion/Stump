using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.AI.Fights.Spells
{
    public class TargetCell
    {
        protected bool Equals(TargetCell other)
        {
            return Equals(Cell, other.Cell) && Direction == other.Direction;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Cell != null ? Cell.GetHashCode() : 0)*397) ^ (int) Direction;
            }
        }

        private MapPoint m_point;

        public TargetCell(Cell cell, DirectionFlagEnum direction = DirectionFlagEnum.ALL_DIRECTIONS)
        {
            Cell = cell;
            Direction = direction;
            m_point = new MapPoint(cell);
        }

        public Cell Cell
        {
            get;
            private set;
        }

        public MapPoint Point
        {
            get { return m_point; }
        }

        public DirectionFlagEnum Direction
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TargetCell) obj);
        }
    }
}