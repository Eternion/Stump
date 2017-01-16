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

        [WorldHandler(ZaapRespawnSaveRequestMessage.Id)]
        public static void HandleZaapRespawnSaveRequestMessage(WorldClient client, ZaapRespawnSaveRequestMessage message)
        {
            if (client.Character.Map.Zaap == null)
                return;

            client.Character.SetSpawnPoint(client.Character.Map);
        }

        public static void SendZaapRespawnUpdatedMessage(WorldClient client)
        {
            client.Send(new ZaapRespawnUpdatedMessage(client.Character.Record.SpawnMapId ?? 0));
        }

        public static void SendInteractiveUsedMessage(IPacketReceiver client, Character user, InteractiveObject interactiveObject, Skill skill)
        {
            //todo: CanMove
            client.Send(new InteractiveUsedMessage(user.Id, interactiveObject.Id, (short) skill.SkillTemplate.Id, (short) (skill.GetDuration(user, true)/100), true));
        }

        public static void SendInteractiveUseErrorMessage(IPacketReceiver client, int interactiveId, int skillId)
        {
            client.Send(new InteractiveUseErrorMessage(interactiveId, skillId));
        }

        public static void SendStatedElementUpdatedMessage(IPacketReceiver client, int elementId, short elementCellId, int state)
        {
            client.Send(new StatedElementUpdatedMessage(new StatedElement(elementId, elementCellId, state, true)));
        }

        public static void SendMapObstacleUpdatedMessage(IPacketReceiver client, IEnumerable<MapObstacle> obstacles)
        {
            client.Send(new MapObstacleUpdateMessage(obstacles));
        }

        public static void SendInteractiveElementUpdatedMessage(IPacketReceiver client, Character character, InteractiveObject interactive)
        {
            client.Send(new InteractiveElementUpdatedMessage(interactive.GetInteractiveElement(character)));
        }

        public static void SendInteractiveUseEndedMessage(IPacketReceiver client, InteractiveObject interactive, Skill skill)
        {
            client.Send(new InteractiveUseEndedMessage(interactive.Id, (short)skill.SkillTemplate.Id));
        }
    }
}