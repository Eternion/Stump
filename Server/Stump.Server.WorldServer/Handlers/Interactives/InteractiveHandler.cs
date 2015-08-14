using System.Collections.Generic;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
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
            if (client.Character.IsInZaapDialog())
            {
                var map = World.Instance.GetMap(message.mapId);

                if (map == null)
                    return;

                client.Character.ZaapDialog.Teleport(map);
            }
            else if (client.Character.IsInZaapiDialog())
            {
                var map = World.Instance.GetMap(message.mapId);

                if (map == null)
                    return;

                client.Character.ZaapiDialog.Teleport(map);
            }
        }

        public static void SendInteractiveUsedMessage(IPacketReceiver client, Character user, InteractiveObject interactiveObject, Skill skill)
        {
            client.Send(new InteractiveUsedMessage(user.Id, interactiveObject.Id, (short) skill.Id, (short) skill.GetDuration(user)));
        }

        public static void SendStatedElementUpdatedMessage(IPacketReceiver client, int elementId, short elementCellId, int state)
        {
            client.Send(new StatedElementUpdatedMessage(new StatedElement(elementId, elementCellId, state)));
        }

        public static void SendMapObstacleUpdatedMessage(IPacketReceiver client, IEnumerable<MapObstacle> obstacles)
        {
            client.Send(new MapObstacleUpdateMessage(obstacles));
        }
    }
}