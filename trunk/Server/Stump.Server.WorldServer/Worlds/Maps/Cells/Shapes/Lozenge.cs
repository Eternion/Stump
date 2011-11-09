using System;
using System.Collections.Generic;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Worlds.Maps.Cells.Shapes
{
    public class Lozenge : IShape
    {
        public Lozenge(uint minRadius, uint radius)
        {
            MinRadius = minRadius;
            Radius = radius;
        }

        #region IShape Members

        public uint Surface
        {
            get
            {
                return (Radius + 1)*(Radius + 1) + Radius*Radius;
            }
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

            if (Radius == 0)
            {
                if (MinRadius == 0)
                    result.Add(centerCell);

                return result.ToArray();
            }

            int x = (int) (centerPoint.X - Radius);
            int y = 0;
            int i = 0;
            int j = 1;
            while (x <= centerPoint.X + Radius)
            {
                y = -i;

                while (y <= i)
                {
                    if (MinRadius == 0 || Math.Abs(centerPoint.X - x) + Math.Abs(y) >= MinRadius)
                        AddCellIfValid(x, y + centerPoint.Y, map, result);

                    y++;
                }

                if (i == Radius)
                {
                    j = -j;
                }

                i = i + j;
                x++;
            }

            return result.ToArray();
        }

        private static void AddCellIfValid(int x, int y, Map map, IList<Cell> container)
        {
            if (!MapPoint.IsInMap(x, y))
                return;

            container.Add(map.Cells[MapPoint.CoordToCellId(x, y)]);
        }

        #endregion
    }
}