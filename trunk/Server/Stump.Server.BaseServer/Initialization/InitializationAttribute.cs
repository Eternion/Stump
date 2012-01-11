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
        public InitializationAttribute(Type dependance)
        {
            Dependance = dependance;
        }

        public InitializationAttribute(InitializationPass pass)
        {
            Pass = pass;
        }

        public InitializationAttribute(InitializationPass pass, string desciption = "")
        {
            Description = desciption;
            Pass = pass;
        }

        public InitializationAttribute(Type dependantOf, string desciption = "")
        {
            Description = desciption;
            Dependance = dependantOf;
        }

        public Type Dependance
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public bool Silent
        {
            get;
            set;
        }

        public InitializationPass Pass
        {
            get;
            set;
        }
    }
}