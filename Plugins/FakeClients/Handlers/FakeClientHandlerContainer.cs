using Stump.Server.BaseServer.Handler;
using Stump.Server.BaseServer.Network;

namespace FakeClients.Handlers
{
    public class FakeClientHandlerContainer : IHandlerContainer
    {
        public bool CanHandleMessage(IClient client, uint messageId)
        {
            return client is FakeClient;
        }
    }
}