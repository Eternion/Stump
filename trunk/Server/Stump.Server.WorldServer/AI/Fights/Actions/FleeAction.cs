using System;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;

namespace Stump.Server.WorldServer.AI.Fights.Actions
{
    public class FleeAction : AIAction
    {
        public FleeAction(AIFighter fighter)
            : base(fighter)
        {
        }

        public override void Execute()
        {
            var fleeCell = Fighter.Brain.Environnment.GetCellToFlee();

            if (fleeCell.Equals(default(Cell)))
                return;

            var moveaction = new MoveAction(Fighter, fleeCell);
            moveaction.Execute();
        }
    }
}