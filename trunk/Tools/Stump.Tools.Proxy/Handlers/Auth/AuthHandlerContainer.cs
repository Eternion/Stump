
using System;
using System.Collections.Generic;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.Auth
{
    public abstract class AuthHandlerContainer : IHandlerContainer
    {
        public static Dictionary<uint, Predicate<AuthClient>> Predicates = new Dictionary<uint, Predicate<AuthClient>>();

        public bool CanHandleMessage(BaseClient client, uint messageId)
        {
            if (!( client is AuthClient ))
                return false;

            if (!Predicates.ContainsKey(messageId))
                return true;

            return Predicates[messageId](client as AuthClient);
        }
    }
}