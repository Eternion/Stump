using System;
using System.Collections.Generic;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
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
            if (!Fighter.CanMove())
                return RunStatus.Failure;

            var fleeCell = Fighter.Brain.Environment.GetCellToFlee();

            if (fleeCell.Equals(Cell.Null))
                return RunStatus.Failure;

            if (fleeCell.Id == Fighter.Cell.Id)
                return RunStatus.Success;

            var moveaction = new MoveAction(Fighter, fleeCell);
            return moveaction.YieldExecute(context);
        }
    }
}