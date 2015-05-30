using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stump.Server.BaseServer.Commands.Commands;

namespace Stump.Server.BaseServer.Commands
{
    /// <summary>
    /// Represents a command that contains only SubCommands
    /// </summary>
    public abstract class SubCommandContainer : CommandBase, IEnumerable<SubCommand>
    {
        private readonly List<SubCommand> m_subCommands = new List<SubCommand>();


        /// <summary>
        /// Gets the subcommand by his name or returns null if not found
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public SubCommand this[string name]
        {
            get
            {
                SubCommand command;
                return !TryGetSubCommand(name, out command) ? null : command;
            }
        }

        public int Count
        {
            get
            {
                return m_subCommands.Count;
            }
        }

        #region IEnumerable<SubCommand> Members

        public IEnumerator<SubCommand> GetEnumerator()
        {
            return m_subCommands.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public override void Execute(TriggerBase trigger)
        {
            var str = trigger.Args.NextWord();

            SubCommand command;
            if (!TryGetSubCommand(IgnoreCommandCase ? str.ToLower() : str, out command) || !trigger.CanAccessCommand(command))
            {
                HelpCommand.DisplayFullCommandDescription(trigger, this);
                return;
            }

            if (trigger.BindToCommand(command))
                command.Execute(trigger);
        }

        public void AddSubCommand(SubCommand subCommand)
        {
            m_subCommands.Add(subCommand);
        }

        public void RemoveSubCommand(SubCommand subCommand)
        {
            m_subCommands.Remove(subCommand);
        }

        public void SortSubCommands()
        {
            m_subCommands.Sort((subcmd1, subcmd2) => subcmd1.Aliases.First().CompareTo(subcmd2.Aliases.First()));
        }

        /// <summary>
        ///   Try to get a SubCommand with its name.
        /// </summary>
        /// <param name = "subcmd">Requested subcommand name</param>
        /// <param name = "result">Out the requested subcommand</param>
        /// <returns>Returns true if the requested subcommand exists.</returns>
        public bool TryGetSubCommand(string subcmd, out SubCommand result)
        {
            var query = from sub in m_subCommands
                        from subalias in sub.Aliases
                        where subalias.Equals(subcmd, IgnoreCommandCase ?
                             StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture)
                        select sub;

            result = query.SingleOrDefault();

            return result != null;
        }
    }
}