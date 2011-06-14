
using System;

namespace Stump.Server.BaseServer.Handler
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public abstract class Handler : Attribute
    {
        protected Handler(Type message)
        {
            Message = message;
        }

        public Type Message
        {
            get; private set;
        }

        public override string ToString()
        {
            return Message.ToString();
        }
    }
}