using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
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
                ContextHandler.SendChallengeFightJoinRefusedMessage(client, client.Character, result);

            var fight = FightManager.Instance.CreatePvTFight(client.Character.Map);

            fight.RedTeam.AddFighter(taxCollector.CreateFighter(fight.RedTeam));
            fight.BlueTeam.AddFighter(client.Character.CreateFighter(fight.BlueTeam));

            fight.StartPlacement();
        }

        [WorldHandler(GuildFightJoinRequestMessage.Id)]
        public static void HandleGuildFightJoinRequestMessage(WorldClient client, GuildFightJoinRequestMessage message)
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

        public static void SendGuildFightPlayersHelpersJoinMessage(IPacketReceiver client, Fight fight, Character character)
        {
            client.Send(new GuildFightPlayersHelpersJoinMessage(fight.Id, character.GetCharacterBaseInformations()));
        }

        public static void SendGuildFightPlayersHelpersLeaveMessage(IPacketReceiver client, Fight fight, Character character)
        {
            client.Send(new GuildFightPlayersHelpersLeaveMessage(fight.Id, character.Id));
        }

        public static void SendGuildFightPlayersEnemyRemoveMessage(IPacketReceiver client, Fight fight, Character character)
        {
            client.Send(new GuildFightPlayersEnemyRemoveMessage(fight.Id, character.Id));
        }

        public static void SendGuildFightPlayersEnemiesListMessage(IPacketReceiver client, Fight fight, IEnumerable<Character> characters)
        {
            client.Send(new GuildFightPlayersEnemiesListMessage(fight.Id, characters.Select(x => x.GetCharacterBaseInformations())));
        }

        public static void SendTaxCollectorAttackedResultMessage(IPacketReceiver client, bool deadOrAlive, TaxCollectorNpc taxCollector)
        {
            client.Send(new TaxCollectorAttackedResultMessage(deadOrAlive, taxCollector.GetTaxCollectorBasicInformations()));
        }
    }
}
