
using Stump.DofusProtocol.Messages;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.Auth
{
    public class SelectedServerDataMessageHandler : AuthHandlerContainer
    {
        [AuthHandler(SelectedServerDataMessage.Id)]
        public static void HandleSelectedServerDataMessage(AuthClient client, SelectedServerDataMessage message)
        {
            WorldClient.PushTicket(message.ticket, message);

            var customMessage = new SelectedServerDataMessage
                {
                    canCreateNewCharacter = true,
                    serverId = message.serverId,
                    ticket = message.ticket,
                    address = Proxy.WorldAddress,
                    port = (ushort) Proxy.WorldPort
                };

            client.Send(customMessage);
            client.Disconnect();
        }
    }
}