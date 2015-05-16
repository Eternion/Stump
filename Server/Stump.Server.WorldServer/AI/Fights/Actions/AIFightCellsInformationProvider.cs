using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;

namespace Stump.Server.WorldServer.AI.Fights.Actions
{
    public class AIFightCellsInformationProvider : FigthCellsInformationProvider
    {
        public AIFightCellsInformationProvider(IFight fight, AIFighter fighter)
            : base(fight)
        {
            Fighter = fighter;
        }

        public AIFighter Fighter
        {
            get;
            private set;
        }

        public override CellInformation GetCellInformation(short cell)
        {
            // ignore the cell where the fighter already is (in case we perform pathfinding with others cells and suppose the fighter has already moved)
            return new CellInformation(Fight.Map.Cells[cell], Fighter.Cell.Id  == cell || IsCellWalkable(cell), true, true, 1, null, null);
        }
    }
}