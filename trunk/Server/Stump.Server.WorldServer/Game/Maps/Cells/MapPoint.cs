using System;
using System.Collections.Generic;
using System.Drawing;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Game.Maps.Cells
{
    /// <summary>
    /// Represents a point on a 2 dimensional plan from a map cell
    /// </summary>
    public class MapPoint
    {
        public const uint MapWidth = 14;
        public const uint MapHeight = 20;

        public const uint MapSize = MapWidth * MapHeight * 2;

        private static readonly Point VectorRight = new Point(1, 1);
        private static readonly Point VectorDownRight = new Point(1, 0);
        private static readonly Point VectorDown = new Point(1, -1);
        private static readonly Point VectorDownLeft = new Point(0, -1);
        private static readonly Point VectorLeft = new Point(-1, -1);
        private static readonly Point VectorUpLeft = new Point(-1, 0);
        private static readonly Point VectorUp = new Point(-1, 1);
        private static readonly Point VectorUpRight = new Point(0, 1);

        private static bool m_initialized;
        private static readonly Point[] OrthogonalGridReference = new Point[MapSize];


        private short m_cellId;
        private int m_x;
        private int m_y;

        public MapPoint(short cellId)
        {
            m_cellId = cellId;

            SetFromCellId();
        }

        public MapPoint(Cell cell)
        {
            m_cellId = cell.Id;

            SetFromCellId();
        }

        public MapPoint(int x, int y)
        {
            m_x = x;
            m_y = y;

            SetFromCoords();
        }

        public MapPoint(Point point)
        {
            m_x = point.X;
            m_y = point.Y;

            SetFromCoords();
        }

        public short CellId
        {
            get { return m_cellId; }
            set
            {
                m_cellId = value;
                SetFromCellId();
            }
        }

        public int X
        {
            get { return m_x; }
            set
            {
                m_x = value;
                SetFromCoords();
            }
        }

        public int Y
        {
            get { return m_y; }
            set
            {
                m_y = value;
                SetFromCoords();
            }
        }

        private void SetFromCoords()
        {
            if (!m_initialized)
                InitializeStaticGrid();

            m_cellId = (short) ((m_x - m_y)*MapWidth + m_y + (m_x - m_y)/2);
        }

        private void SetFromCellId()
        {
            if (!m_initialized)
                InitializeStaticGrid();

            if (m_cellId < 0 || m_cellId > MapSize)
                throw new IndexOutOfRangeException("Cell identifier out of bounds (" + m_cellId + ").");

            Point point = OrthogonalGridReference[m_cellId];
            m_x = point.X;
            m_y = point.Y;
        }


        public uint DistanceTo(MapPoint point)
        {
            return (uint)Math.Sqrt(( point.X - m_x ) * ( point.X - m_x ) + ( point.Y - m_y ) * ( point.Y - m_y ));
        }

        public uint DistanceToCell(MapPoint point)
        {
            return (uint) (Math.Abs(m_x - point.X) + Math.Abs(m_y - point.Y));
        }

        public bool IsAdjacentTo(MapPoint point)
        {
            return DistanceToCell(point) == 1;
        }

        public DirectionsEnum OrientationToAdjacent(MapPoint point)
        {
            var vector = new Point
                             {
                                 X = point.X > m_x ? (1) : (point.X < m_x ? (-1) : (0)),
                                 Y = point.Y > m_y ? (1) : (point.Y < m_y ? (-1) : (0))
                             };

            if (vector == VectorRight)
            {
                return DirectionsEnum.DIRECTION_EAST;
            }
            if (vector == VectorDownRight)
            {
                return DirectionsEnum.DIRECTION_SOUTH_EAST;
            }
            if (vector == VectorDown)
            {
                return DirectionsEnum.DIRECTION_SOUTH;
            }
            if (vector == VectorDownLeft)
            {
                return DirectionsEnum.DIRECTION_SOUTH_WEST;
            }
            if (vector == VectorLeft)
            {
                return DirectionsEnum.DIRECTION_WEST;
            }
            if (vector == VectorUpLeft)
            {
                return DirectionsEnum.DIRECTION_NORTH_WEST;
            }
            if (vector == VectorUp)
            {
                return DirectionsEnum.DIRECTION_NORTH;
            }
            if (vector == VectorUpRight)
            {
                return DirectionsEnum.DIRECTION_NORTH_EAST;
            }

            return DirectionsEnum.DIRECTION_EAST;
        }

        public DirectionsEnum OrientationTo(MapPoint point, Boolean diagonal = true)
        {
            int dx = point.X - m_x;
            int dy = m_y - point.Y;

            double distance = Math.Sqrt(dx*dx + dy*dy);
            double angleInRadians = Math.Acos(dx / distance);

            double angleInDegrees = angleInRadians * 180 / Math.PI;
            double transformedAngle = angleInDegrees * (point.Y > m_y ? ( -1 ) : ( 1 ));

            double orientation = !diagonal ? Math.Round(transformedAngle / 90) * 2 + 1 : Math.Round(transformedAngle / 45) + 1;

            if (orientation < 0)
            {
                orientation = orientation + 8;
            }

            return (DirectionsEnum) (uint) orientation;
        }

        public MapPoint[] GetCellsOnLineBetween(MapPoint destination)
        {
            var result = new List<MapPoint>();
            var direction = OrientationTo(destination);
            var current = this;
            for (int i = 0; i < MapHeight * MapWidth / 2; i++)
            {
                current = current.GetCellInDirection(direction, 1);

                if (current == null)
                    break;

                if (current.CellId == destination.CellId)
                    break;

                result.Add(current);
            }

            return result.ToArray();
        }

        public MapPoint GetCellInDirection(DirectionsEnum direction, short step)
        {
            MapPoint mapPoint = null;
            switch (direction)
            {
                case DirectionsEnum.DIRECTION_EAST:
                    {
                        mapPoint = new MapPoint(m_x + step, m_y + step);
                        break;
                    }
                case DirectionsEnum.DIRECTION_SOUTH_EAST:
                    {
                        mapPoint = new MapPoint(m_x + step, m_y);
                        break;
                    }
                case DirectionsEnum.DIRECTION_SOUTH:
                    {
                        mapPoint = new MapPoint(m_x + step, m_y - step);
                        break;
                    }
                case DirectionsEnum.DIRECTION_SOUTH_WEST:
                    {
                        mapPoint = new MapPoint(m_x, m_y - step);
                        break;
                    }
                case DirectionsEnum.DIRECTION_WEST:
                    {
                        mapPoint = new MapPoint(m_x - step, m_y - step);
                        break;
                    }
                case DirectionsEnum.DIRECTION_NORTH_WEST:
                    {
                        mapPoint = new MapPoint(m_x - step, m_y);
                        break;
                    }
                case DirectionsEnum.DIRECTION_NORTH:
                    {
                        mapPoint = new MapPoint(m_x - step, m_y + step);
                        break;
                    }
                case DirectionsEnum.DIRECTION_NORTH_EAST:
                    {
                        mapPoint = new MapPoint(m_x, m_y + step);
                        break;
                    }
            }

            if (mapPoint != null)
                if (IsInMap(mapPoint.X, mapPoint.Y))
                    return mapPoint;
                else
                    return null;

            return null;
        }

        public MapPoint GetNearestCellInDirection(DirectionsEnum direction)
        {
            return GetCellInDirection(direction, 1);
        }

        public IEnumerable<MapPoint> GetAdjacentCells(Func<short, bool> predicate)
        {
            var southEast = new MapPoint(m_x + 1, m_y);
            if (IsInMap(southEast.X, southEast.Y) && predicate(southEast.CellId))
                yield return southEast;

            var southWest = new MapPoint(m_x, m_y - 1);
            if (IsInMap(southWest.X, southWest.Y) && predicate(southWest.CellId))
                yield return southWest;

            var northEast = new MapPoint(m_x, m_y + 1);
            if (IsInMap(northEast.X, northEast.Y) && predicate(northEast.CellId))
                yield return northEast;

            var northWest = new MapPoint(m_x - 1, m_y);
            if (IsInMap(northWest.X, northWest.Y) && predicate(northWest.CellId))
                yield return northWest;
        }

        public static bool IsInMap(int x, int y)
        {
            return x + y >= 0 && x - y >= 0 && x - y < MapHeight*2 && x + y < MapWidth*2;
        }

        public static uint CoordToCellId(int x, int y)
        {
            if (!m_initialized)
                InitializeStaticGrid();

            return (uint) ((x - y)*MapWidth + y + (x - y)/2);
        }

        public static Point CellIdToCoord(uint param1)
        {
            if (!m_initialized)
                InitializeStaticGrid();

            return OrthogonalGridReference[param1];
        }

        /// <summary>
        /// Initialize a static 2D plan that is used as reference to convert a cell to a (X,Y) point
        /// </summary>
        private static void InitializeStaticGrid()
        {
            int posX = 0;
            int posY = 0;
            int cellCount = 0;

            for (int x = 0; x < MapHeight; x++)
            {
                for (int y = 0; y < MapWidth; y++)
                    OrthogonalGridReference[cellCount++] = new Point(posX + y, posY + y);

                posX++;

                for (int y = 0; y < MapWidth; y++)
                    OrthogonalGridReference[cellCount++] = new Point(posX + y, posY + y);

                posY--;
            }

            m_initialized = true;
        }

        public override string ToString()
        {
            return "[MapPoint(x:" + m_x + ", y:" + m_y + ", id:" + m_cellId + ")]";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(MapPoint)) return false;
            return Equals((MapPoint)obj);
        }

        public bool Equals(MapPoint other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.m_cellId == m_cellId && other.m_x == m_x && other.m_y == m_y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = m_cellId;
                result = (result*397) ^ m_x;
                result = (result*397) ^ m_y;
                return result;
            }
        }
    }
}