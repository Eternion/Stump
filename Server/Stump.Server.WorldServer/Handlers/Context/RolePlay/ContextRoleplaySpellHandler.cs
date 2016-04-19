using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Handlers.Context.RolePlay
{
    public partial class ContextRoleplayHandler : WorldHandlerContainer
    {
        [WorldHandler(SpellModifyRequestMessage.Id)]
        public static void HandleSpellModifyRequestMessage(WorldClient client, SpellModifyRequestMessage message)
        {
            client.Character.Spells.BoostSpell(message.spellId, (ushort)message.spellLevel);
            client.Character.RefreshStats();
        }

        public static void SendSpellModifySuccessMessage(IPacketReceiver client, Spell spell)
        {
            client.Send(new SpellModifySuccessMessage(spell.Id, (sbyte)spell.CurrentLevel));
        }

        public static void SendSpellModifySuccessMessage(IPacketReceiver client, int spellId, sbyte level)
        {
            client.Send(new SpellModifySuccessMessage(spellId, level));
        }

        public static void SendSpellModifyFailureMessage(IPacketReceiver client)
        {
            client.Send(new SpellModifyFailureMessage());
        }

        public static void SendSpellItemBoostMessage(IPacketReceiver client, int statId, short spellId, short value)
        {
            client.Send(new SpellItemBoostMessage(statId, spellId, value));
        }
    }
}