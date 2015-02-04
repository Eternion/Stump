using Stump.Server.WorldServer.Game.Actors.Fight;
using TreeSharp;

namespace Stump.Server.WorldServer.AI.Fights.Actions
{
    public class MoveFarFrom : AIAction
    {
        public MoveFarFrom(AIFighter fighter, FightActor from)
            : base(fighter)
        {
            From = from;
        }

        public FightActor From
        {
            get;
            private set;
        }

        protected override RunStatus Run(object context)
        {
            if (!Fighter.CanMove())
                return RunStatus.Failure;

            if (From == null)
                return RunStatus.Failure;

            var orientation = From.Position.Point.OrientationTo(Fighter.Position.Point);
            var destination = Fighter.Position.Point.GetCellInDirection(orientation, (short) Fighter.MP);

            if (destination == null)
                return RunStatus.Failure;

            var moveAction = new MoveAction(Fighter, destination);
            return moveAction.YieldExecute(context);
        }
    }
}