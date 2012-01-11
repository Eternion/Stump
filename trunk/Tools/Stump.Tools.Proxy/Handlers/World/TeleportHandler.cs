
using System;
using Stump.DofusProtocol.Messages;
using Stump.Tools.Proxy.Data;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.World
{
    public class TeleportHandler : WorldHandlerContainer
    {
        [WorldHandler(CurrentMapMessage.Id)]
        public static void HandleCurrentMapMessage(WorldClient client, CurrentMapMessage message)
        {
            client.Send(message);

            if (client.HasReceive(LeaveDialogMessage.Id, 2))
                client.GuessNpcReply = client.LastNpcReply;

            if(client.GuessAction)
            {
                client.CallWhenTeleported(() => DataFactory.BuildActionTeleport(client, message));
            }
        }

        [WorldHandler(TeleportDestinationsListMessage.Id)]
        public static void HandleTeleportDestinationsListMessage(WorldClient client, TeleportDestinationsListMessage message)
        {
            client.Send(message);
        }
    }
}