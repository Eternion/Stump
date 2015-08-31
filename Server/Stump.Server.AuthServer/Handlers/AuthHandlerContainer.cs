
using System;
using System.Collections.Generic;
using Stump.Server.AuthServer.Network;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.AuthServer.Handlers
{
    public abstract class AuthHandlerContainer : IHandlerContainer
    {
        private static readonly Dictionary<uint, Predicate<AuthClient>> Predicates = new Dictionary<uint, Predicate<AuthClient>>();

        protected void Predicate(uint messageId, Predicate<AuthClient> predicate)
        {
            Predicates.Add(messageId, predicate);
        }

        public bool CanHandleMessage(IClient client, uint messageId)
        {
            if (!Predicates.ContainsKey(messageId))
                return true;

            if (!( client is AuthClient ))
                return false;

            return Predicates[messageId](client as AuthClient);
        }
    }
}