
using System;

namespace Stump.Server.BaseServer.Commands
{
    public interface ICommandParameter : ICloneable
    {
        string Name
        {
            get;
        }

        string ShortName
        {
            get;
        }

        string Description
        {
            get;
        }

        bool IsOptional
        {
            get;
        }

        bool IsValueDefined
        {
            get;
        }

        object Value
        {
            get;
        }

        object DefaultValue
        {
            get;
        }

        Type ValueType
        {
            get;
        }

        string StringValue
        {
            get;
        }
        void SetStringValue(string str, TriggerBase trigger);
        bool SetDefaultValue();
        string GetUsage();
    }

    public interface ICommandParameter<out T> : ICommandParameter
    {
        Func<string, TriggerBase, T> Converter
        {
            get;
        }

        new T DefaultValue
        {
            get;
        }

        new T Value
        {
            get;
        }

        T GetValue();
    }
}