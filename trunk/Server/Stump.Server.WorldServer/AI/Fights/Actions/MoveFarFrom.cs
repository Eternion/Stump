using System;
using Stump.Server.WorldServer.Worlds.Actors.Fight;

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

        public override void Execute()
        {
            var orientation = From.Position.Point.OrientationTo(Fighter.Position.Point);
            var destination = Fighter.Position.Point.GetCellInDirection(orientation, (short) Fighter.MP);

            if (destination == null)
                return;

            var moveAction = new MoveAction(Fighter, destination);
            moveAction.Execute();
        }
    }
}