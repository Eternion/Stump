using Stump.BaseCore.Framework.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public class SubscriberHandler : WorldHandlerContainer
    {
        [Variable]
        public static bool EnableSubscriptionLimitation = true;

        public static void SendSubscriptionLimitationMessage(WorldClient client, SubscriptionRequiredEnum reason)
        {
            client.Send(new SubscriptionLimitationMessage((uint) reason));
        }

        public static void SendSubscriptionZoneMessage(WorldClient client)
        {
            client.Send(new SubscriptionZoneMessage(EnableSubscriptionLimitation));
        }
    }
}