
using System;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Global
{
    /// <summary>
    /// Represent a position from an object on a map
    /// </summary>
    public class ObjectPosition
    {
        public ObjectPosition(CellLinked cell, DirectionsEnum direction = DirectionsEnum.DIRECTION_NONE)
        {
            Cell = cell;
            Direction = direction;
        }

        public ObjectPosition(Map map, ushort cellId, DirectionsEnum direction = DirectionsEnum.DIRECTION_NONE)
        {
            Cell = map.GetCell(cellId);
            Direction = direction;
        }

        public ObjectPosition(int mapId, ushort cellId, DirectionsEnum direction = DirectionsEnum.DIRECTION_NONE)
        {
            Cell = World.Instance.GetMap(mapId).GetCell(cellId);
            Direction = direction;
        }

        public ObjectPosition(MapPoint point, DirectionsEnum direction = DirectionsEnum.DIRECTION_NONE)
        {
            Point = point;
            Direction = direction;
        }

        public CellLinked Cell
        {
            get;
            set;
        }

        public ushort CellId
        {
            get { return Cell.Id; }
            set
            {
                if (value < 0 || value > Map.MaximumCellsCount)
                    throw new ArgumentException("value must be between 0 and " + Map.MaximumCellsCount);

                Cell = Cell.ParrentMap.GetCell(value);
            }
        }

        public MapPoint Point
        {
            get { return Cell.Point; }
            set { CellId = value.CellId; }
        }

        public DirectionsEnum Direction
        {
            get;
            set;
        }

        public Map Map
        {
            get { return Cell.ParrentMap; }
            set { Cell = value.GetCell(Cell.Id); }
        }
    }
}