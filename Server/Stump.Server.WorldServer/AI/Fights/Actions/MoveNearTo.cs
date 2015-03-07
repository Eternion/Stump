using System.Linq;
using Stump.Server.WorldServer.Game.Actors.Fight;
using TreeSharp;

namespace Stump.Server.WorldServer.AI.Fights.Actions
{
    public class MoveNearTo : AIAction
    {
        public MoveNearTo(AIFighter fighter, FightActor target)
            : base(fighter)
        {
            Target = target;
        }

        public FightActor Target
        {
            get;
            private set;
        }

        protected override RunStatus Run(object context)
        {
            if (!Fighter.CanMove())
                return RunStatus.Failure;

            if (Target == null)
                return RunStatus.Failure;

            var cellInfoProvider = new AIFightCellsInformationProvider(Fighter.Fight, Fighter);

            if (Fighter.Position.Point.IsAdjacentTo(Target.Position.Point))
                return RunStatus.Success;

            // todo : avoid tackle
            var cell = Target.Position.Point.GetAdjacentCells(cellInfoProvider.IsCellWalkable).OrderBy(entry => entry.ManhattanDistanceTo(Fighter.Position.Point)).FirstOrDefault();

            if (cell == null)
                return RunStatus.Failure;

            var moveAction = new MoveAction(Fighter, cell);
            return moveAction.YieldExecute(context);
        }
    }
}