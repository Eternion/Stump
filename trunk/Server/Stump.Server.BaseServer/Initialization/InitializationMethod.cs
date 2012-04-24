using System;
using System.Reflection;

namespace Stump.Server.BaseServer.Initialization
{
    public class InitializationMethod
    {
        public InitializationMethod(InitializationAttribute attribute, MethodInfo method)
        {
            Attribute = attribute;
            Method = method;
        }

        public InitializationAttribute Attribute
        {
            get;
            private set;
        }

        public MethodInfo Method
        {
            get;
            private set;
        }

        public object Caller
        {
            get;
            set;
        }

        public bool Initialized
        {
            get;
            set;
        }
    }
}