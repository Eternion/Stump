using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Paddocks;
using Stump.Server.WorldServer.Game.Arena;

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

            var objectItems = client.Character.Map.GetObjectItems();

            if (client.Character.Map.Id == ArenaManager.KolizeumMapId)
            {
                var arenaCount = ArenaManager.Instance.Arenas.Sum(x => x.Value.Map.GetFightCount());

                if (arenaCount > 0)
                    SendMapFightCountMessage(client, (short)arenaCount);
            }
            else if (client.Character.Map.GetFightCount() > 0)
                    SendMapFightCountMessage(client, client.Character.Map.GetFightCount());

            foreach (var objectItem in objectItems.ToArray())
                SendObjectGroundAddedMessage(client, objectItem);

            var paddock = PaddockManager.Instance.GetPaddockByMap(message.mapId);
            if (paddock != null)
                client.Send(paddock.GetPaddockPropertiesMessage());

            var skills = client.Character.Map.GetInteractiveObjects().SelectMany(x => x.GetSkills().Where(y => (y is SkillCraft || y is SkillRuneCraft) && y.IsEnabled(client.Character)).Select(y => y.SkillTemplate)).Distinct();

            SendJobMultiCraftAvailableSkillsMessage(client, client.Character, skills, true);
        }

        [WorldHandler(MapRunningFightListRequestMessage.Id)]
        public static void HandleMapRunningFightListRequestMessage(WorldClient client, MapRunningFightListRequestMessage message)
        {
            if (client.Character.Map.Id == ArenaManager.KolizeumMapId)
                SendMapRunningFightListMessage(client, ArenaManager.Instance.Arenas.SelectMany(x => x.Value.Map.Fights), client.Character);
            else
                SendMapRunningFightListMessage(client, client.Character.Map.Fights, client.Character);
        }

        [WorldHandler(MapRunningFightDetailsRequestMessage.Id)]
        public static void HandleMapRunningFightDetailsRequestMessage(WorldClient client, MapRunningFightDetailsRequestMessage message)
        {
            var fight = FightManager.Instance.GetFight(message.fightId);

            if (fight == null || (fight.Map != client.Character.Map && client.Character.Map.Id != ArenaManager.KolizeumMapId))
                return;

            SendMapRunningFightDetailsMessage(client, fight);
        }

        [WorldHandler(GameRolePlayFreeSoulRequestMessage.Id)]
        public static void HandleGameRoleplayFreeSoulRequestMessage(WorldClient client, GameRolePlayFreeSoulRequestMessage message)
        {
            client.Character.FreeSoul();
        }

        public static void SendMapRunningFightListMessage(IPacketReceiver client, IEnumerable<IFight> fights, Character character)
        {
            client.Send(new MapRunningFightListMessage(fights.Select(entry => entry.GetFightExternalInformations(character))));
        }

        public static void SendMapRunningFightDetailsMessage(IPacketReceiver client, IFight fight)
        {
            var redFighters = fight.ChallengersTeam.GetAllFighters(x => !(x is SummonedFighter) && !(x is SummonedBomb)).ToArray();
            var blueFighters = fight.DefendersTeam.GetAllFighters(x => !(x is SummonedFighter) && !(x is SummonedBomb)).ToArray();

            var partiesName = fight.GetPartiesName().ToArray();

            if (partiesName.Length > 0)
            {
                client.Send(new MapRunningFightDetailsExtendedMessage(
                    fight.Id,
                    redFighters.Select(entry => entry.GetGameFightFighterLightInformations()),
                    blueFighters.Select(entry => entry.GetGameFightFighterLightInformations()),
                    partiesName));
            }
            else
            {
                client.Send(new MapRunningFightDetailsMessage(
                    fight.Id,
                    redFighters.Select(entry => entry.GetGameFightFighterLightInformations()),
                    blueFighters.Select(entry => entry.GetGameFightFighterLightInformations())));
            }
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

        public static void SendGameRolePlayShowActorMessage(IPacketReceiver client, Character character, RolePlayActor actor)
        {
            client.Send(new GameRolePlayShowActorMessage(actor.GetGameContextActorInformations(character) as GameRolePlayActorInformations));
        }

        public static void SendObjectGroundAddedMessage(IPacketReceiver client, WorldObjectItem objectItem)
        {
            client.Send(new ObjectGroundAddedMessage(objectItem.Cell.Id, (short)objectItem.Item.Id));
        }

        public static void SendObjectGroundRemovedMessage(IPacketReceiver client, WorldObjectItem objectItem)
        {
            client.Send(new ObjectGroundRemovedMessage(objectItem.Cell.Id));
        }
    }
}