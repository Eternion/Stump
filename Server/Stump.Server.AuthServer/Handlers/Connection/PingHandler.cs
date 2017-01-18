using Stump.DofusProtocol.Messages;
using Stump.Server.AuthServer.Network;

namespace Stump.Server.AuthServer.Handlers.Connection
{
    public class PingHandler : AuthHandlerContainer
    {
        [AuthHandler(BasicPingMessage.Id)]
        public static void HandleBasicPingMessage(AuthClient client, BasicPingMessage message)
        {
            client.Send(new BasicPongMessage());
        }
    }
}