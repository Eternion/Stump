
using System.Collections.Generic;
using System.Linq;
using Stump.BaseCore.Framework.Attributes;

namespace Stump.Server.BaseServer.Commands
{
    public abstract class CommandBase : ExecuterBase
    {
        /// <summary>
        /// Enable/Disable case check for server's commands
        /// </summary>
        [Variable]
        public static bool IgnoreCommandCase = true;

        protected CommandBase()
        {
            SubCommands = new List<SubCommand>();
        }

        public List<SubCommand> SubCommands
        {
            get;
            internal set;
        }

        /// <summary>
        ///   Try to get a SubCommand with its name.
        /// </summary>
        /// <param name = "subcmd">requested subcommand name</param>
        /// <param name = "result">Out the requested subcommand</param>
        /// <returns>if the requested subcommand exists.</returns>
        public bool TryGetSubCommand(string subcmd, out SubCommand result)
        {
            foreach (SubCommand sub in from sub in SubCommands
                                       from subalias in sub.Aliases
                                       where subalias == subcmd
                                       select sub)
            {
                result = sub;
                return true;
            }

            result = null;
            return false;
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}