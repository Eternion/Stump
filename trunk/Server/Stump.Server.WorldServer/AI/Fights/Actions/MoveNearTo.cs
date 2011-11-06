using System;
using System.Linq;
using Stump.Server.WorldServer.Worlds.Actors.Fight;

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

        public override void Execute()
        {
            var cellInfoProvider = new AIFightCellsInformationProvider(Fighter.Fight, Fighter);

            // todo : avoid tackle
            var cell = Target.Position.Point.GetAdjacentCells(cellInfoProvider.IsCellWalkable).OrderBy(entry => entry.DistanceTo(Fighter.Position.Point)).FirstOrDefault();

            if (cell == null)
                return;

            var moveAction = new MoveAction(Fighter, cell);
            moveAction.Execute();
        }
    }
}