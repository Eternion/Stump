using Stump.Server.WorldServer.Game.Actors.Fight;
using TreeSharp;

namespace Stump.Server.WorldServer.AI.Fights.Actions
{
    public abstract class AIAction : Action
    {
        protected AIAction(AIFighter fighter)
        {
            Fighter = fighter;
        }

        public AIFighter Fighter
        {
            get;
        }

        public RunStatus YieldExecute(object context)
        {
            if (Fighter.IsDead())
                return RunStatus.Failure;

            return Run(context);
        }

        protected override RunStatus Run(object context) => RunStatus.Failure;
    }
}