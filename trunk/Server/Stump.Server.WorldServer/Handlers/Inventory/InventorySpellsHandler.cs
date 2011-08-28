using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Handlers.Inventory
{
    public partial class InventoryHandler : WorldHandlerContainer
    {
        [WorldHandler(SpellUpgradeRequestMessage.Id)]
        public static void HandleSpellUpgradeRequestMessage(WorldClient client, SpellUpgradeRequestMessage message)
        {
            client.ActiveCharacter.Spells.BoostSpell(message.spellId);
        }

        public static void SendSpellUpgradeSuccessMessage(WorldClient client, Spell spell)
        {
            client.Send(new SpellUpgradeSuccessMessage(spell.Id, spell.CurrentLevel));
        }

        public static void SendSpellUpgradeFailureMessage(WorldClient client)
        {
            client.Send(new SpellUpgradeFailureMessage());
        }

        public static void SendSpellListMessage(WorldClient client, bool previsualization)
        {

            client.Send(new SpellListMessage(previsualization,
                                             client.ActiveCharacter.Spells.GetSpells().Select(
                                                 entry => entry.GetSpellItem())));
        }
    }
}