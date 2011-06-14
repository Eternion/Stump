
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.Commands
{
    /// <summary>
    ///   Only used to make SubCommand and CommandBase shorter
    /// </summary>
    public abstract class ExecuterBase
    {
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

        public IEnumerable<ICommandParameter> Parameters
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
                                        select
                                            entry.GetUsage());
            }
            else return Usage;
        }

        public abstract void Execute(TriggerBase trigger);
    }
}