using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Maps.Cells.Shapes.Set
{
    public class Intersection : Set
    {
        public Intersection(Set A, Set B)
        {
            this.A = A;
            this.B = B;
        }
        public Set A
        {
            get;
            private set;
        }

        public Set B
        {
            get;
            private set;
        }

        public override IEnumerable<MapPoint> EnumerateSet()
        {
            return A.EnumerateSet().Intersect(B.EnumerateSet());
        }

        public override bool BelongToSet(MapPoint point)
        {
            return A.BelongToSet(point) && B.BelongToSet(point);
        }
    }
}