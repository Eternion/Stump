using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
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

            // todo : check with MapChangeData the neighbour validity
            if (neighbourState != MapNeighbour.None && client.ActiveCharacter.Position.Cell.MapChangeData != 0)
                client.ActiveCharacter.Teleport(neighbourState);
        }

        [WorldHandler(MapInformationsRequestMessage.Id)]
        public static void HandleMapInformationsRequestMessage(WorldClient client, MapInformationsRequestMessage message)
        {
            SendMapComplementaryInformationsDataMessage(client);
        }

        public static void SendCurrentMapMessage(IPacketReceiver client, int mapId)
        {
            client.Send(new CurrentMapMessage(mapId));
        }

        public static void SendMapFightCountMessage(IPacketReceiver client, short fightsCount)
        {
            client.Send(new MapFightCountMessage(fightsCount));
        }

        public static void SendMapComplementaryInformationsDataMessage(WorldClient client)
        {
            client.Send(client.ActiveCharacter.Map.GetMapComplementaryInformationsDataMessage(client.ActiveCharacter));
        }

        public static void SendGameRolePlayShowActorMessage(IPacketReceiver client, RolePlayActor actor)
        {
            client.Send(new GameRolePlayShowActorMessage(actor.GetGameContextActorInformations() as GameRolePlayActorInformations));
        }
    }
}