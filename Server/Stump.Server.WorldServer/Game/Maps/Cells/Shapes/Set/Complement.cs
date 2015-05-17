using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Maps.Cells.Shapes.Set
{
    public class Complement : Set
    {
        public Set A
        {
            get;
            set;
        }

        public Set Container
        {
            get;
            set;
        }

        public Complement(Set A, Set Container)
        {
            this.A = A;
            this.Container = Container;
        }

        public override IEnumerable<MapPoint> EnumerateSet()
        {
            return Container.EnumerateSet().Except(A.EnumerateSet());
        }

        public override bool BelongToSet(MapPoint point)
        {
            return !A.BelongToSet(point) && Container.BelongToSet(point);
        }
    }
}