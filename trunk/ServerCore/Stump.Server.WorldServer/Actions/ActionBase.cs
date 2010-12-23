using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Actions
{
    public abstract class ActionBase
    {
        public ActionBaseArgument Argument
        {
            get;
            protected set;
        }

        protected ActionBase(ActionBaseArgument argument)
        {
            Argument = argument;
        }

        public abstract void Execute();
    }
}