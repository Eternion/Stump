using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Fights;

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

        public static void SendTaxCollectorListMessage(WorldClient client)
        {
            client.Send(new TaxCollectorListMessage((sbyte)client.Character.Guild.MaxTaxCollectors, client.Character.Guild.HireCost,
                client.Character.Guild.TaxCollectors.Select(x => x.GetNetworkTaxCollector()), new TaxCollectorFightersInformation[0]));
        }

        public static void SendTaxCollectorAttackedMessage(IPacketReceiver client, TaxCollectorNpc taxCollector)
        {
            client.Send(new TaxCollectorAttackedMessage(taxCollector.FirstNameId, taxCollector.LastNameId, (short) taxCollector.Map.Position.X, (short) taxCollector.Map.Position.Y,
                taxCollector.Map.Id, taxCollector.Map.SubArea.Id));
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
