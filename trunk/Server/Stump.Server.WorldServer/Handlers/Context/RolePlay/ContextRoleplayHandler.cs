using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Worlds;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay;
using Stump.Server.WorldServer.Worlds.Maps;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay
{
    public partial class ContextRoleplayHandler
    {
        [WorldHandler(ChangeMapMessage.Id)]
        public static void HandleChangeMapMessage(WorldClient client, ChangeMapMessage message)
        {
            var neighbourState = client.ActiveCharacter.Map.GetMapRelativePosition(message.mapId);

            if (neighbourState != MapNeighbour.None)
                client.ActiveCharacter.Teleport(neighbourState);
        }

        [WorldHandler(MapInformationsRequestMessage.Id)]
        public static void HandleMapInformationsRequestMessage(WorldClient client, MapInformationsRequestMessage message)
        {
            SendMapComplementaryInformationsDataMessage(client);
        }

        public static void SendCurrentMapMessage(WorldClient client, int mapId)
        {
            client.Send(new CurrentMapMessage(mapId));
        }

        public static void SendMapComplementaryInformationsDataMessage(WorldClient client)
        {
            client.Send(client.ActiveCharacter.Map.GetMapComplementaryInformationsDataMessage());
        }

        public static void SendGameRolePlayShowActorMessage(WorldClient client, RolePlayActor actor)
        {
            client.Send(new GameRolePlayShowActorMessage(actor.GetGameContextActorInformations() as GameRolePlayActorInformations));
        }
    }
}