
using System;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.AuthServer.Handlers
{
    public class AuthHandler : Handler
    {
        public AuthHandler(Type message)
            : base(message)
        {
        }
    }
}