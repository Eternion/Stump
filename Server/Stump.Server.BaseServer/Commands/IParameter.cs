namespace Stump.Server.BaseServer.Commands
{
    public interface IParameter
    {
        IParameterDefinition Definition
        {
            get;
        }

        object Value
        {
            get;
        }

        bool IsDefined
        {
            get;
        }

        void SetValue(string str, TriggerBase trigger);
        void SetDefaultValue(TriggerBase trigger);
    }
}