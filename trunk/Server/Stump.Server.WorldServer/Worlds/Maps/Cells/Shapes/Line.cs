using System.Collections.Generic;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Worlds.Maps.Cells.Shapes
{
    public class Line : IShape
    {
        public Line(uint radius)
        {
            Radius = radius;
            Direction = DirectionsEnum.DOWN_RIGHT;
        }

        #region IShape Members

        public uint Surface
        {
            get
            {
                return Radius + 1;
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

            for (int i = (int) MinRadius; i <= Radius; i++)
            {
                switch (Direction)
                {
                    case DirectionsEnum.LEFT:
                        if (MapPoint.IsInMap(centerPoint.X - i, centerPoint.Y - i))
                            result.Add(GetCell(centerPoint.X - i, centerPoint.Y - i, map));
                        break;
                    case DirectionsEnum.UP:
                        if (MapPoint.IsInMap(centerPoint.X - i, centerPoint.Y + i))
                            result.Add(GetCell(centerPoint.X - i, centerPoint.Y - i, map));
                        break;
                    case DirectionsEnum.RIGHT:
                        if (MapPoint.IsInMap(centerPoint.X + i, centerPoint.Y + i))
                            result.Add(GetCell(centerPoint.X - i, centerPoint.Y - i, map));
                        break;
                    case DirectionsEnum.DOWN:
                        if (MapPoint.IsInMap(centerPoint.X + i, centerPoint.Y - i))
                            result.Add(GetCell(centerPoint.X - i, centerPoint.Y - i, map));
                        break;
                    case DirectionsEnum.UP_LEFT:
                        if (MapPoint.IsInMap(centerPoint.X - i, centerPoint.Y))
                            result.Add(GetCell(centerPoint.X - i, centerPoint.Y - i, map));
                        break;
                    case DirectionsEnum.DOWN_LEFT:
                        if (MapPoint.IsInMap(centerPoint.X, centerPoint.Y - i))
                            result.Add(GetCell(centerPoint.X - i, centerPoint.Y - i, map));
                        break;
                    case DirectionsEnum.DOWN_RIGHT:
                        if (MapPoint.IsInMap(centerPoint.X + i, centerPoint.Y))
                            result.Add(GetCell(centerPoint.X - i, centerPoint.Y - i, map));
                        break;
                    case DirectionsEnum.UP_RIGHT:
                        if (MapPoint.IsInMap(centerPoint.X, centerPoint.Y + i))
                            result.Add(GetCell(centerPoint.X - i, centerPoint.Y - i, map));
                        break;
                }
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