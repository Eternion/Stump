
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;

namespace Stump.Server.WorldServer.Commands
{
    public class WorkerCommands : WorldCommand
    {
        public WorkerCommands()
        {
            Aliases = new[] { "worker", "wk" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Manage worker's.";
        }

        public override void Execute(TriggerBase trigger)
        {
            throw new DummyCommandException();
        }
    }

    public class WorkerBenchmarkCommand : WorldSubCommand
    {
        public WorkerBenchmarkCommand()
        {
            Aliases = new[] { "benchmark", "bench" };
            ParentCommand = typeof(WorkerCommands);
            RequiredRole = RoleEnum.Administrator;
            Description = "Display the benchmark of treated messages";
            Parameters = new List<ICommandParameter>
                {
                    new CommandParameter<bool>("order", "o", "Order messages by average treatment time", true, true),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            trigger.Reply(WorldServer.Instance.WorkerManager.
                GetDetailedMessageTypes(trigger.GetArgument<bool>("order")));
        }
    }

    public class WorkerMessageCommand : WorldSubCommand
    {
        public WorkerMessageCommand()
        {
            Aliases = new[] { "message", "msg" };
            ParentCommand = typeof(WorkerCommands);
            RequiredRole = RoleEnum.Administrator;
            Description = "Display detailed treatment of a message";
            Parameters = new List<ICommandParameter>
                {
                    new CommandParameter<string>("name", "n", "Name of the message"),
                    new CommandParameter<bool>("order", "o", "Order messages by average treatment time", true, true),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            trigger.Reply(WorldServer.Instance.WorkerManager.GetDetailedMessages(
                trigger.GetArgument<string>("name"),
                trigger.GetArgument<bool>("order")));
        }
    }

    public class WorkerMessageIdCommand : WorldSubCommand
    {
        public WorkerMessageIdCommand()
        {
            Aliases = new[] { "messageid", "msgid" };
            ParentCommand = typeof(WorkerCommands);
            RequiredRole = RoleEnum.Administrator;
            Description = "Display info about the treatment of a unique message id";
            Parameters = new List<ICommandParameter>
                {
                    new CommandParameter<string>("name", "n", "Name of the message"),
                    new CommandParameter<int>("id", "id", "Unique message id's treatment"),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            trigger.Reply(WorldServer.Instance.WorkerManager.GetDetailedMessage(
                trigger.GetArgument<string>("name"),
                trigger.GetArgument<int>("id")));
        }
    }
}