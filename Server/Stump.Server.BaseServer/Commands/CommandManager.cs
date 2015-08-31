using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NLog;
using Stump.Core.Reflection;
using Stump.Core.Xml;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.Commands
{
    public class CommandManager : Singleton<CommandManager>
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private IDictionary<string, CommandBase> m_commandsByAlias;
        private readonly List<CommandBase> m_registeredCommands;
        private readonly List<Type> m_registeredTypes;
        private readonly List<CommandInfo> m_commandsInfos; 

        private CommandManager()
        {
            m_commandsByAlias = new Dictionary<string, CommandBase>();
            m_registeredCommands = new List<CommandBase>();
            m_registeredTypes = new List<Type>();
            m_commandsInfos = new List<CommandInfo>();
        }

        /// <summary>
        /// Regroup all CommandBase and SubCommandContainer by alias
        /// </summary>
        public IDictionary<string, CommandBase> CommandsByAlias
        {
            get { return m_commandsByAlias; }
        }

        /// <summary>
        /// Regroup all CommandBases, SubCommandContainers and SubCommands
        /// </summary>
        public IReadOnlyCollection<CommandBase> AvailableCommands
        {
            get
            {
                return m_registeredCommands.AsReadOnly();
            }
        }

        #region Get Method

        public CommandBase GetCommand(string alias)
        {
            CommandBase command;
            m_commandsByAlias.TryGetValue(alias, out command);

            return command;
        }

        public CommandBase this[string alias]
        {
            get { return GetCommand(alias); }
        }

        #endregion

        #region Register Methods

        public void RegisterAll(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            var callTypes = assembly.GetTypes().Where(entry => !entry.IsAbstract);

            foreach (var type in callTypes.Where(type => !IsCommandRegister(type)))
            {
                RegisterCommand(type);
            }

            SortCommands();
        }

        public void LoadOrCreateCommandsInfo(string xmlFile)
        {
            if (!File.Exists(xmlFile))
            {
                XmlUtils.Serialize(xmlFile, m_commandsInfos);
            }
            else
            {
                var infos = XmlUtils.Deserialize<CommandInfo[]>(xmlFile);
                LoadCommandsInfo(infos);
            }
        }

        public void LoadCommandsInfo(CommandInfo[] infos)
        {
            foreach (var info in infos)
            {
                if (m_commandsInfos.RemoveAll(x => x.Name == info.Name) > 0)
                    m_commandsInfos.Add(info);

                var command = AvailableCommands.FirstOrDefault(x => x.GetType().Name == info.Name);
                if (command != null)
                    info.ModifyCommandInfo(command);
            }
        }

        public void RegisterCommand(Type commandType)
        {
            if (IsCommandRegister(commandType))
                return;

            if (commandType.IsSubclassOf(typeof(SubCommand)))
            {
                RegisterSubCommand(commandType);
            }
            else if (commandType.IsSubclassOf(typeof(SubCommandContainer)))
            {
                RegisterSubCommandContainer(commandType);
            }
            else if (commandType.IsSubclassOf(typeof(CommandBase)))
            {
                RegisterCommandBase(commandType);
            }
        }

        private void RegisterCommandBase(Type commandType)
        {
            var command = Activator.CreateInstance(commandType) as CommandBase;

            if (command == null)
                throw new Exception(string.Format("Cannot create a new instance of {0}", commandType));

            if (command.Aliases == null)
            {
                logger.Error("Cannot register Command {0}, Aliases is null", commandType.Name);
                return;
            }

            if (command.RequiredRole == RoleEnum.None)
            {
                logger.Error(
                    "Cannot register Command : {0}. RequiredRole is incorrect", commandType.Name);
                return;
            }

            m_registeredCommands.Add(command);
            m_commandsInfos.Add(new CommandInfo(command));
            m_registeredTypes.Add(commandType);

            foreach (var alias in command.Aliases)
            {
                CommandBase value;
                if (!m_commandsByAlias.TryGetValue(alias, out value))
                {
                    m_commandsByAlias[CommandBase.IgnoreCommandCase ? alias.ToLower() : alias] = command;
                }
                else
                {
                    logger.Error("Found two Commands with Alias \"{0}\": {1} and {2}", alias, value, command);
                }
            }
        }

        private void RegisterSubCommandContainer(Type commandType)
        {
            var command = Activator.CreateInstance(commandType) as SubCommandContainer;

            if (command == null)
                throw new Exception(string.Format("Cannot create a new instance of {0}", commandType));

            if (command.Aliases == null)
            {
                logger.Error("Cannot register Command {0}, Aliases is null", commandType.Name);
                return;
            }

            if (command.RequiredRole == RoleEnum.None)
            {
                logger.Error(
                    "Cannot register Command : {0}. RequiredRole is incorrect", commandType.Name);
                return;
            }

            m_registeredCommands.Add(command);
            m_commandsInfos.Add(new CommandInfo(command));
            m_registeredTypes.Add(commandType);
            foreach (var alias in command.Aliases)
            {
                CommandBase value;
                if (!m_commandsByAlias.TryGetValue(alias, out value))
                {
                    m_commandsByAlias[CommandBase.IgnoreCommandCase ? alias.ToLower() : alias] = command;

                }
                else
                {
                    logger.Error("Found two Commands with Alias \"{0}\": {1} and {2}", alias, value, command);
                }
            }
        }

        private void RegisterSubCommand(Type commandType)
        {
            var subcommand = Activator.CreateInstance(commandType) as SubCommand;

            if (subcommand == null)
                throw new Exception(string.Format("Cannot create a new instance of {0}", commandType));

            if (subcommand.Aliases == null)
            {
                logger.Error("Cannot register subcommand {0}, Aliases is null", commandType.Name);
                return;
            }

            if (subcommand.RequiredRole == RoleEnum.None)
            {
                logger.Error(
                    "Cannot register subcommand : {0}. RequiredRole is incorrect", commandType.Name);
                return;
            }

            if (subcommand.ParentCommandType == null)
            {
                logger.Error("The subcommand {0} has no parent command and cannot be registered", commandType);
                return;
            }

            if (!IsCommandRegister(subcommand.ParentCommandType))
                RegisterCommand(subcommand.ParentCommandType);

            var parentCommand = AvailableCommands.SingleOrDefault(entry => entry.GetType() == subcommand.ParentCommandType) as SubCommandContainer;

            if (parentCommand == null)
                throw new Exception(string.Format("Cannot found declaration of command '{0}'", subcommand.ParentCommandType));

            parentCommand.AddSubCommand(subcommand);
            subcommand.SetParentCommand(parentCommand);
            m_registeredCommands.Add(subcommand);
            m_commandsInfos.Add(new CommandInfo(subcommand));
            m_registeredTypes.Add(commandType);
        }

        public void UnRegisterAll(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            var callTypes = assembly.GetTypes().Where(entry => !entry.IsAbstract);

            foreach (var type in callTypes.Where(IsCommandRegister))
            {
                UnRegisterCommand(type);
            }
        }

        public void UnRegisterCommand(Type commandType)
        {
            var command = Activator.CreateInstance(commandType) as CommandBase;

            if (command == null)
                return;

            foreach (var alias in command.Aliases)
            {
                m_commandsByAlias.Remove(alias);
                m_commandsInfos.RemoveAll(x => x.Name == alias);
            }

            m_registeredTypes.Remove(commandType);
            m_registeredCommands.RemoveAll(entry => entry.GetType() == commandType);
        }

        private void SortCommands()
        {
            m_commandsByAlias = m_commandsByAlias.OrderBy(entry => entry.Key).ToDictionary(entry => entry.Key,
                                                                               entry => entry.Value);

            foreach (var availableCommand in AvailableCommands.OfType<SubCommandContainer>())
            {
                availableCommand.SortSubCommands();
            }
        }

        public bool IsCommandRegister(Type commandType)
        {
            return m_registeredTypes.Contains(commandType);
        }

        #endregion

        #region Handle Method

        public void HandleCommand(TriggerBase trigger)
        {
            var cmdstring = trigger.Args.NextWord();

            if (CommandBase.IgnoreCommandCase)
                cmdstring = cmdstring.ToLower();

            var cmd = this[cmdstring];

            if (cmd != null && trigger.CanAccessCommand(cmd))
            {
                try
                {
                    if (trigger.BindToCommand(cmd))
                        cmd.Execute(trigger);

                    //Log command
                    trigger.Log();
                }
                catch (Exception ex)
                {
                    trigger.ReplyError("Raised exception (error-index:{0}) : {1}", trigger.RegisterException(ex), ex.Message);

                    if (ex.InnerException != null)
                        trigger.ReplyError(" => " + ex.InnerException.Message);
                }
            }
            else
            {
                trigger.ReplyError("Incorrect Command \"{0}\". Type commandslist or help for command list.", cmdstring);
            }
        }

        #endregion
    }
}