
using System;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.BaseServer.Handler
{
    public interface IHandlerContainer
    {
        bool CanHandleMessage(BaseClient client, uint messageId);
    }
}