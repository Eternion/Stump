using System.Collections.Generic;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Context
{
    public partial class ContextHandler : WorldHandlerContainer
    {
        public static void SendNotificationListMessage(WorldClient client, List<int> notifications)
        {
            client.Send(new NotificationListMessage(notifications));
        }
    }
}