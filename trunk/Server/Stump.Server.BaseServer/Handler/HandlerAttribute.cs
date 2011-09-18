
using System;

namespace Stump.Server.BaseServer.Handler
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public abstract class HandlerAttribute : Attribute
    {
        protected HandlerAttribute(uint messageId)
        {
            MessageId = messageId;
        }

        protected HandlerAttribute(uint messageId, bool handledByIOTask)
        {
            MessageId = messageId;
            HandledByIOTask = handledByIOTask;
        }

        public bool HandledByIOTask
        {
            get;
            private set;
        }

        public uint MessageId
        {
            get; 
            private set;
        }

        public override string ToString()
        {
            return MessageId.ToString();
        }
    }
}