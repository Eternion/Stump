using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Maps.Cells.Shapes.Set
{
    public class SingleSet : Set
    {
        public MapPoint Element
        {
            get;
            private set;
        }

        public SingleSet(MapPoint element)
        {
            Element = element;
        }

        public override IEnumerable<MapPoint> EnumerateSet()
        {
            return Enumerable.Repeat(Element, 1);
        }

        public override bool BelongToSet(MapPoint point)
        {
            return Equals(point, Element);
        }
    }
}