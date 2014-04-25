using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.Commands
{
    public abstract class CommandBase
    {
        protected CommandBase()
        {
            Parameters = new List<IParameterDefinition>();
        }

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
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public RoleEnum RequiredRole
        {
            get;
            set;
        }

        public List<IParameterDefinition> Parameters
        {
            get;
            protected set;
        }

        public void AddParameter<T>(string name, string shortName = "", string description = "", T defaultValue = default(T), bool isOptional = false,
                                ConverterHandler<T> converter = null)
        {
            Parameters.Add(new ParameterDefinition<T>(name, shortName, description, defaultValue, isOptional, converter));
        }

        public string GetSafeUsage()
        {
            if (!string.IsNullOrEmpty(Usage)) return Usage;
            if (Parameters == null)
                return "";

            return string.Join(" ", from entry in Parameters
                select entry.GetUsage());
        }

        public virtual string[] GetFullAliases()
        {
            return Aliases;
        }

        public abstract void Execute(TriggerBase trigger);

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}