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
            client.Send(new SpellUpgradeSuccessMessage(spell.Id, spell.CurrentLevel));
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
    }
}