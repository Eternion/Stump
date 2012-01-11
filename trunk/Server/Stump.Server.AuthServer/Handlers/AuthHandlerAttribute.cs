
using System;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.AuthServer.Handlers
{
    public class AuthHandlerAttribute : HandlerAttribute
    {
        public AuthHandlerAttribute(uint messageId)
            : base(messageId)
        {
        }
    }
}