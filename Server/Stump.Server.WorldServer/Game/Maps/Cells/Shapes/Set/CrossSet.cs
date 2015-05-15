using System.Collections.Generic;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Game.Maps.Cells.Shapes.Set
{
    public class CrossSet : Set
    {

        public CrossSet(MapPoint center, int maxRange)
            : this(center, 0, maxRange)
        {
        }

        public CrossSet(MapPoint center, int minRange, int maxRange)
        {
            Center = center;
            MinRange = minRange;
            MaxRange = maxRange;
        }

        public MapPoint Center
        {
            get;
            set;
        }

        public int MinRange
        {
            get;
            set;
        }

        public int MaxRange
        {
            get;
            set;
        }

        public override IEnumerable<MapPoint> EnumerateSet()
        {
            for (int i = MinRange; i <= MaxRange; i++)
            {
                MapPoint point;
                if ((point = Center.GetCellInDirection(DirectionsEnum.DIRECTION_NORTH_EAST, i)) != null)
                    yield return point;
                if ((point = Center.GetCellInDirection(DirectionsEnum.DIRECTION_SOUTH_EAST, i)) != null)
                    yield return point;                
                if ((point = Center.GetCellInDirection(DirectionsEnum.DIRECTION_SOUTH_WEST, i)) != null)
                    yield return point;
                if ((point = Center.GetCellInDirection(DirectionsEnum.DIRECTION_NORTH_WEST, i)) != null)
                    yield return point;
            }
        }

        public override bool BelongToSet(MapPoint point)
        {
            var dist = point.ManhattanDistanceTo(Center);
            return point.IsOnSameLine(Center) && dist >= MinRange && dist <= MaxRange;
        }
    }
}