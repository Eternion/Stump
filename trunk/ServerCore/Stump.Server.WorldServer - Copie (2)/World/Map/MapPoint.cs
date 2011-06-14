// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Drawing;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Global.Maps
{
    public class MapPoint
    {
        public const uint MAP_WIDTH = 14;
        public const uint MAP_HEIGHT = 20;

        private static readonly Point VECTOR_RIGHT = new Point(1, 1);
        private static readonly Point VECTOR_DOWN_RIGHT = new Point(1, 0);
        private static readonly Point VECTOR_DOWN = new Point(1, -1);
        private static readonly Point VECTOR_DOWN_LEFT = new Point(0, -1);
        private static readonly Point VECTOR_LEFT = new Point(-1, -1);
        private static readonly Point VECTOR_UP_LEFT = new Point(-1, 0);
        private static readonly Point VECTOR_UP = new Point(-1, 1);
        private static readonly Point VECTOR_UP_RIGHT = new Point(0, 1);

        private static bool m_initialized;
        private static readonly Point[] OrthogonalGridReference = new Point[MAP_WIDTH*MAP_HEIGHT*2];


        private ushort m_cellId;
        private int m_x;
        private int m_y;

        public MapPoint(Map map, ushort cellId)
        {
            Map = map;
            m_cellId = cellId;

            SetFromCellId();
        }

        public MapPoint(Map map, int x, int y)
        {
            Map = map;
            m_x = x;
            m_y = y;

            SetFromCoords();
        }

        public MapPoint(Map map, CellData cellData)
        {
            Map = map;
            m_cellId = cellData.Id;

            SetFromCellId();
        }

        public MapPoint( CellData cellData)
        {
            Map = cellData.ParrentMap;
            m_cellId = cellData.Id;

            SetFromCellId();
        }

        /// <summary>
        /// Get the linked map to this point
        /// </summary>
        public Map Map
        {
            get;
            internal set;
        }

        public CellData Cell
        {
            get { return Map.CellsData[CellId]; }
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


        public uint DistanceTo(MapPoint point)
        {
            return (uint) Math.Sqrt(Math.Pow(point.X - m_x, 2) + Math.Pow(point.Y - m_y, 2));
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

            if (vector == VECTOR_RIGHT)
            {
                return DirectionsEnum.DIRECTION_EAST;
            }
            if (vector == VECTOR_DOWN_RIGHT)
            {
                return DirectionsEnum.DIRECTION_SOUTH_EAST;
            }
            if (vector == VECTOR_DOWN)
            {
                return DirectionsEnum.DIRECTION_SOUTH;
            }
            if (vector == VECTOR_DOWN_LEFT)
            {
                return DirectionsEnum.DIRECTION_SOUTH_WEST;
            }
            if (vector == VECTOR_LEFT)
            {
                return DirectionsEnum.DIRECTION_WEST;
            }
            if (vector == VECTOR_UP_LEFT)
            {
                return DirectionsEnum.DIRECTION_NORTH_WEST;
            }
            if (vector == VECTOR_UP)
            {
                return DirectionsEnum.DIRECTION_NORTH;
            }
            if (vector == VECTOR_UP_RIGHT)
            {
                return DirectionsEnum.DIRECTION_NORTH_EAST;
            }
                
            return DirectionsEnum.DIRECTION_EAST;
        }

        public uint AdvancedOrientationTo(MapPoint point, Boolean param2 = true)
        {
            int x = point.X - m_x;
            int y = m_y - point.Y;
            double _loc_5 = Math.Acos(x/Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)))*180/Math.PI*
                            (point.Y > m_y ? (-1) : (1));
            if (param2)
            {
                _loc_5 = Math.Round(_loc_5/90)*2 + 1;
            }
            else
            {
                _loc_5 = Math.Round(_loc_5/45) + 1;
            }
            if (_loc_5 < 0)
            {
                _loc_5 = _loc_5 + 8;
            }
            return (uint) _loc_5;
        }

        //public MapPoint getNearestFreeCell(Map map, Boolean param2 = true)
        //{
        //    MapPoint _loc_3 = null;
        //    for (int i = 0; i < 8; i++)
        //    {
        //        _loc_3 = GetNearestFreeCellInDirection(i, map, false, param2);
        //        if (_loc_3 != null)
        //            break;
        //    }
        //    return _loc_3;
        //}

        public MapPoint GetNearestCellInDirection(DirectionsEnum direction)
        {
            MapPoint mapPoint = null;
            switch (direction)
            {
                case DirectionsEnum.DIRECTION_EAST:
                    {
                        mapPoint = new MapPoint(Map, m_x + 1, m_y + 1);
                        break;
                    }
                case DirectionsEnum.DIRECTION_SOUTH_EAST:
                    {
                        mapPoint = new MapPoint(Map, m_x + 1, m_y);
                        break;
                    }
                case DirectionsEnum.DIRECTION_SOUTH:
                    {
                        mapPoint = new MapPoint(Map, m_x + 1, m_y - 1);
                        break;
                    }
                case DirectionsEnum.DIRECTION_SOUTH_WEST:
                    {
                        mapPoint = new MapPoint(Map, m_x, m_y - 1);
                        break;
                    }
                case DirectionsEnum.DIRECTION_WEST:
                    {
                        mapPoint = new MapPoint(Map, m_x - 1, m_y - 1);
                        break;
                    }
                case DirectionsEnum.DIRECTION_NORTH_WEST:
                    {
                        mapPoint = new MapPoint(Map, m_x - 1, m_y);
                        break;
                    }
                case DirectionsEnum.DIRECTION_NORTH:
                    {
                        mapPoint = new MapPoint(Map, m_x - 1, m_y + 1);
                        break;
                    }
                case DirectionsEnum.DIRECTION_NORTH_EAST:
                    {
                        mapPoint = new MapPoint(Map, m_x, m_y + 1);
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

        //public MapPoint GetNearestFreeCellInDirection(int direction, Map param2, Boolean param3 = true, Boolean param4 = true)
        //{
        //MapPoint _loc_5 = null;
        //uint _loc_6 = 0;
        //do
        //{
        //    _loc_5 = GetNearestCellInDirection(direction);
        //    if (_loc_5 && !param2.pointMov(_loc_5.m_x, _loc_5.m_y, param4))
        //    {
        //        direction = (direction + 1) % 8;

        //        _loc_5 = null;
        //    }
        //} while (_loc_5 == null && _loc_6++ < 8);
        //if (!_loc_5 && param3 && param2.pointMov(this._nX, this._nY, param4))
        //{
        //    return this;
        //}
        //return _loc_5;
        // }

        public override string ToString()
        {
            return "[MapPoint(x:" + m_x + ", y:" + m_y + ", id:" + m_cellId + ")]";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (MapPoint)) return false;
            return Equals((MapPoint) obj);
        }

        private void SetFromCoords()
        {
            if (!m_initialized)
                InitializeStaticGrid();

            m_cellId = (ushort) ((m_x - m_y)*MAP_WIDTH + m_y + (m_x - m_y)/2);
        }

        private void SetFromCellId()
        {
            if (!m_initialized)
                InitializeStaticGrid();

            if (m_cellId < 0 || m_cellId > Map.MaximumCellsCount)
                throw new IndexOutOfRangeException("Cell identifier out of bounds (" + m_cellId + ").");

            Point point = OrthogonalGridReference[m_cellId];
            m_x = point.X;
            m_y = point.Y;
        }

        public static bool IsInMap(int x, int y)
        {
            return x + y >= 0 && x - y >= 0 && x - y < MAP_HEIGHT*2 && x + y < MAP_WIDTH*2;
        }

        public static uint CoordToCellId(int param1, int param2)
        {
            if (!m_initialized)
                InitializeStaticGrid();
            return (uint) ((param1 - param2)*MAP_WIDTH + param2 + (param1 - param2)/2);
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
            int _loc_1 = 0;
            int _loc_2 = 0;
            int cellCount = 0;

            for (int x = 0; x < MAP_HEIGHT; x++)
            {
                for (int y = 0; y < MAP_WIDTH; y++)
                    OrthogonalGridReference[cellCount++] = new Point(_loc_1 + y, _loc_2 + y);

                _loc_1++;

                for (int y = 0; y < MAP_WIDTH; y++)
                    OrthogonalGridReference[cellCount++] = new Point(_loc_1 + y, _loc_2 + y);

                _loc_2--;
            }

            m_initialized = true;
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