using System;
using System.Collections.Generic;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers
{
    public abstract class WorldHandlerContainer : IHandlerContainer
    {
        protected Dictionary<uint, Predicate<WorldClient>> Predicates =
            new Dictionary<uint, Predicate<WorldClient>>();

        #region IHandlerContainer Members

        protected void Predicate(uint messageId, Predicate<WorldClient> predicate)
        {
            Predicates.Add(messageId, predicate);
        }


        public bool CanHandleMessage(IClient client, uint messageId)
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