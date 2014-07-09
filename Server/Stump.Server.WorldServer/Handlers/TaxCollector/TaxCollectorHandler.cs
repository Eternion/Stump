using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Guilds;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Handlers.TaxCollector
{
    public class TaxCollectorHandler : WorldHandlerContainer
    {
        [WorldHandler(TaxCollectorHireRequestMessage.Id)]
        public static void HandleTaxCollectorHireRequestMessage(WorldClient client, TaxCollectorHireRequestMessage message)
        {
            if (client.Character.Guild == null)
                return;

            TaxCollectorManager.Instance.AddTaxCollectorSpawn(client.Character);
        }

        [WorldHandler(GameRolePlayTaxCollectorFightRequestMessage.Id)]
        public static void HandleGameRolePlayTaxCollectorFightRequestMessage(WorldClient client, GameRolePlayTaxCollectorFightRequestMessage message)
        {
            var taxCollector = client.Character.Map.GetActor<TaxCollectorNpc>(message.taxCollectorId);

            var result = client.Character.CanAttack(taxCollector);

            if (result != FighterRefusedReasonEnum.FIGHTER_ACCEPTED)
            {
                ContextHandler.SendChallengeFightJoinRefusedMessage(client, client.Character, result);
                return;
            }

            var fight = FightManager.Instance.CreatePvTFight(client.Character.Map);

            fight.RedTeam.AddFighter(client.Character.CreateFighter(fight.RedTeam));
            fight.BlueTeam.AddFighter(taxCollector.CreateFighter(fight.BlueTeam));

            fight.StartPlacement();
        }

        [WorldHandler(GuildFightJoinRequestMessage.Id)]
        public static void HandleGuildFightJoinRequestMessage(WorldClient client, GuildFightJoinRequestMessage message)
        {
            if (client.Character.Guild == null)
                return;

            var taxCollector = client.Character.Guild.TaxCollectors.FirstOrDefault(x => x.GlobalId == message.taxCollectorId);

            if (taxCollector == null || !taxCollector.IsFighting)
                return;

            var fight = taxCollector.Fighter.Fight as FightPvT;

            if (fight == null)
                return;

            var result = fight.AddDefender(client.Character);

            if (result != FighterRefusedReasonEnum.FIGHTER_ACCEPTED)
                ContextHandler.SendChallengeFightJoinRefusedMessage(client, client.Character, result);
        }

        [WorldHandler(GuildFightLeaveRequestMessage.Id)]
        public static void HandleGuildFightLeaveRequestMessage(WorldClient client, GuildFightLeaveRequestMessage message)
        {
             if (client.Character.Guild == null)
                return;

            var taxCollector =
                client.Character.Guild.TaxCollectors.FirstOrDefault(x => x.GlobalId == message.taxCollectorId);

            if (taxCollector == null || !taxCollector.IsFighting)
                return;

            var fight = taxCollector.Fighter.Fight as FightPvT;

            if (fight == null)
                return;

            fight.RemoveDefender(client.Character);
        }

        public static void SendTaxCollectorListMessage(IPacketReceiver client, Guild guild)
        {
            client.Send(new TaxCollectorListMessage((sbyte)guild.MaxTaxCollectors, guild.HireCost,
                guild.TaxCollectors.Select(x => x.GetNetworkTaxCollector()), 
                guild.TaxCollectors.Where(x => x.IsFighting).Select(x => x.Fighter.GetTaxCollectorFightersInformation())));
        }

        public static void SendTaxCollectorAttackedMessage(IPacketReceiver client, TaxCollectorNpc taxCollector)
        {
            client.Send(new TaxCollectorAttackedMessage(taxCollector.FirstNameId, taxCollector.LastNameId, (short) taxCollector.Map.Position.X, (short) taxCollector.Map.Position.Y,
                taxCollector.Map.Id, (short)taxCollector.Map.SubArea.Id));
        }

        public static void SendGuildFightPlayersHelpersJoinMessage(IPacketReceiver client, TaxCollectorNpc taxCollector, Character character)
        {
            client.Send(new GuildFightPlayersHelpersJoinMessage(taxCollector.GlobalId, character.GetCharacterMinimalPlusLookInformations()));
        }

        public static void SendGuildFightPlayersHelpersLeaveMessage(IPacketReceiver client, TaxCollectorNpc taxCollector, Character character)
        {
            client.Send(new GuildFightPlayersHelpersLeaveMessage(taxCollector.GlobalId, character.Id));
        }

        public static void SendGuildFightPlayersEnemyRemoveMessage(IPacketReceiver client, TaxCollectorNpc taxCollector, Character character)
        {
            client.Send(new GuildFightPlayersEnemyRemoveMessage(taxCollector.GlobalId, character.Id));
        }

        public static void SendGuildFightPlayersEnemiesListMessage(IPacketReceiver client, TaxCollectorNpc taxCollector, IEnumerable<Character> characters)
        {
            client.Send(new GuildFightPlayersEnemiesListMessage(taxCollector.GlobalId, characters.Select(x => x.GetCharacterMinimalPlusLookInformations())));
        }

        public static void SendTaxCollectorAttackedResultMessage(IPacketReceiver client, bool deadOrAlive, TaxCollectorNpc taxCollector)
        {
            client.Send(new TaxCollectorAttackedResultMessage(deadOrAlive, taxCollector.GetTaxCollectorBasicInformations()));
        }

        public static void SendGetExchangeGuildTaxCollectorMessage(IPacketReceiver client, TaxCollectorNpc taxCollector)
        {
            client.Send(taxCollector.GetExchangeGuildTaxCollector());
        }

        public static void SendTaxCollectorMovementMessage(IPacketReceiver client, bool hireOrFire, TaxCollectorNpc taxCollector, string name)
        {
            client.Send(new TaxCollectorMovementMessage(hireOrFire, taxCollector.GetTaxCollectorBasicInformations(), name));
        }

        public static void SendTaxCollectorMovementAddMessage(IPacketReceiver client, TaxCollectorNpc taxCollector)
        {
            client.Send(new TaxCollectorMovementAddMessage(taxCollector.GetNetworkTaxCollector()));
        }

        public static void SendTaxCollectorMovementRemoveMessage(IPacketReceiver client, TaxCollectorNpc taxCollector)
        {
            client.Send(new TaxCollectorMovementRemoveMessage(taxCollector.GlobalId));
        }
    }
}
