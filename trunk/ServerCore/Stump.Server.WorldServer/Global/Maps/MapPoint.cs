using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Global.Maps
{
    class MapPoint
    {

        private int m_cellId;
        public int CellId
        {
            get { return m_cellId; }
            set
            {
                m_cellId = value;
                SetFromCellId();
            }
        }

        private int m_x;
        public int X
        {
            get { return m_x; }
            set
            {
                m_x = value;
                SetFromCoords();
            }
        }

        private int m_y;
        public int Y
        {
            get { return m_y; }
            set
            {
                m_y = value;
                SetFromCoords();
            }
        }

        private static Point VECTOR_RIGHT = new Point(1, 1);
        private static Point VECTOR_DOWN_RIGHT = new Point(1, 0);
        private static Point VECTOR_DOWN = new Point(1, -1);
        private static Point VECTOR_DOWN_LEFT = new Point(0, -1);
        private static Point VECTOR_LEFT = new Point(-1, -1);
        private static Point VECTOR_UP_LEFT = new Point(-1, 0);
        private static Point VECTOR_UP = new Point(-1, 1);
        private static Point VECTOR_UP_RIGHT = new Point(0, 1);
        public const uint MAP_WIDTH = 14;
        public const uint MAP_HEIGHT = 20;
        private static bool m_initialized = false;
        private static Point[] CELLPOS = new Point[MAP_WIDTH * MAP_HEIGHT * 2];


        public uint DistanceTo(MapPoint point)
        {
            return (uint)Math.Sqrt(Math.Pow(point.X - m_x, 2) + Math.Pow(point.Y - m_y, 2));
        }

        public int DistanceToCell(MapPoint point)
        {
            return Math.Abs(m_x - point.X) + Math.Abs(m_y - point.Y);
        }

        public DirectionsEnum OrientationTo(MapPoint point)
        {
            Point _loc_2 = new Point();
            _loc_2.X = point.X > m_x ? (1) : (point.X < m_x ? (-1) : (0));
            _loc_2.Y = point.Y > m_y ? (1) : (point.Y < m_y ? (-1) : (0));

            if (_loc_2 == VECTOR_RIGHT)
            {
                return DirectionsEnum.DIRECTION_EAST;
            }
            else if (_loc_2 == VECTOR_DOWN_RIGHT)
            {
                return DirectionsEnum.DIRECTION_SOUTH_EAST;
            }
            else if (_loc_2 == VECTOR_DOWN)
            {
                return DirectionsEnum.DIRECTION_SOUTH;
            }
            else if (_loc_2 == VECTOR_DOWN_LEFT)
            {
                return DirectionsEnum.DIRECTION_SOUTH_WEST;
            }
            else if (_loc_2 == VECTOR_LEFT)
            {
                return DirectionsEnum.DIRECTION_WEST;
            }
            else if (_loc_2 == VECTOR_UP_LEFT)
            {
                return DirectionsEnum.DIRECTION_NORTH_WEST;
            }
            else if (_loc_2 == VECTOR_UP)
            {
                return DirectionsEnum.DIRECTION_NORTH;
            }
            else if (_loc_2 == VECTOR_UP_RIGHT)
            {
                return DirectionsEnum.DIRECTION_NORTH_EAST;
            }
            else
            {
                return DirectionsEnum.DIRECTION_EAST;
            }
        }

        public uint AdvancedOrientationTo(MapPoint point, Boolean param2 = true)
        {
            int x = point.X - m_x;
            int y = m_y - point.Y;
            var _loc_5 = Math.Acos(x / Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2))) * 180 / Math.PI * (point.Y > m_y ? (-1) : (1));
            if (param2)
            {
                _loc_5 = Math.Round(_loc_5 / 90) * 2 + 1;
            }
            else
            {
                _loc_5 = Math.Round(_loc_5 / 45) + 1;
            }
            if (_loc_5 < 0)
            {
                _loc_5 = _loc_5 + 8;
            }
            return (uint)_loc_5;
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

        //public MapPoint GetNearestCellInDirection(int direction)
        //{
        //    MapPoint mapPoint;
        //    switch (direction)
        //    {
        //        case 0:
        //            {
        //                mapPoint = MapPoint.FromCoords(m_x + 1, m_y + 1);
        //                break;
        //            }
        //        case 1:
        //            {
        //                mapPoint = MapPoint.FromCoords(m_x + 1, m_y);
        //                break;
        //            }
        //        case 2:
        //            {
        //                mapPoint = MapPoint.FromCoords(m_x + 1, m_y - 1);
        //                break;
        //            }
        //        case 3:
        //            {
        //                mapPoint = MapPoint.FromCoords(m_x, m_y - 1);
        //                break;
        //            }
        //        case 4:
        //            {
        //                mapPoint = MapPoint.FromCoords(m_x - 1, m_y - 1);
        //                break;
        //            }
        //        case 5:
        //            {
        //                mapPoint = MapPoint.FromCoords(m_x - 1, m_y);
        //                break;
        //            }
        //        case 6:
        //            {
        //                mapPoint = MapPoint.FromCoords(m_x - 1, m_y + 1);
        //                break;
        //            }
        //        case 7:
        //            {
        //                mapPoint = MapPoint.FromCoords(m_x, m_y + 1);
        //                break;
        //            }
        //        default:
        //            {
        //                return null;
        //            }
        //    }
        //    if (MapPoint.IsInMap(mapPoint.m_x, mapPoint.m_y))
        //        return mapPoint;
        //    else
        //        return null;
        //}

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
            MapPoint mp = obj as MapPoint;
            if (mp == null) return false;
            return m_cellId == mp.m_cellId;
        }

        private void SetFromCoords()
        {
            if (!m_initialized)
                Init();
            m_cellId = (int)((m_x - m_y) * MAP_WIDTH + m_y + (m_x - m_y) / 2);
        }

        private void SetFromCellId()
        {
            if (!m_initialized)
                Init();

            if (m_cellId < 0 || m_cellId > 560)
                throw new IndexOutOfRangeException("Cell identifier out of bounds (" + m_cellId + ").");

            var _loc_1 = CELLPOS[this.m_cellId];
            this.m_x = _loc_1.X;
            this.m_y = _loc_1.Y;
        }

        public static MapPoint FromCellId(int cellId)
        {
            MapPoint mapPoint = new MapPoint();
            mapPoint.m_cellId = cellId;
            mapPoint.SetFromCellId();
            return mapPoint;
        }

        public static MapPoint FromCoords(int x, int y)
        {
            MapPoint mapPoint = new MapPoint();
            mapPoint.m_x = x;
            mapPoint.m_y = y;
            mapPoint.SetFromCoords();
            return mapPoint;
        }

        public static bool IsInMap(int x, int y)
        {
            return x + y >= 0 && x - y >= 0 && x - y < MAP_HEIGHT * 2 && x + y < MAP_WIDTH * 2;
        }

        public static uint CoordToCellId(int param1, int param2)
        {
            if (!m_initialized)
                Init();
            return (uint)((param1 - param2) * MAP_WIDTH + param2 + (param1 - param2) / 2);
        }

        public static Point CellIdToCoord(uint param1)
        {
            if (!m_initialized)
                Init();
            return CELLPOS[param1];
        }

        private static void Init()
        {
            int _loc_1 = 0;
            int _loc_2 = 0;
            int cellCount = 0;

            for (int x = 0; x < MAP_HEIGHT; x++)
            {
                for (int y = 0; y < MAP_WIDTH; y++)
                    CELLPOS[cellCount++] = new Point(_loc_1 + y, _loc_2 + y);

                _loc_1++;

                for (int y = 0; y < MAP_WIDTH; y++)
                    CELLPOS[cellCount++] = new Point(_loc_1 + y, _loc_2 + y);

                _loc_2--;
            }

            m_initialized = true;
        }

    }
}
