using System;

namespace Stump.Server.BaseServer.Initialization
{
    /// <summary>
    /// Define a initialization method, called on server start.
    /// The method is called when the initialization pass is executed or
    /// when the type, whose method is dependant, is initialized
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class InitializationAttribute : Attribute
    {
        public InitializationAttribute(string name, InitializationPass pass)
        {
            Name = name;
            Pass = pass;
        }

        public InitializationAttribute(string name, Type dependantOf)
        {
            Name = name;
            Dependance = dependantOf;
        }

        public Type Dependance
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public InitializationPass Pass
        {
            get;
            private set;
        }
    }
}