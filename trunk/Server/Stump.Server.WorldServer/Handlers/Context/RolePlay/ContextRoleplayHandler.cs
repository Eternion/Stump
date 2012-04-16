using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Handlers.Basic;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay
{
    public partial class ContextRoleplayHandler
    {
        [WorldHandler(ChangeMapMessage.Id)]
        public static void HandleChangeMapMessage(WorldClient client, ChangeMapMessage message)
        {
            var neighbourState = client.Character.Map.GetClientMapRelativePosition(message.mapId);

            // todo : check with MapChangeData the neighbour validity
            if (neighbourState != MapNeighbour.None && client.Character.Position.Cell.MapChangeData != 0)
                client.Character.Teleport(neighbourState);
        }

        [WorldHandler(MapInformationsRequestMessage.Id)]
        public static void HandleMapInformationsRequestMessage(WorldClient client, MapInformationsRequestMessage message)
        {
            SendMapComplementaryInformationsDataMessage(client);

            var fightCount = client.Character.Map.GetFightCount();

            if (fightCount > 0)
                SendMapFightCountMessage(client, fightCount);
        }

        [WorldHandler(MapRunningFightListRequestMessage.Id)]
        public static void HandleMapRunningFightListRequestMessage(WorldClient client, MapRunningFightListRequestMessage message)
        {
            SendMapRunningFightListMessage(client, client.Character.Map.GetFights());
        }

        [WorldHandler(MapRunningFightDetailsRequestMessage.Id)]
        public static void HandleMapRunningFightDetailsRequestMessage(WorldClient client, MapRunningFightDetailsRequestMessage message)
        {
            var fight = FightManager.Instance.GetFight(message.fightId);

            if (fight == null || fight.Map != client.Character.Map)
                return;

            SendMapRunningFightDetailsMessage(client, fight);
            BasicHandler.SendBasicNoOperationMessage(client);
        }

        public static void SendMapRunningFightListMessage(IPacketReceiver client, IEnumerable<Fight> fights)
        {
            client.Send(new MapRunningFightListMessage(fights.Select(entry => entry.GetFightExternalInformations())));
        }

        public static void SendMapRunningFightDetailsMessage(IPacketReceiver client, Fight fight)
        {
            var redFighters = fight.RedTeam.GetAllFighters().ToArray();
            var blueFighters = fight.BlueTeam.GetAllFighters().ToArray();

            var fighters = redFighters.Concat(blueFighters).ToArray();

            client.Send(new MapRunningFightDetailsMessage(
                fight.Id,
                fighters.Select(entry => entry.GetMapRunningFighterName()),
                fighters.Select(entry => (short)entry.Level),
                (sbyte) redFighters.Length,
                fighters.Select(entry => entry.IsAlive())));
        }

        public static void SendCurrentMapMessage(IPacketReceiver client, int mapId)
        {
            // todo
            client.Send(new CurrentMapMessage(mapId, "649ae451ca33ec53bbcbcc33becf15f4"));
        }

        public static void SendMapFightCountMessage(IPacketReceiver client, short fightsCount)
        {
            client.Send(new MapFightCountMessage(fightsCount));
        }

        public static void SendMapComplementaryInformationsDataMessage(WorldClient client)
        {
            client.Send(client.Character.Map.GetMapComplementaryInformationsDataMessage(client.Character));
        }

        public static void SendGameRolePlayShowActorMessage(IPacketReceiver client, RolePlayActor actor)
        {
            client.Send(new GameRolePlayShowActorMessage(actor.GetGameContextActorInformations() as GameRolePlayActorInformations));
        }
    }
}