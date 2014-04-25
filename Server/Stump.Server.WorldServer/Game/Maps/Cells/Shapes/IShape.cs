using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Game.Maps.Cells.Shapes
{
    public interface IShape
    {
        uint Surface
        {
            get;
        }

        byte MinRadius
        {
            get;
            set;
        }

        DirectionsEnum Direction
        {
            get;
            set;
        }

        byte Radius
        {
            get;
            set;
        }

        Cell[] GetCells(Cell centerCell, Map map);
    }
}