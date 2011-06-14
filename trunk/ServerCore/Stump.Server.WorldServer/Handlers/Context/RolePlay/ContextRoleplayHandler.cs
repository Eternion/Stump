
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class ContextHandler
    {
        [WorldHandler(typeof (ChangeMapMessage))]
        public static void HandleChangeMapMessage(WorldClient client, ChangeMapMessage message)
        {
            Map nextMap = World.Instance.GetMap((int) message.mapId);

            client.ActiveCharacter.ChangeMap(nextMap);
        }

        [WorldHandler(typeof (MapInformationsRequestMessage))]
        public static void HandleMapInformationsRequestMessage(WorldClient client, MapInformationsRequestMessage message)
        {
            if (!client.ActiveCharacter.InWorld)
                client.ActiveCharacter.LogIn();

            SendMapComplementaryInformationsDataMessage(client);
        }

        public static void SendCurrentMapMessage(WorldClient client, int mapId)
        {
            client.Send(new CurrentMapMessage((uint) mapId));
        }

        public static void SendMapComplementaryInformationsDataMessage(WorldClient client)
        {
            client.Send(new MapComplementaryInformationsDataMessage(
                            (uint) client.ActiveCharacter.Zone.Id,
                            (uint) client.ActiveCharacter.Map.Id,
                            0,
                            new List<HouseInformations>(),
                            client.ActiveCharacter.Map.Entities.Select(entry => entry.Value.ToNetworkActor(client)).
                                ToList(),
                            client.ActiveCharacter.Map.InteractiveObjects.Select(
                                entry => entry.Value.ToNetworkElement(client)).ToList(),
                            new List<StatedElement>(),
                            new List<MapObstacle>(),
                            new List<FightCommonInformations>()));
        }


        public static void SendGameRolePlayShowActorMessage(WorldClient client, Entity entity)
        {
            client.Send(new GameRolePlayShowActorMessage(entity.ToNetworkActor(client)));
        }
    }
}