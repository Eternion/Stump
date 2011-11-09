using System;
using System.Collections.Generic;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using TreeSharp;

namespace Stump.Server.WorldServer.AI.Fights.Actions
{
    public class FleeAction : AIAction
    {
        public FleeAction(AIFighter fighter)
            : base(fighter)
        {
        }

        protected override RunStatus Run(object context)
        {
            var fleeCell = Fighter.Brain.Environment.GetCellToFlee();

            if (fleeCell.Equals(default(Cell)))
                return RunStatus.Failure;

            var moveaction = new MoveAction(Fighter, fleeCell);
            return moveaction.YieldExecute(context);
        }
    }
}