using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Handlers.Social
{
    public class SocialHandler : WorldHandlerContainer
    {
        [WorldHandler(ContactLookRequestByIdMessage.Id)]
        public static void HandleContactLookRequestByIdMessage(WorldClient client, ContactLookRequestByIdMessage message)
        {
            var target = World.Instance.GetCharacter((int) message.playerId);

            if (target != null)
                SendContactLookMessage(client, message.requestId, target);
            else
                SendContactLookErrorMessage(client, message.requestId);
        }

        [WorldHandler(ContactLookRequestByNameMessage.Id)]
        public static void HandleContactLookRequestByNameMessage(WorldClient client, ContactLookRequestByNameMessage message)
        {
            var target = World.Instance.GetCharacter(message.playerName);

            if (target != null)
                SendContactLookMessage(client, message.requestId, target);
            else
                SendContactLookErrorMessage(client, message.requestId);
        }

        public static void SendContactLookMessage(IPacketReceiver client, int requestId, Character character)
        {
            client.Send(new ContactLookMessage(requestId, character.Name, character.Id, character.Look.GetEntityLook()));
        }

        public static void SendContactLookErrorMessage(IPacketReceiver client, int requestId)
        {
            client.Send(new ContactLookErrorMessage(requestId));
        }
    }
}