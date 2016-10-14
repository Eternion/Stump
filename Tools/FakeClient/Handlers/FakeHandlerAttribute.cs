using System;
using Stump.Server.BaseServer.Handler;

namespace FakeClient.Handlers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class FakeHandlerAttribute : HandlerAttribute
    {
        public FakeHandlerAttribute(uint messageId)
            : base(messageId)
        {
        }
    }
}