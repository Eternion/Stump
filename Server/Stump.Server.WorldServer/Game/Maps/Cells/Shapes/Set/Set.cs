using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Maps.Cells.Shapes.Set
{
    public abstract class Set
    {
        public abstract IEnumerable<MapPoint> EnumerateSet();
        public abstract bool BelongToSet(MapPoint point);

        public IEnumerable<MapPoint> EnumerateValidPoints()
        {
            return EnumerateSet().Where(x => x.IsInMap());
        }
    }
}