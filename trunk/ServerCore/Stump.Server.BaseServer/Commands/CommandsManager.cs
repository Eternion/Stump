// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.Commands
{
    public class CommandsManager
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private IDictionary<string, CommandBase> m_commandsByAlias;

        public CommandsManager()
        {
            m_commandsByAlias = new Dictionary<string, CommandBase>();
        }

        public IEnumerable<CommandBase> CommandsByAlias
        {
            get { return m_commandsByAlias.Values; }
        }

        public List<CommandBase> AvailableCommands
        {
            get { return m_commandsByAlias.Values.Distinct().ToList(); }
        }

        public CommandBase this[string alias]
        {
            get { return GetCommand(alias); }
        }

        public void RegisterAll<T, TC>()
        {
            Assembly asm = typeof (T).Assembly;
            Type[] callTypes;

            if (asm != null)
                callTypes = asm.GetTypes();
            else
                return;

            foreach (Type type in callTypes)
            {
                RegisterCommand<T>(type);
            }

            m_commandsByAlias = m_commandsByAlias.OrderBy(entry => entry.Key).ToDictionary(entry => entry.Key,
                                                                                           entry => entry.Value);

            foreach (Type type in callTypes)
            {
                RegisterSubCommand<TC>(type);
            }


            foreach (CommandBase command in m_commandsByAlias.Values)
            {
                command.SubCommands = command.SubCommands.OrderBy(entry => entry.Aliases.First()).ToList();
            }
        }

        public void HandleCommand(TriggerBase trigger)
        {
            string cmdstring = trigger.Args.NextWord();

            if (CommandBase.IgnoreCommandCase)
                cmdstring = cmdstring.ToLower();

            try
            {
                if (cmdstring == "help" || cmdstring == "?")
                {
                    HandleHelpCommand(trigger);
                    return;
                }
                else if (cmdstring == "commandslist")
                {
                    HandleListCommand(trigger);
                    return;
                }
            }
            catch (Exception ex)
            {
                trigger.Reply("Raised exception when executing command : " + ex.Message);
            }


            CommandBase cmd = this[cmdstring];

            if (cmd != null && trigger.UserRole >= cmd.RequiredRole)
            {
                trigger.BindedCommand = cmd;

                if (cmd.SubCommands.Count > 0)
                {
                    // Well a command containing subcmd is considered as 'dummy', meaning there is nothing to do.
                    // So we want to go and find the given subcommand.

                    string subcmdstring = trigger.Args.NextWord();
                    SubCommand subcmd;
                    if (cmd.TryGetSubCommand(subcmdstring, out subcmd))
                    {
                        if (trigger.UserRole >= subcmd.RequiredRole)
                        {
                            try
                            {
                                trigger.BindedSubCommand = subcmd;

                                if (trigger.DefineParameters(subcmd.Parameters))
                                    subcmd.Execute(trigger);
                            }
                            catch (Exception ex)
                            {
                                trigger.Reply("Raised exception when executing command : " + ex.Message);
                            }
                        }
                        else
                            trigger.Reply("Incorrect SubCommand \"{0}\". Type help for command list.", subcmdstring);
                    }
                    else
                    {
                        // User want to use only cmd even if there are subcmds.
                        try
                        {
                            trigger.BindedSubCommand = subcmd;

                            if (trigger.DefineParameters(cmd.Parameters))
                                cmd.Execute(trigger);
                        }
                        catch (Exception ex)
                        {
                            trigger.Reply("Raised exception when executing command : " + ex.Message);
                        }
                    }
                }
                else
                {
                    try
                    {
                        if (trigger.DefineParameters(cmd.Parameters))
                            cmd.Execute(trigger);
                    }
                    catch (Exception ex)
                    {
                        trigger.Reply("Raised exception when executing command : " + ex.Message);
                    }
                }
            }
            else
            {
                trigger.Reply("Incorrect Command \"{0}\". Type help or ? for command list.", cmdstring);
            }
        }

        public void HandleHelpCommand(TriggerBase trigger)
        {
            string commandStr = trigger.Args.NextWord();

            if (commandStr == "")
            {
                DisplayAllCommands(trigger);
            }
            else
            {
                CommandBase command = GetCommand(commandStr);

                if (command == null)
                {
                    trigger.Reply("Command \"{0}\" doesn't exist", commandStr);
                    return;
                }

                string subCommandStr = trigger.Args.NextWord();

                if (subCommandStr == "")
                {
                    DisplayCompleteCommandDescription(trigger, command);
                }
                else
                {
                    SubCommand subCommand;
                    command.TryGetSubCommand(subCommandStr, out subCommand);

                    if (subCommand == null)
                    {
                        trigger.Reply("SubCommand \"{0} {1}\" doesn't exist", commandStr, subCommandStr);
                        return;
                    }

                    DisplayFullSubCommandDescription(trigger, command, subCommand);
                }
            }
        }

        public void HandleListCommand(TriggerBase trigger)
        {
            DisplayCommandsList(trigger);
        }

        private void DisplayCommandsList(TriggerBase trigger)
        {
            trigger.Reply(string.Join(", ", from entry in AvailableCommands
                                            select entry.Aliases.First()));
        }

        private void DisplayAllCommands(TriggerBase trigger)
        {
            foreach (CommandBase command in AvailableCommands)
            {
                DisplayCommandDescription(trigger, command);

                foreach (SubCommand subCommand in command.SubCommands)
                {
                    DisplaySubCommandDescription(trigger, command, subCommand);
                }
            }
        }

        private static void DisplayCommandDescription(TriggerBase trigger, CommandBase command)
        {
            trigger.Reply("{0} {1} - {2}",
                          string.Join("/", command.Aliases),
                          command.SubCommands.Count > 0
                              ? string.Format("({0} subcmds)", command.SubCommands.Count)
                              : "",
                          command.Description,
                          command.Aliases.First());
        }

        private static void DisplayFullCommandDescription(TriggerBase trigger, CommandBase command)
        {
            trigger.Reply("{0} {1} - {2} : {3}",
                          string.Join("/", command.Aliases),
                          command.SubCommands.Count > 0
                              ? string.Format("({0} subcmds)", command.SubCommands.Count)
                              : "",
                          command.Description,
                          command.SubCommands.Count == 0
                              ? command.Aliases.First() + " " + command.GetSafeUsage()
                              : "");

            if (command.Parameters != null)
                foreach (ICommandParameter commandParameter in command.Parameters)
                {
                    DisplayCommandParameter(trigger, commandParameter);
                }
        }

        private static void DisplaySubCommandDescription(TriggerBase trigger, CommandBase command, SubCommand subcommand)
        {
            trigger.Reply("{0} {1} - {2}",
                          command.Aliases.First(),
                          string.Join("/", subcommand.Aliases),
                          subcommand.Description);
        }

        private static void DisplayFullSubCommandDescription(TriggerBase trigger, CommandBase command,
                                                             SubCommand subcommand)
        {
            trigger.Reply("{0} {1} - {2} : {0} {1} {3}",
                          command.Aliases.First(),
                          string.Join("/", subcommand.Aliases),
                          subcommand.Description,
                          subcommand.GetSafeUsage());

            foreach (ICommandParameter commandParameter in subcommand.Parameters)
            {
                DisplayCommandParameter(trigger, commandParameter);
            }
        }

        private static void DisplayCommandParameter(TriggerBase trigger, ICommandParameter commandParameter)
        {
            trigger.Reply("\t({0} : {1})",
                          commandParameter.GetUsage(),
                          commandParameter.Description ?? "");
        }

        private static void DisplayCompleteCommandDescription(TriggerBase trigger, CommandBase command)
        {
            DisplayFullCommandDescription(trigger, command);

            foreach (SubCommand subCommand in command.SubCommands)
            {
                DisplayFullSubCommandDescription(trigger, command, subCommand);
            }
        }

        private void RegisterCommand<T>(Type commandType)
        {
            if (commandType.IsSubclassOf(typeof (T)))
            {
                var command = (T) Activator.CreateInstance(commandType) as CommandBase;

                if (command != null)
                {
                    if (command.Aliases == null || command.RequiredRole == RoleEnum.None)
                    {
                        logger.Error(
                            "An error occurred while registering Command : {0}. Either aliases are null or RequiredRole is incorrect.\nPlease check and repair.", commandType.Name);
                        return;
                    }

                    // Add to table, mapped by aliases
                    foreach (string alias in command.Aliases)
                    {
                        CommandBase exCommand;
                        if (!m_commandsByAlias.TryGetValue(alias, out exCommand))
                        {
                            m_commandsByAlias[alias] = command;
                        }
                        else
                        {
                            logger.Error("Found two Commands with Alias \"{0}\": {1} and {2}", alias, exCommand, command);
                        }
                    }
                }
            }
        }

        private void RegisterSubCommand<TC>(Type subcommandType)
        {
            if (subcommandType.IsSubclassOf(typeof (TC)))
            {
                var subcommand = (TC) Activator.CreateInstance(subcommandType) as SubCommand;

                if (subcommand != null)
                {
                    if (subcommand.Aliases == null || subcommand.RequiredRole == RoleEnum.None)
                    {
                        logger.Error(
                            "An error occurred while registering SubCommand : {0}. Either aliases are null or RequiredRole is incorrect.\nPlease check and repair.");
                        return;
                    }

                    CommandBase parent =
                        m_commandsByAlias.Values.Where(o => o.GetType() == subcommand.ParentCommand).FirstOrDefault();


                    if (parent == null)
                    {
                        logger.Error(
                            "Couldn't find Command when registering SubCommand \"{0}\".\nCheck if ParentCommand was correctly filled out.",
                            subcommand);
                        return;
                    }

                    parent.SubCommands.Add(subcommand);
                }
            }
        }

        public CommandBase GetCommand(string alias)
        {
            CommandBase command;
            m_commandsByAlias.TryGetValue(alias, out command);

            return command;
        }
    }
}