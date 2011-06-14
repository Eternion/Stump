
using System.Net;
using Stump.DofusProtocol.Messages;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.World
{
    public class AuthenticationTicketMessageHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (AuthenticationTicketMessage))]
        public static void HandleAuthenticationTicketMessage(WorldClient client, AuthenticationTicketMessage message)
        {
            var serverDataMessage = WorldClient.PopTicket(message.ticket);
            if (serverDataMessage != null)
            {
                client.Ticket = serverDataMessage.ticket;
                client.BindToServer(new IPEndPoint(IPAddress.Parse(serverDataMessage.address), (int)serverDataMessage.port));
            }
            else
                client.Disconnect();
        }
    }
}