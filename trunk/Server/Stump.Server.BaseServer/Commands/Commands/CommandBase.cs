
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.Commands
{
    public abstract class CommandBase
    {
        /// <summary>
        /// Enable/Disable case check for server's commands
        /// </summary>
        [Variable]
        public static bool IgnoreCommandCase = true;

        public string[] Aliases
        {
            get;
            protected set;
        }

        public string Usage
        {
            get;
            protected set;
        }

        public string Description
        {
            get;
            protected set;
        }

        public RoleEnum RequiredRole
        {
            get;
            protected set;
        }

        public List<IParameterDefinition> Parameters
        {
            get;
            protected set;
        }

        public string GetSafeUsage()
        {
            if (string.IsNullOrEmpty(Usage))
            {
                if (Parameters == null)
                    return "";

                return string.Join(" ", from entry in Parameters
                                        select entry.GetUsage());
            }

            return Usage;
        }

        public abstract void Execute(TriggerBase trigger);

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}