
using Stump.DofusProtocol.Messages;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.Auth
{
    public class IdentificationSuccessMessageHandler : AuthHandlerContainer
    {
        [AuthHandler(IdentificationSuccessMessage.Id)]
        public static void HandleIdentificationSuccessMessage(AuthClient client, IdentificationSuccessMessage message)
        {
            //message.accountId = 1;
            //message.nickname = "MegaAdmin";
            message.hasRights = true;

            client.Send(message);
        }
    }
}