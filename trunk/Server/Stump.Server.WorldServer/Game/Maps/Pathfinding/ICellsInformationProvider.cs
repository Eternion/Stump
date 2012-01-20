namespace Stump.Server.WorldServer.Game.Maps.Pathfinding
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