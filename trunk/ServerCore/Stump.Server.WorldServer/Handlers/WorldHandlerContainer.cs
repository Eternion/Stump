
using System;
using System.Collections.Generic;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.WorldServer.Handlers
{
    public abstract class WorldHandlerContainer : IHandlerContainer
    {
        public Dictionary<Type, Predicate<WorldClient>> Predicates =
            new Dictionary<Type, Predicate<WorldClient>>();

        #region IHandlerContainer Members

        public bool PredicateSuccess(BaseClient client, Type messageType)
        {
            if (!Predicates.ContainsKey(messageType))
                return true;

            if (!(client is WorldClient))
                return false;

            return Predicates[messageType](client as WorldClient);
        }

        #endregion
    }
}