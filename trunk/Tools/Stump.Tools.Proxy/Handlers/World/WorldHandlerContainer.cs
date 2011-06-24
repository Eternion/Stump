
using System;
using System.Collections.Generic;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.World
{
    public abstract class WorldHandlerContainer : IHandlerContainer
    {
        public static Dictionary<Type, Predicate<WorldClient>> Predicates =
            new Dictionary<Type, Predicate<WorldClient>>();

        #region IHandlerContainer Members

        public bool CanHandleMessage(BaseClient client, Type messageType)
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