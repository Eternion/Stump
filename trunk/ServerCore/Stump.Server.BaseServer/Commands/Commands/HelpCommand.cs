using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.Commands.Commands
{
    public class HelpCommand : CommandBase
    {
        public HelpCommand()
        {
            Aliases = new [] { "help", "?" };
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
                foreach (var command in CommandManager.Instance.AvailableCommands)
                {
                    DisplayCommandDescription(trigger, command);

                    if (command is SubCommandContainer)
                        foreach (var subcommand in (command as SubCommandContainer))
                        {
                            DisplaySubCommandDescription(trigger, command, subcommand);
                        }
                }
            }
            else
            {
                var command = CommandManager.Instance.GetCommand(cmdStr);

                if (command == null)
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
                    if (!( command is SubCommandContainer ))
                    {
                        trigger.Reply("Command '{0}' has no sub commands", cmdStr);
                        return;
                    }

                    var subcommand = ( command as SubCommandContainer )[subcmdStr];

                    if (subcommand == null)
                    {
                        trigger.Reply("Command '{0} {1}' doesn't exist", cmdStr, subcmdStr);
                        return;
                    }

                    DisplayFullSubCommandDescription(trigger, command, subcommand);
                }
            }
        }

        private static void DisplayCommandDescription(TriggerBase trigger, CommandBase command)
        {
            trigger.Reply("{0}{1} - {2}",
                          string.Join("/", command.Aliases),
                          command is SubCommandContainer
                              ? string.Format(" ({0} subcmds)", ( command as SubCommandContainer ).Count)
                              : "",
                          command.Description);
        }

        private static void DisplaySubCommandDescription(TriggerBase trigger, CommandBase command, SubCommand subcommand)
        {
            trigger.Reply("{0} {1} - {2}",
                          command.Aliases.First(),
                          string.Join("/", subcommand.Aliases),
                          subcommand.Description);
        }


        private static void DisplayFullCommandDescription(TriggerBase trigger, CommandBase command)
        {
            trigger.Reply("{0}{1} - {2} : {3}",
                          string.Join("/", command.Aliases),
                          command is SubCommandContainer
                              ? string.Format(" ({0} subcmds)", ( command as SubCommandContainer ).Count)
                              : "",
                          command.Description,
                          !( command is SubCommandContainer )
                              ? command.Aliases.First() + " " + command.GetSafeUsage()
                              : "");

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

        private static void DisplayFullSubCommandDescription(TriggerBase trigger, CommandBase command,
                                                     SubCommand subcommand)
        {
            trigger.Reply("{0} {1} - {2} : {0} {1} {3}",
                          command.Aliases.First(),
                          string.Join("/", subcommand.Aliases),
                          subcommand.Description,
                          subcommand.GetSafeUsage());

            foreach (IParameterDefinition commandParameter in subcommand.Parameters)
            {
                DisplayCommandParameter(trigger, commandParameter);
            }
        }

        private static void DisplayCommandParameter(TriggerBase trigger, IParameterDefinition parameter)
        {
            trigger.Reply("\t({0} : {1})",
                          parameter.GetUsage(),
                          parameter.Description ?? "");
        }
    }
}