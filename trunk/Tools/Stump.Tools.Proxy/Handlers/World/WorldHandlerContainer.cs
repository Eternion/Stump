
using System;
using System.Collections.Generic;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.World
{
    public abstract class WorldHandlerContainer : IHandlerContainer
    {
        public static Dictionary<uint, Predicate<WorldClient>> Predicates =
            new Dictionary<uint, Predicate<WorldClient>>();

        #region IHandlerContainer Members

        public bool CanHandleMessage(BaseClient client, uint messageId)
        {
            if (!Predicates.ContainsKey(messageId))
                return true;

            if (!(client is WorldClient))
                return false;

            return Predicates[messageId](client as WorldClient);
        }

        #endregion
    }
}