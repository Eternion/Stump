
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Tools.Proxy.Data;
using Stump.Tools.Proxy.Network;
using ServerWorld = Stump.Server.WorldServer.Game.World;

namespace Stump.Tools.Proxy.Handlers.World
{
    public class MapDataHandler : WorldHandlerContainer
    {
        [WorldHandler(MapComplementaryInformationsDataMessage.Id)]
        public static void HandleMapComplementaryInformationsDataMessage(WorldClient client,
                                                                         MapComplementaryInformationsDataMessage message)
        {
            client.Send(message);

            client.MapNpcs.Clear();
            client.MapIOs.Clear();
            client.Actors.Clear();
            client.CurrentMap = ServerWorld.Instance.GetMap(message.mapId);

            foreach(var actor in message.actors)
            {
                if (client.Actors.ContainsKey(actor.contextualId))
                    client.Actors[actor.contextualId] = actor;
                else
                    client.Actors.Add(actor.contextualId, actor);

                DataFactory.HandleActorInformations(client, actor);

                if (actor is GameRolePlayNpcInformations)
                    client.MapNpcs.Add((actor as GameRolePlayNpcInformations).contextualId,
                                       (GameRolePlayNpcInformations) actor);
                else if (actor is GameRolePlayCharacterInformations &&
                         (actor as GameRolePlayCharacterInformations).contextualId == client.CharacterInformations.id)
                    client.Disposition = actor.disposition;
            }

            foreach(var entry in message.interactiveElements)
            {
                DataFactory.HandleInteractiveObject(client, entry);

                client.MapIOs.Add(entry.elementId, entry);
            }

            client.GuessCellTrigger = null;
            client.GuessNpcFirstAction = null;
            client.GuessNpcReply = null;
            client.GuessSkillAction = null;
        }

        [WorldHandler(GameRolePlayShowActorMessage.Id)]
        public static void HandleGameRolePlayShowActorMessage(WorldClient client, GameRolePlayShowActorMessage message)
        {
            client.Send(message);
            if (client.Actors.ContainsKey(message.informations.contextualId))
                client.Actors[message.informations.contextualId] = message.informations;
            else
                client.Actors.Add(message.informations.contextualId, message.informations);
            DataFactory.HandleActorInformations(client, message.informations);
        }

        [WorldHandler(GameContextRemoveElementMessage.Id)]
        public static void HandleGameContextRemoveElementMessage(WorldClient client, GameContextRemoveElementMessage message)
        {
            client.Send(message);
            client.Actors.Remove(message.id);
        }

        [WorldHandler(GameMapMovementMessage.Id)]
        public static void HandleGameMapMovementConfirmMessage(WorldClient client, GameMapMovementMessage message)
        {
            client.Send(message);

            if (message.actorId != client.CharacterInformations.id)
                return;

            var cell = (ushort) (message.keyMovements.Last() & 4095);

            Point point = MapPoint.CellIdToCoord(cell);
            if (point.X != 0 && point.Y != 0)
            {
                client.GuessCellTrigger = cell;
            }
        }
    }
}