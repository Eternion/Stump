
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public class SocialHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (ContactLookRequestMessage))]
        public static void HandleContactLookRequestMessage(WorldClient client, ContactLookRequestMessage message)
        {
        }

        [WorldHandler(typeof (ContactLookRequestByIdMessage))]
        public static void HandleContactLookRequestByIdMessage(WorldClient client, ContactLookRequestByIdMessage message)
        {
        }

        public static void SendContactLookMessage(WorldClient client)
        {
            client.Send(new ContactLookMessage());
        }

        public static void SendContactLookErrorMessage(WorldClient client)
        {
            client.Send(new ContactLookErrorMessage());
        }
    }
}