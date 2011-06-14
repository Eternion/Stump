
using Stump.DofusProtocol.Messages;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.World
{
    public class HelloGameMessageHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (HelloGameMessage))]
        public static void HandleHelloGameMessage(WorldClient client, HelloGameMessage message)
        {
            client.Server.Send(new AuthenticationTicketMessage("fr", client.Ticket));
        }
    }
}