using Stump.Server.WorldServer.Worlds.Actors.Fight;

namespace Stump.Server.WorldServer.AI.Fights.Actions
{
    public abstract class AIAction
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

        public abstract void Execute();
    }
}