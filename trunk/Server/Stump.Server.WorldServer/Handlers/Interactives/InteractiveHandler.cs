using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs.Interactives;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;

namespace Stump.Server.WorldServer.Handlers.Interactives
{
    public class InteractiveHandler : WorldHandlerContainer
    {
        [WorldHandler(InteractiveUseRequestMessage.Id)]
        public static void HandleInteractiveUseRequestMessage(WorldClient client, InteractiveUseRequestMessage message)
        {
            client.Character.Map.UseInteractiveObject(client.Character, message.elemId, message.skillInstanceUid);
        }

        [WorldHandler(InteractiveUseEndedMessage.Id)]
        public static void HandleInteractiveUseEndedMessage(WorldClient client, InteractiveUseEndedMessage message)
        {

        }

        [WorldHandler(TeleportRequestMessage.Id)]
        public static void HandleTeleportRequestMessage(WorldClient client, TeleportRequestMessage message)
        {
            if (!client.Character.IsInZaapDialog())
                return;

            var map = World.Instance.GetMap(message.mapId);

            if (map == null)
                return;

            client.Character.ZaapDialog.Teleport(map);
        }

        public static void SendInteractiveUsedMessage(IPacketReceiver client, Character user, InteractiveObject interactiveObject, Skill skill)
        {
            client.Send(new InteractiveUsedMessage(user.Id, interactiveObject.Id, (short) skill.Id, (short) skill.GetDuration(user)));
        }
    }
}