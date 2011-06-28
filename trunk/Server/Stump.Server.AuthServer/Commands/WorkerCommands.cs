
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;

namespace Stump.Server.AuthServer.Commands
{
    public class WorkerCommands : SubCommandContainer
    {
        public WorkerCommands()
        {
            Aliases = new[] { "worker", "wk" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Manage worker's.";
        }
    }

    public class WorkerBenchmarkCommand : SubCommand
    {
        public WorkerBenchmarkCommand()
        {
            Aliases = new[] { "benchmark", "bench" };
            ParentCommand = typeof(WorkerCommands);
            RequiredRole = RoleEnum.Administrator;
            Description = "Display the benchmark of treated messages";
            Parameters = new List<IParameterDefinition>
                {
                    new ParameterDefinition<bool>("order", "o", "Order messages by average treatment time", true),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            trigger.Reply(AuthServer.Instance.WorkerManager.
                GetDetailedMessageTypes(trigger.Get<bool>("order")));
        }
    }

    public class WorkerMessageCommand : SubCommand
    {
        public WorkerMessageCommand()
        {
            Aliases = new[] { "message", "msg" };
            ParentCommand = typeof(WorkerCommands);
            RequiredRole = RoleEnum.Administrator;
            Description = "Display detailed treatment of a message";
            Parameters = new List<IParameterDefinition>
                {
                    new ParameterDefinition<string>("name", "n", "Name of the message"),
                    new ParameterDefinition<bool>("order", "o", "Order messages by average treatment time", true),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            trigger.Reply(AuthServer.Instance.WorkerManager.GetDetailedMessages(
                trigger.Get<string>("name"),
                trigger.Get<bool>("order")));
        }
    }

    public class WorkerMessageIdCommand : SubCommand
    {
        public WorkerMessageIdCommand()
        {
            Aliases = new[] { "messageid", "msgid" };
            ParentCommand = typeof(WorkerCommands);
            RequiredRole = RoleEnum.Administrator;
            Description = "Display info about the treatment of a unique message id";
            Parameters = new List<IParameterDefinition>
                {
                    new ParameterDefinition<string>("name", "n", "Name of the message"),
                    new ParameterDefinition<int>("id", "id", "Unique message id's treatment"),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            trigger.Reply(AuthServer.Instance.WorkerManager.GetDetailedMessage(
                trigger.Get<string>("name"),
                trigger.Get<int>("id")));
        }
    }
}