
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

        protected HandlerAttribute()
        {
            throw new NotImplementedException();
        }

        public uint MessageId
        {
            get; 
            set;
        }

        public override string ToString()
        {
            return MessageId.ToString();
        }
    }
}