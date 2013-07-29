using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Characters;

namespace Stump.Server.WorldServer.Handlers.Inventory
{
    public partial class InventoryHandler : WorldHandlerContainer
    {
        [WorldHandler(SpellUpgradeRequestMessage.Id)]
        public static void HandleSpellUpgradeRequestMessage(WorldClient client, SpellUpgradeRequestMessage message)
        {
            client.Character.Spells.BoostSpell(message.spellId);
            client.Character.RefreshStats();
        }

        public static void SendSpellUpgradeSuccessMessage(IPacketReceiver client, Spell spell)
        {
            client.Send(new SpellUpgradeSuccessMessage(spell.Id, (sbyte) spell.CurrentLevel));
        }

        public static void SendSpellUpgradeSuccessMessage(IPacketReceiver client, int spellId, sbyte level)
        {
            client.Send(new SpellUpgradeSuccessMessage(spellId, level));
        }

        public static void SendSpellUpgradeFailureMessage(IPacketReceiver client)
        {
            client.Send(new SpellUpgradeFailureMessage());
        }

        public static void SendSpellListMessage(WorldClient client, bool previsualization)
        {

            client.Send(new SpellListMessage(previsualization,
                                             client.Character.Spells.GetSpells().Select(
                                                 entry => entry.GetSpellItem())));
        }

        public static void SendSpellForgottenMessage(IPacketReceiver client, IEnumerable<Spell> forgottenSpells, short spellPoints)
        {
            client.Send(new SpellForgottenMessage(forgottenSpells.Select(x => (short)x.Id), spellPoints));
        }

        public static void SendSpellForgottenMessage(IPacketReceiver client, Spell forgottenSpell, short spellPoints)
        {
            client.Send(new SpellForgottenMessage(new [] {(short) forgottenSpell.Id}, spellPoints));
        }
    }
}