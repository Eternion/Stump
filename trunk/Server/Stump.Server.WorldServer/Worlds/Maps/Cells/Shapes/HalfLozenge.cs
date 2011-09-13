using System.Collections.Generic;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Worlds.Maps.Cells.Shapes
{
    public class HalfLozenge : IShape
    {
        public HalfLozenge(uint minRadius, uint radius)
        {
            MinRadius = minRadius;
            Radius = radius;

            Direction = DirectionsEnum.UP;
        }

        public uint Surface
        {
            get { return Radius*2 + 1; }
        }

        public uint MinRadius
        {
            get;
            set;
        }

        public DirectionsEnum Direction
        {
            get;
            set;
        }

        public uint Radius
        {
            get;
            set;
        }

        public Cell[] GetCells(Cell centerCell, Map map)
        {
            var centerPoint = new MapPoint(centerCell);
            var result = new List<Cell>();

            if (MinRadius == 0)
                result.Add(centerCell);

            for (int i = 1; i <= Radius; i++)
            {
                switch (Direction)
                {
                    case DirectionsEnum.UP_LEFT:
                        result.Add(GetCell(centerPoint.X + i, centerPoint.Y + i, map));
                        result.Add(GetCell(centerPoint.X + i, centerPoint.Y - i, map));
                        break;

                    case DirectionsEnum.UP_RIGHT:
                        result.Add(GetCell(centerPoint.X - i, centerPoint.Y - i, map));
                        result.Add(GetCell(centerPoint.X + i, centerPoint.Y - i, map));
                        break;

                    case DirectionsEnum.DOWN_RIGHT:
                        result.Add(GetCell(centerPoint.X - i, centerPoint.Y + i, map));
                        result.Add(GetCell(centerPoint.X - i, centerPoint.Y - i, map));
                        break;

                    case DirectionsEnum.DOWN_LEFT:
                        result.Add(GetCell(centerPoint.X - i, centerPoint.Y + i, map));
                        result.Add(GetCell(centerPoint.X + i, centerPoint.Y + i, map));
                        break;
                }
            }

            return result.ToArray();
        }

        private static Cell GetCell(int x, int y, Map map)
        {
            return map.Cells[new MapPoint(x, y).CellId];
        }
    }
}