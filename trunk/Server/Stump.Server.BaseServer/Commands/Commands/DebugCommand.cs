using System;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.Commands.Commands
{
    public class DebugCommand : SubCommandContainer
    {
        public DebugCommand()
        {
            Aliases = new[] { "debug" };
            RequiredRole = RoleEnum.Moderator;
            Description = "Provides command to debug things";
        }
    }

    public class CommandsExceptions : SubCommand
    {
        public CommandsExceptions()
        {
            Aliases = new[] {"cmderror"};
            ParentCommand = typeof (DebugCommand);
            RequiredRole = RoleEnum.Moderator;
            Description = "Give command error details";
            AddParameter<int>("index", "i", "Error index (last if not defined)", isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            int index;

            if (!trigger.IsArgumentDefined("index"))
                index = trigger.User.CommandsErrors.Count - 1; // last index
            else index = trigger.Get<int>("index");

            if (trigger.User.CommandsErrors.Count <= index)
            {
                trigger.ReplyError("No error at index {0}", index);
                return;
            }

            var pair = trigger.User.CommandsErrors[index];

            trigger.Reply("Command : " + pair.Key);
            trigger.Reply("Exception : ");

            foreach (var line in pair.Value.ToString().Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries))
            {
                trigger.Reply(line);
            }
        }
    }

    public class CommandGlobalExceptions : SubCommand
    {
        public CommandGlobalExceptions()
        {
            Aliases = new[] { "error" };
            ParentCommand = typeof(DebugCommand);
            RequiredRole = RoleEnum.Moderator;
            Description = "Give error details";
            AddParameter<int>("index", "i", "Error index (last if not defined)", isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            int index;

            if (!trigger.IsArgumentDefined("index"))
                index = trigger.User.CommandsErrors.Count - 1; // last index
            else index = trigger.Get<int>("index");

            if (trigger.User.CommandsErrors.Count <= index)
            {
                trigger.ReplyError("No error at index {0}", index);
                return;
            }

            var pair = trigger.User.CommandsErrors[index];

            trigger.Reply("Command : " + pair.Key);
            trigger.Reply("Exception : ");

            foreach (var line in pair.Value.ToString().Split(new [] {'\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                trigger.Reply(line);
            }
        }
    }
}