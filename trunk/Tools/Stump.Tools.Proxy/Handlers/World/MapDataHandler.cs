
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Tools.Proxy.Data;
using Stump.Tools.Proxy.Network;

namespace Stump.Tools.Proxy.Handlers.World
{
    public class MapDataHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof (MapComplementaryInformationsDataMessage))]
        public static void HandleMapComplementaryInformationsDataMessage(WorldClient client,
                                                                         MapComplementaryInformationsDataMessage message)
        {
            client.Send(message);

            client.MapNpcs.Clear();
            client.MapIOs.Clear();
            client.CurrentMap = message.mapId;

            Parallel.ForEach(message.actors, actor =>
            {
                DataFactory.HandleActorInformations(client, actor);

                if (actor is GameRolePlayNpcInformations)
                    client.MapNpcs.Add((actor as GameRolePlayNpcInformations).contextualId,
                                       (GameRolePlayNpcInformations) actor);
                else if (actor is GameRolePlayCharacterInformations &&
                         (actor as GameRolePlayCharacterInformations).contextualId == client.CharacterInformations.id)
                    client.Disposition = actor.disposition;
            });

            Parallel.ForEach(message.interactiveElements, entry =>
            {
                DataFactory.HandleInteractiveObject(client, entry);

                client.MapIOs.Add((int) entry.elementId, entry);
            });

            client.GuessCellTrigger = null;
            client.GuessNpcFirstAction = null;
            client.GuessNpcReply = null;
            client.GuessSkillAction = null;
        }

        [WorldHandler(typeof (GameMapMovementMessage))]
        public static void HandleGameMapMovementConfirmMessage(WorldClient client, GameMapMovementMessage message)
        {
            client.Send(message);

            var cell = (ushort) (message.keyMovements.Last() & 4095);

            Point point = MapPoint.CellIdToCoord(cell);
            if (point.X != 0 && point.Y != 0)
            {
                client.GuessCellTrigger = cell;
            }
        }
    }
}