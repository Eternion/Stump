using System.Collections.Generic;
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
            private set;
        }

        public RunStatus YieldExecute(object context)
        {
            return Run(context);
        }

        protected override RunStatus Run(object context)
        {
            return RunStatus.Failure;
        }
    }
}