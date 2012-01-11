
using System;
using System.Collections.Generic;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.World
{
    public abstract class WorldHandlerContainer : IHandlerContainer
    {
        #region IHandlerContainer Members

        public bool CanHandleMessage(BaseClient client, uint messageId)
        {
            if (!(client is WorldClient))
                return false;

            return true;
        }

        #endregion
    }
}