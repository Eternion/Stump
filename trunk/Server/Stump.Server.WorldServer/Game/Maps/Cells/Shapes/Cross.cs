using System.Collections.Generic;
using System.Linq;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Game.Maps.Cells.Shapes
{
    public class Cross : IShape
    {
        public Cross(uint minRadius, uint radius)
        {
            MinRadius = minRadius;
            Radius = radius;

            DisabledDirections = new List<DirectionsEnum>();
        }

        public bool Diagonal
        {
            get;
            set;
        }

        public List<DirectionsEnum> DisabledDirections
        {
            get;
            set;
        }

        public bool OnlyPerpendicular
        {
            get;
            set;
        }

        public bool AllDirections
        {
            get;
            set;
        }

        #region IShape Members

        public uint Surface
        {
            get { return Radius*4 + 1; }
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
            var result = new List<Cell>();

            if (MinRadius == 0)
                result.Add(centerCell);

            List<DirectionsEnum> disabledDirections = DisabledDirections.ToList();
            if (OnlyPerpendicular)
            {
                switch (Direction)
                {
                    case DirectionsEnum.DOWN_RIGHT:
                    case DirectionsEnum.UP_LEFT:
                        {
                            disabledDirections.Add(DirectionsEnum.DOWN_RIGHT);
                            disabledDirections.Add(DirectionsEnum.UP_LEFT);
                            break;
                        }
                    case DirectionsEnum.UP_RIGHT:
                    case DirectionsEnum.DOWN_LEFT:
                        {
                            disabledDirections.Add(DirectionsEnum.UP_RIGHT);
                            disabledDirections.Add(DirectionsEnum.DOWN_LEFT);
                            break;
                        }
                    case DirectionsEnum.DOWN:
                    case DirectionsEnum.UP:
                        {
                            disabledDirections.Add(DirectionsEnum.DOWN);
                            disabledDirections.Add(DirectionsEnum.UP);
                            break;
                        }
                    case DirectionsEnum.RIGHT:
                    case DirectionsEnum.LEFT:
                        {
                            disabledDirections.Add(DirectionsEnum.RIGHT);
                            disabledDirections.Add(DirectionsEnum.LEFT);
                            break;
                        }
                }
            }

            var centerPoint = new MapPoint(centerCell);

            for (var i = (int) Radius; i > 0; i--)
            {
                if (i < MinRadius)
                    continue;

                if (!Diagonal)
                {
                    if (MapPoint.IsInMap(centerPoint.X + i, centerPoint.Y) && !disabledDirections.Contains(DirectionsEnum.DOWN_RIGHT))
                        AddCellIfValid(centerPoint.X + i, centerPoint.Y, map, result);
                    if (MapPoint.IsInMap(centerPoint.X - i, centerPoint.Y) && !disabledDirections.Contains(DirectionsEnum.UP_LEFT))
                        AddCellIfValid(centerPoint.X - i, centerPoint.Y, map, result);
                    if (MapPoint.IsInMap(centerPoint.X, centerPoint.Y + i) && !disabledDirections.Contains(DirectionsEnum.UP_RIGHT))
                        AddCellIfValid(centerPoint.X, centerPoint.Y + i, map, result);
                    if (MapPoint.IsInMap(centerPoint.X, centerPoint.Y - i) && !disabledDirections.Contains(DirectionsEnum.DOWN_LEFT))
                        AddCellIfValid(centerPoint.X, centerPoint.Y - i, map, result);
                }

                if (Diagonal || AllDirections)
                {
                    if (MapPoint.IsInMap(centerPoint.X + i, centerPoint.Y - i) && !disabledDirections.Contains(DirectionsEnum.DOWN))
                        AddCellIfValid(centerPoint.X + i, centerPoint.Y - i, map, result);
                    if (MapPoint.IsInMap(centerPoint.X - i, centerPoint.Y + i) && !disabledDirections.Contains(DirectionsEnum.UP))
                        AddCellIfValid(centerPoint.X - i, centerPoint.Y + i, map, result);
                    if (MapPoint.IsInMap(centerPoint.X + i, centerPoint.Y + i) && !disabledDirections.Contains(DirectionsEnum.RIGHT))
                        AddCellIfValid(centerPoint.X + i, centerPoint.Y + i, map, result);
                    if (MapPoint.IsInMap(centerPoint.X - i, centerPoint.Y - i) && !disabledDirections.Contains(DirectionsEnum.LEFT))
                        AddCellIfValid(centerPoint.X - i, centerPoint.Y - i, map, result);
                }
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