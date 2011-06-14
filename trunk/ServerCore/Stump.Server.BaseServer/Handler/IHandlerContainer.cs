
using System;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.BaseServer.Handler
{
    public interface IHandlerContainer
    {
        bool PredicateSuccess(BaseClient client, Type messageType);
    }
}