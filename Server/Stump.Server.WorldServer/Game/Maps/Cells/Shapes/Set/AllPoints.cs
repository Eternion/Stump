using System.Collections.Generic;

namespace Stump.Server.WorldServer.Game.Maps.Cells.Shapes.Set
{
    public class AllPoints : Set
    {
        public override IEnumerable<MapPoint> EnumerateSet()
        {
            return MapPoint.GetAllPoints();
        }

        public override bool BelongToSet(MapPoint point)
        {
            return point.IsInMap();
        }
    }
}