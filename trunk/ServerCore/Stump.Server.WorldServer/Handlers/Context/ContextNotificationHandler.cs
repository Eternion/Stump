
using System.Collections.Generic;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ContextHandler : WorldHandlerContainer
    {
        public static void SendNotificationListMessage(WorldClient client, List<int> notifications)
        {
            client.Send(new NotificationListMessage(notifications));
        }
    }
}