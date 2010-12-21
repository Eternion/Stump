using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Global
{
    /// <summary>
    /// Represent a vector on a 2D isometric plan.
    /// </summary>
    public class VectorIsometric
    {
        public VectorIsometric(MapPoint point)
        {
            Point = point;
        }

        public VectorIsometric(CellData cellData)
        {
            Point = new MapPoint(cellData);
        }

        public VectorIsometric(Map map, ushort cellId)
        {
            Point = new MapPoint(map, cellId);
        }

        public VectorIsometric(MapPoint point, DirectionsEnum direction)
        {
            Point = point;
            Direction = direction;
        }

        public VectorIsometric(CellData cellData, DirectionsEnum direction)
        {
            Point = new MapPoint(cellData);
            Direction = direction;
        }

        public VectorIsometric(Map map, ushort cellId, DirectionsEnum direction)
        {
            Direction = direction;
            Point = new MapPoint(map, cellId);
        }

        public VectorIsometric(Map map, VectorIsometric vectorIsometric)
        {
            Direction = vectorIsometric.Direction;
            Point = new MapPoint(map, vectorIsometric.CellId);
        }

        public DirectionsEnum Direction
        {
            get;
            set;
        }

        public MapPoint Point
        {
            get;
            set;
        }

        public Map Map
        {
            get { return Point.Map; }
        }

        public ushort CellId
        {
            get
            {
                return Point.CellId;
            }
            set
            {
                if (value >= Map.MaximumCellsCount)
                    throw new ArgumentException("'value' must be lesser than " + Map.MaximumCellsCount);

                Point.CellId = value;
            }
        }

        public CellData Cell
        {
            get { return Point.Cell; }
            set
            {
                if (Map != null && Map.CellsData.Contains(value))
                {
                    Point.CellId = (ushort)Map.CellsData.FindIndex(entry => entry == value);
                }
            }
        }

        public void ChangeLocation(DirectionsEnum direction)
        {
            Direction = direction;
        }

        public void ChangeLocation(ushort cellId)
        {
            CellId = cellId;
        }

        public void ChangeLocation(MapPoint mapPoint)
        {
            Point = mapPoint;
        }

        public void ChangeLocation(CellData cellData)
        {
            Point = new MapPoint(cellData);
        }

        public void ChangeLocation(Map map)
        {
            Point = new MapPoint(map, Point.CellId);
        }

        public void ChangeLocation(Map map, ushort cellId)
        {
            Point = new MapPoint(map, cellId);
        }

        public void ChangeLocation(VectorIsometric vectorIsometric)
        {
            Direction = vectorIsometric.Direction;
            Point = vectorIsometric.Point;
        }
    }
}