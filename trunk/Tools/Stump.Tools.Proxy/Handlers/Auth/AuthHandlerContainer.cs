
using System;
using System.Collections.Generic;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.Auth
{
    public abstract class AuthHandlerContainer : IHandlerContainer
    {
        public static Dictionary<Type, Predicate<AuthClient>> Predicates = new Dictionary<Type, Predicate<AuthClient>>();

        public bool CanHandleMessage(BaseClient client, Type messageType)
        {
            if (!Predicates.ContainsKey(messageType))
                return true;

            if (!( client is AuthClient ))
                return false;

            return Predicates[messageType](client as AuthClient);
        }
    }
}