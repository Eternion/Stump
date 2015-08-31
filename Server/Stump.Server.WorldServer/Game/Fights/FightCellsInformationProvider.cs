using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class FigthCellsInformationProvider : ICellsInformationProvider
    {
        public FigthCellsInformationProvider(IFight fight)
        {
            Fight = fight;
        }

        public IFight Fight
        {
            get;
            private set;
        }

        #region ICellsInformationProvider Members

        public Map Map
        {
            get
            {
                return Fight.Map;
            }
        }

        public virtual bool IsCellWalkable(short cell)
        {
            return Fight.IsCellFree(Fight.Map.Cells[cell]);
        }

        public virtual CellInformation GetCellInformation(short cell)
        {
            return new CellInformation(Fight.Map.Cells[cell], IsCellWalkable(cell), true);
        }

        #endregion
    }
}