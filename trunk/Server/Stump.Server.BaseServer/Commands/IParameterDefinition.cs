using System;

namespace Stump.Server.BaseServer.Commands
{
    public interface IParameterDefinition
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

        Type ValueType
        {
            get;
        }

        /// <summary>
        /// Parse the string value and convert it to the right type.
        /// If value equals null or an empty string, it returns the default value
        /// </summary>
        object ConvertString(string value, TriggerBase trigger);
        IParameter CreateParameter();
        string GetUsage();
    }

    public interface IParameterDefinition<out T> : IParameterDefinition
    {
        ConverterHandler<T> Converter
        {
            get;
        }

         T DefaultValue
        {
            get;
        }
    }
}