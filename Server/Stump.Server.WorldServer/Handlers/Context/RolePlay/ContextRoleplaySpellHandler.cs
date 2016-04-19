using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Dialogs.Spells;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay
{
    public partial class ContextRoleplayHandler : WorldHandlerContainer
    {
        /*[WorldHandler(SpellUpgradeRequestMessage.Id)]
        public static void HandleSpellUpgradeRequestMessage(WorldClient client, SpellUpgradeRequestMessage message)
        {
            client.Character.Spells.BoostSpell(message.spellId, (ushort)message.spellLevel);
            client.Character.RefreshStats();
        }

        [WorldHandler(ValidateSpellForgetMessage.Id)]
        public static void HandleValidateSpellForgetMessage(WorldClient client, ValidateSpellForgetMessage message)
        {
            var panel = client.Character.Dialog as SpellForgetPanel;
            if (panel != null)
            {
                panel.DowngradeSpell(client, message.spellId);
            }
        }

        public static void SendSpellForgetUIMessage(IPacketReceiver client, bool open)
        {
            client.Send(new SpellForgetUIMessage(open));
        }

        public static void SendSpellForgottenMessage(IPacketReceiver client, IEnumerable<Spell> forgottenSpells, short spellPoints)
        {
            client.Send(new SpellForgottenMessage(forgottenSpells.Select(x => (short)x.Id), spellPoints));
        }

        public static void SendSpellForgottenMessage(IPacketReceiver client, Spell forgottenSpell, short spellPoints)
        {
            client.Send(new SpellForgottenMessage(new[] { (short)forgottenSpell.Id }, spellPoints));
        }

        public static void SendSpellUpgradeSuccessMessage(IPacketReceiver client, Spell spell)
        {
            client.Send(new SpellUpgradeSuccessMessage(spell.Id, (sbyte)spell.CurrentLevel));
        }

        public static void SendSpellUpgradeSuccessMessage(IPacketReceiver client, int spellId, sbyte level)
        {
            client.Send(new SpellUpgradeSuccessMessage(spellId, level));
        }

        public static void SendSpellUpgradeFailureMessage(IPacketReceiver client)
        {
            client.Send(new SpellUpgradeFailureMessage());
        }*/

        public static void SendSpellItemBoostMessage(IPacketReceiver client, int statId, short spellId, short value)
        {
            client.Send(new SpellItemBoostMessage(statId, spellId, value));
        }
    }
}