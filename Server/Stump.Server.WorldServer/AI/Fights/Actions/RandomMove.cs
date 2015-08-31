using System;
using System.Linq;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;
using TreeSharp;

namespace Stump.Server.WorldServer.AI.Fights.Actions
{
    public class RandomMove : AIAction
    {
        public RandomMove(AIFighter fighter) : base(fighter)
        {
        }

        protected override RunStatus Run(object context)
        {
            if (!Fighter.CanMove())
                return RunStatus.Failure;

            var lozenge = new Lozenge(0, (byte) Fighter.MP);
            var cells = lozenge.GetCells(Fighter.Cell, Fighter.Fight.Map).Where(entry => Fighter.Fight.IsCellFree(entry)).ToArray();

            if (cells.Length == 0)
                return RunStatus.Failure;

            var rand = new Random();
            var cell = cells[rand.Next(cells.Length)];

            var moveAction = new MoveAction(Fighter, cell);
            return moveAction.YieldExecute(context);
        }
    }
}