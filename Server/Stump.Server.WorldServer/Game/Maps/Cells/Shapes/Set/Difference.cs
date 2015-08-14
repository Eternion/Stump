using System.Collections.Generic;

namespace Stump.Server.WorldServer.Game.Maps.Cells.Shapes.Set
{
    public class Difference : Set
    {
        public Set A
        {
            get;
            set;
        }

        public Set B
        {
            get;
            set;
        }

        public Difference(Set A, Set B)
        {
            this.A = A;
            this.B = B;
        }

        public override IEnumerable<MapPoint> EnumerateSet()
        {
            var data = new HashSet<MapPoint>(A.EnumerateSet());
            data.SymmetricExceptWith(B.EnumerateSet());
            return data;
        }

        public override bool BelongToSet(MapPoint point)
        {
            var belongToA = A.BelongToSet(point);
            var belongToB = B.BelongToSet(point);

            return (belongToA || belongToB) && !(belongToA && belongToB);
        }
    }
}