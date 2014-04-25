using System;

namespace Stump.Server.BaseServer.Commands.Patterns
{
    public abstract class AddRemoveCommand : CommandBase
    {
        protected AddRemoveCommand()
        {
            AddParameter<string>("action", "action", "Add/Remove");
        }

        public override void Execute(TriggerBase trigger)
        {
            var action = trigger.Get<string>("action");

            if (action.Equals("add", StringComparison.InvariantCultureIgnoreCase))
            {
                ExecuteAdd(trigger);
                return;
            }

            if (action.Equals("remove", StringComparison.InvariantCultureIgnoreCase) || action.Equals("rm", StringComparison.InvariantCultureIgnoreCase))
            {
                ExecuteRemove(trigger);
            }
            else
            {
                trigger.ReplyError("Invalid action {0}, define add or remove", action);
            }
        }

        public abstract void ExecuteAdd(TriggerBase trigger);
        public abstract void ExecuteRemove(TriggerBase trigger);
    }

    public abstract class AddRemoveSubCommand : SubCommand
    {
        protected AddRemoveSubCommand()
        {
            AddParameter<string>("action", "action", "Add/Remove");
        }

        public override void Execute(TriggerBase trigger)
        {
            var action = trigger.Get<string>("action");

            if (action.Equals("add", StringComparison.InvariantCultureIgnoreCase))
            {
                ExecuteAdd(trigger);
                return;
            }

            if (action.Equals("remove", StringComparison.InvariantCultureIgnoreCase) ||
                action.Equals("rm", StringComparison.InvariantCultureIgnoreCase))
            {
                ExecuteRemove(trigger);
            }

            else
            {
                trigger.ReplyError("Invalid action {0}, define add or remove", action);
            }
        }

        public abstract void ExecuteAdd(TriggerBase trigger);
        public abstract void ExecuteRemove(TriggerBase trigger);
    }
}