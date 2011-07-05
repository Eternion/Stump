using System;
using System.Drawing;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.World.Maps.Cells
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


        private ushort m_cellId;
        private int m_x;
        private int m_y;

        public MapPoint(ushort cellId)
        {
            m_cellId = cellId;

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

        public ushort CellId
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

            m_cellId = (ushort) ((m_x - m_y)*MapWidth + m_y + (m_x - m_y)/2);
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

        public int DistanceToCell(MapPoint point)
        {
            return Math.Abs(m_x - point.X) + Math.Abs(m_y - point.Y);
        }

        public DirectionsEnum OrientationTo(MapPoint point)
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

        public uint AdvancedOrientationTo(MapPoint point, Boolean param2 = true)
        {
            int x = point.X - m_x;
            int y = m_y - point.Y;
            double orientation = Math.Acos(x/Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)))*180/Math.PI*
                            (point.Y > m_y ? (-1) : (1));
            if (param2)
            {
                orientation = Math.Round(orientation/90)*2 + 1;
            }
            else
            {
                orientation = Math.Round(orientation/45) + 1;
            }
            if (orientation < 0)
            {
                orientation = orientation + 8;
            }
            return (uint) orientation;
        }

        public MapPoint GetNearestCellInDirection(DirectionsEnum direction)
        {
            MapPoint mapPoint = null;
            switch (direction)
            {
                case DirectionsEnum.DIRECTION_EAST:
                    {
                        mapPoint = new MapPoint(m_x + 1, m_y + 1);
                        break;
                    }
                case DirectionsEnum.DIRECTION_SOUTH_EAST:
                    {
                        mapPoint = new MapPoint(m_x + 1, m_y);
                        break;
                    }
                case DirectionsEnum.DIRECTION_SOUTH:
                    {
                        mapPoint = new MapPoint(m_x + 1, m_y - 1);
                        break;
                    }
                case DirectionsEnum.DIRECTION_SOUTH_WEST:
                    {
                        mapPoint = new MapPoint(m_x, m_y - 1);
                        break;
                    }
                case DirectionsEnum.DIRECTION_WEST:
                    {
                        mapPoint = new MapPoint(m_x - 1, m_y - 1);
                        break;
                    }
                case DirectionsEnum.DIRECTION_NORTH_WEST:
                    {
                        mapPoint = new MapPoint(m_x - 1, m_y);
                        break;
                    }
                case DirectionsEnum.DIRECTION_NORTH:
                    {
                        mapPoint = new MapPoint(m_x - 1, m_y + 1);
                        break;
                    }
                case DirectionsEnum.DIRECTION_NORTH_EAST:
                    {
                        mapPoint = new MapPoint(m_x, m_y + 1);
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