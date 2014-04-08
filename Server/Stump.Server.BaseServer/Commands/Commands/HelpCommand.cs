using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.Commands.Commands
{
    public class HelpCommand : CommandBase
    {
        public HelpCommand()
        {
            Aliases = new[] {"help", "?"};
            RequiredRole = RoleEnum.Player;
            Description = "List all available commands";
            Parameters = new List<IParameterDefinition>
                             {
                                 new ParameterDefinition<string>("command", "cmd", "Display the complete help of a command", string.Empty),
                                 new ParameterDefinition<string>("subcommand", "subcmd", "Display the complete help of a subcommand", string.Empty),
                             };
        }

        public override void Execute(TriggerBase trigger)
        {
            var cmdStr = trigger.Get<string>("command");
            var subcmdStr = trigger.Get<string>("subcmd");

            if (cmdStr == string.Empty)
            {
                foreach (CommandBase command in CommandManager.Instance.AvailableCommands)
                {
                    if (command is SubCommand)
                        continue;

                    if (command.RequiredRole > trigger.UserRole)
                        continue;

                    DisplayCommandDescription(trigger, command);
                }
            }
            else
            {
                CommandBase command = CommandManager.Instance.GetCommand(cmdStr);

                if (command == null || command.RequiredRole > trigger.UserRole)
                {
                    trigger.Reply("Command '{0}' doesn't exist", cmdStr);
                    return;
                }

                if (subcmdStr == string.Empty)
                {
                    DisplayFullCommandDescription(trigger, command);
                }
                else
                {
                    if (!(command is SubCommandContainer))
                    {
                        trigger.Reply("Command '{0}' has no sub commands", cmdStr);
                        return;
                    }

                    SubCommand subcommand = (command as SubCommandContainer)[subcmdStr];

                    if (subcommand == null || subcommand.RequiredRole > trigger.UserRole)
                    {
                        trigger.Reply("Command '{0} {1}' doesn't exist", cmdStr, subcmdStr);
                        return;
                    }

                    DisplayFullSubCommandDescription(trigger, command, subcommand);
                }
            }
        }

        public static void DisplayCommandDescription(TriggerBase trigger, CommandBase command)
        {
            trigger.Reply(trigger.Bold("{0}") + "{1} - {2}",
                          string.Join("/", command.Aliases),
                          command is SubCommandContainer
                              ? string.Format(" ({0} subcmds)", ( command as SubCommandContainer ).Count(entry => entry.RequiredRole <= trigger.UserRole))
                              : "",
                          command.Description);
        }

        public static void DisplaySubCommandDescription(TriggerBase trigger, CommandBase command, SubCommand subcommand)
        {
            trigger.Reply(trigger.Bold("{0}") + " {1} - {2}",
                          command.Aliases.First(),
                          string.Join("/", subcommand.Aliases),
                          subcommand.Description);
        }


        public static void DisplayFullCommandDescription(TriggerBase trigger, CommandBase command)
        {
            trigger.Reply(trigger.Bold("{0}") + "{1} - {2}",
                          string.Join("/", command.Aliases),
                          command is SubCommandContainer && (command as SubCommandContainer).Count > 0
                              ? string.Format(" ({0} subcmds)", (command as SubCommandContainer).Count(entry => entry.RequiredRole <= trigger.UserRole))
                              : "",
                          command.Description);

            if (!(command is SubCommandContainer))
                trigger.Reply("  -> " + command.Aliases.First() + " " + command.GetSafeUsage());

            if (command.Parameters != null)
                foreach (IParameterDefinition commandParameter in command.Parameters)
                {
                    DisplayCommandParameter(trigger, commandParameter);
                }

            if (command is SubCommandContainer)
                foreach (SubCommand subCommand in command as SubCommandContainer)
                {
                    DisplayFullSubCommandDescription(trigger, command, subCommand);
                }
        }

        public static void DisplayFullSubCommandDescription(TriggerBase trigger, CommandBase command,
                                                             SubCommand subcommand)
        {
            trigger.Reply(trigger.Bold("{0} {1}") + " - {2}",
                          command.Aliases.First(),
                          string.Join("/", subcommand.Aliases),
                          subcommand.Description);
            trigger.Reply("  -> " + command.Aliases.First() + " " + subcommand.Aliases.First() + " " + subcommand.GetSafeUsage());

            foreach (IParameterDefinition commandParameter in subcommand.Parameters)
            {
                DisplayCommandParameter(trigger, commandParameter);
            }
        }

        public static void DisplayCommandParameter(TriggerBase trigger, IParameterDefinition parameter)
        {
            trigger.Reply("\t(" + trigger.Bold("{0}") + " : {1})",
                          parameter.GetUsage(),
                          parameter.Description ?? "");
        }
    }
}