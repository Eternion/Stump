using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Characters
{
    public class CharacterStatusHandler : WorldHandlerContainer
    {
        [WorldHandler(PlayerStatusUpdateRequestMessage.Id)]
        public static void HandlePlayerStatusUpdateRequestMessage(WorldClient client, PlayerStatusUpdateRequestMessage message)
        {
            client.Character.SetStatus((PlayerStatusEnum)message.status.statusId);
        }

        public static void SendPlayerStatusUpdateMessage(WorldClient client, PlayerStatus status)
        {
            client.Send(new PlayerStatusUpdateMessage(client.Account.Id, client.Character.Id, status));
        }

        public static void SendPlayerStatusUpdateErrorMessage(WorldClient client)
        {
            client.Send(new PlayerStatusUpdateErrorMessage());
        }
    }
}
