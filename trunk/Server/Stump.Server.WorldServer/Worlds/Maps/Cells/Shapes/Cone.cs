using System;
using System.Collections.Generic;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Worlds.Maps.Cells.Shapes
{
    public class Cone : IShape
    {
        public Cone(uint minRadius, uint radius)
        {
            MinRadius = minRadius;
            Radius = radius;

            Direction = DirectionsEnum.DOWN_RIGHT;
        }

        #region IShape Members

        public uint Surface
        {
            get
            {
                return ( Radius + 1 ) * ( Radius + 1 );
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

            int i = 0;
            int j = 1;
            int y = 0;
            int x = 0;
            switch (Direction)
            {
                case DirectionsEnum.UP_LEFT:
                    x = centerPoint.X;
                    while (x >= centerPoint.X - Radius)
                    {
                        y = -i;
                        while (y <= i)
                        {
                            if (MinRadius == 0 || Math.Abs(centerPoint.X - x) + Math.Abs(y) >= MinRadius)
                                if (MapPoint.IsInMap(x, centerPoint.Y + y))
                                    result.Add(GetCell(x, y + centerPoint.Y, map));

                            y++;
                        }
                        i = i + j;
                        x--;
                    }
                    break;
                case DirectionsEnum.DOWN_LEFT:
                    y = centerPoint.Y;
                    while (y >= centerPoint.Y - Radius)
                    {
                        x = -i;
                        while (x <= i)
                        {
                            if (MinRadius == 0 || Math.Abs(x) + Math.Abs(centerPoint.Y - y) >= MinRadius)
                                if (MapPoint.IsInMap(x + centerPoint.X, y))
                                    result.Add(GetCell(x + centerPoint.X, y, map));

                            x++;
                        }
                        i = i + j;
                        y--;
                    }
                    break;
                case DirectionsEnum.DOWN_RIGHT:
                    x = centerPoint.X;
                    while (x <= centerPoint.X + Radius)
                    {
                        y = -i;
                        while (y <= i)
                        {
                            if (MinRadius == 0 || Math.Abs(centerPoint.X - x) + Math.Abs(y) >= MinRadius)
                                if (MapPoint.IsInMap(x, centerPoint.Y + y))
                                    result.Add(GetCell(x, y + centerPoint.Y, map));

                            y++;
                        }
                        i = i + j;
                        x++;
                    }
                    break;
                case DirectionsEnum.UP_RIGHT:
                    y = centerPoint.Y;
                    while (y <= centerPoint.Y - Radius)
                    {
                        x = -i;
                        while (x <= i)
                        {
                            if (MinRadius == 0 || Math.Abs(x) + Math.Abs(centerPoint.Y - y) >= MinRadius)
                                if (MapPoint.IsInMap(x + centerPoint.X, y))
                                    result.Add(GetCell(x + centerPoint.X, y, map));

                            x++;
                        }
                        i = i + j;
                        y++;
                    }
                    break;

            }

            return result.ToArray();
        }

        private static Cell GetCell(int x, int y, Map map)
        {
            return map.Cells[new MapPoint(x, y).CellId];
        }

        #endregion
    }
}