
using System;
using Stump.Server.BaseServer.Handler;

namespace Stump.Tools.Proxy.Handlers.Auth
{
    public class AuthHandlerAttribute : HandlerAttribute
    {
        public AuthHandlerAttribute(uint messageId)
            : base(messageId)
        {
        }
    }
}