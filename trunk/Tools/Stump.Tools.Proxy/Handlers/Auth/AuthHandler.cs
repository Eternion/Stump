
using System;
using Stump.Server.BaseServer.Handler;

namespace Stump.Tools.Proxy.Handlers.Auth
{
    public class AuthHandler : Handler
    {
        public AuthHandler(Type message)
            : base(message)
        {
        }
    }
}