
using System;
using System.Collections.Generic;
using Stump.Database;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Network;
using Stump.Server.BaseServer.Commands;

namespace Stump.Server.AuthServer.Commands
{
    public class WorkerCommands : AuthCommand
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

    public class WorkerBenchmarkCommand : AuthSubCommand
    {
        public WorkerBenchmarkCommand()
        {
            Aliases = new[] { "benchmark", "bench" };
            ParentCommand = typeof(WorkerCommands);
            RequiredRole = RoleEnum.Administrator;
            Description = "Display the benchmark of treated messages";
            Parameters = new List<IParameter>
                {
                    new ParameterDefinition<bool>("order", "o", "Order messages by average treatment time", true, true),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            trigger.Reply(AuthentificationServer.Instance.WorkerManager.
                GetDetailedMessageTypes(trigger.Get<bool>("order")));
        }
    }

    public class WorkerMessageCommand : AuthSubCommand
    {
        public WorkerMessageCommand()
        {
            Aliases = new[] { "message", "msg" };
            ParentCommand = typeof(WorkerCommands);
            RequiredRole = RoleEnum.Administrator;
            Description = "Display detailed treatment of a message";
            Parameters = new List<IParameter>
                {
                    new ParameterDefinition<string>("name", "n", "Name of the message"),
                    new ParameterDefinition<bool>("order", "o", "Order messages by average treatment time", true, true),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            trigger.Reply(AuthentificationServer.Instance.WorkerManager.GetDetailedMessages(
                trigger.Get<string>("name"),
                trigger.Get<bool>("order")));
        }
    }

    public class WorkerMessageIdCommand : AuthSubCommand
    {
        public WorkerMessageIdCommand()
        {
            Aliases = new[] { "messageid", "msgid" };
            ParentCommand = typeof(WorkerCommands);
            RequiredRole = RoleEnum.Administrator;
            Description = "Display info about the treatment of a unique message id";
            Parameters = new List<IParameter>
                {
                    new ParameterDefinition<string>("name", "n", "Name of the message"),
                    new ParameterDefinition<int>("id", "id", "Unique message id's treatment"),
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            trigger.Reply(AuthentificationServer.Instance.WorkerManager.GetDetailedMessage(
                trigger.Get<string>("name"),
                trigger.Get<int>("id")));
        }
    }
}