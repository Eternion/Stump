using System;
using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers
{
    public abstract class ProxyHandlerContainer : IHandlerContainer
    {
        public bool CanHandleMessage(BaseClient client, uint messageId)
        {
            if (!( client is ProxyClient ))
                return false;

            return true;
        }
    }
}