
using Stump.DofusProtocol.Messages;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.Auth
{
    public class IdentificationSuccessMessageHandler : AuthHandlerContainer
    {
        [AuthHandler(typeof (IdentificationSuccessMessage))]
        public static void HandleIdentificationSuccessMessage(AuthClient client, IdentificationSuccessMessage message)
        {
            //message.accountId = 1;
            //message.nickname = "MegaAdmin";
            message.hasRights = true;

            client.Send(message);
        }

        [AuthHandler(typeof(IdentificationFailedForBadVersionMessage))]
        public static void HandleIdentificationFailedForBadVersionMessage(AuthClient client, IdentificationFailedForBadVersionMessage message)
        {
            //message.accountId = 1;
            //message.nickname = "MegaAdmin";
          //  message.hasRights = true;

            client.Send(new IdentificationSuccessMessage("jeredejerede",1,1,true,"",0,false));
        }
    }
}