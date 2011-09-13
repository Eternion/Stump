using System.Collections;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Worlds.Maps.Cells.Shapes
{
    public interface IShape
    {
        uint Surface
        {
            get;
        }

        uint MinRadius
        {
            get;
            set;
        }

        DirectionsEnum Direction
        {
            get;
            set;
        }

        uint Radius
        {
            get;
            set;
        }

        Cell[] GetCells(Cell centerCell, Map map);
    }
}