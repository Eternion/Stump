using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.DofusProtocol.Enums.Extensions;

namespace Stump.Server.WorldServer.Game.Maps.Cells.Shapes
{
    public class Line : IShape
    {
        public Line(byte radius, bool opposedDirection)
        {
            Radius = radius;
            Direction = DirectionsEnum.DIRECTION_SOUTH_EAST;
            OpposedDirection = opposedDirection;
        }

        #region IShape Members

        public uint Surface => (uint)Radius + 1;

        public byte MinRadius
        {
            get;
            set;
        }

        public DirectionsEnum Direction
        {
            get;
            set;
        }

        public byte Radius
        {
            get;
            set;
        }

        public bool OpposedDirection
        {
            get;
            set;
        }

        public Cell[] GetCells(Cell centerCell, Map map)
        {
            if (OpposedDirection)
                Direction = Direction.GetOpposedDirection();

            var centerPoint = new MapPoint(centerCell);
            var result = new List<Cell>();

            for (int i = MinRadius; i <= Radius; i++)
            {
                switch (Direction)
                {
                    case DirectionsEnum.DIRECTION_WEST:
                        AddCellIfValid(centerPoint.X - i, centerPoint.Y - i, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_NORTH:
                        AddCellIfValid(centerPoint.X - i, centerPoint.Y + i, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_EAST:
                        AddCellIfValid(centerPoint.X + i, centerPoint.Y + i, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH:
                        AddCellIfValid(centerPoint.X + i, centerPoint.Y - i, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_NORTH_WEST:
                        AddCellIfValid(centerPoint.X - i, centerPoint.Y, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH_WEST:
                        AddCellIfValid(centerPoint.X, centerPoint.Y - i, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_SOUTH_EAST:
                        AddCellIfValid(centerPoint.X + i, centerPoint.Y, map, result);
                        break;
                    case DirectionsEnum.DIRECTION_NORTH_EAST:
                        AddCellIfValid(centerPoint.X, centerPoint.Y + i, map, result);
                        break;
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