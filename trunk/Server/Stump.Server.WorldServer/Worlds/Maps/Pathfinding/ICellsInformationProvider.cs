using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Worlds.Maps.Pathfinding
{
    public interface ICellsInformationProvider
    {
        Map Map
        {
            get;
        }

        bool IsCellWalkable(short cell);
        CellInformation GetCellInformation(short cell);
    }
}