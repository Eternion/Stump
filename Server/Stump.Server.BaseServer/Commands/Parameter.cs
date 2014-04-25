using System;

namespace Stump.Server.BaseServer.Commands
{
    public class Parameter<T> : IParameter
    {
        public Parameter(IParameterDefinition<T> definition)
        {
            Definition = definition;
        }

        IParameterDefinition IParameter.Definition
        {
            get
            {
                return Definition;
            }
        }

        public IParameterDefinition<T> Definition
        {
            get;
            private set;
        }

        object IParameter.Value
        {
            get
            {
                return Value;
            }
        }

        public T Value
        {
            get;
            private set;
        }

        public bool IsDefined
        {
            get;
            private set;
        }

        public void SetValue(string str, TriggerBase trigger)
        {
            Value = (T)Definition.ConvertString(str, trigger);

            IsDefined = true;
        }

        public void SetDefaultValue(TriggerBase trigger)
        {
            Value = (T)Definition.ConvertString(string.Empty, trigger);

            IsDefined = false;
        }
    }
}