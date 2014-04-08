using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.Commands.Commands
{
    public class ListCommand : CommandBase
    {
        public ListCommand()
        {
            Aliases = new [] { "commandslist" };
            RequiredRole = RoleEnum.Player;
            Description = "List all available commands";
            Parameters = new List<IParameterDefinition>
            {
                new ParameterDefinition<string>("command", "cmd", "List specifics sub commands", string.Empty),
                new ParameterDefinition<RoleEnum>("role", "role", "List commands available for a given role", RoleEnum.None, true) // we must define isOptional = true because RoleEnum.None = default(RoleEnum)
            };
        }

        public override void Execute(TriggerBase trigger)
        {
            var role = trigger.Get<RoleEnum>("role");
            var cmd = trigger.Get<string>("command");

            role = role != RoleEnum.None && role <= trigger.UserRole ?
                role : trigger.UserRole;

            IEnumerable<CommandBase> availableCommands =
                CommandManager.Instance.AvailableCommands.Where(entry => !(entry is SubCommand));
            if (cmd != string.Empty)
            {
                var command = CommandManager.Instance.GetCommand(cmd);

                if (command == null)
                {
                    trigger.ReplyError("Cannot found '{0}'", command);
                    return;
                }

                if (command is SubCommandContainer)
                    availableCommands = (command as SubCommandContainer); // if a command is specified we display his childrens
                else
                    availableCommands = new []{command};
            }

            var commands = from entry in availableCommands
                           where entry.RequiredRole <= role
                           select entry;

            trigger.Reply(string.Join(", ", from entry in commands
                                            select (entry is SubCommandContainer ?
                                                string.Format(trigger.CanFormat ? "<b>{0}</b>({1})" : "{0}({1})", entry.Aliases.First(), ( entry as SubCommandContainer ).Count)
                                                : entry.Aliases.First())));
        }
    }
}