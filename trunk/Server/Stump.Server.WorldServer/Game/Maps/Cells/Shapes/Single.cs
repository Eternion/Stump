using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Game.Maps.Cells.Shapes
{
    public class Single : IShape
    {
        public uint Surface
        {
            get
            {
                return 1;
            }
        }

        public uint MinRadius
        {
            get
            {
                return 1;
            }
            set { }
        }

        public DirectionsEnum Direction
        {
            get
            {
                return DirectionsEnum.UP; 
            }
            set { }
        }

        public uint Radius
        {
            get { return 1; }
            set {  }
        }

        public Cell[] GetCells(Cell centerCell, Map map)
        {
            return new [] {centerCell};
        }
    }
}