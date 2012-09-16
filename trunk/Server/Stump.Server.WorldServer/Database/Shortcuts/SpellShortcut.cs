using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Database
{
    public class SpellShortcutConfiguration : EntityTypeConfiguration<SpellShortcut>
    {
        public SpellShortcutConfiguration()
        {
            Map(x => x.Requires("Discriminator").HasValue("Spell"));
        }
    }
    public class SpellShortcut : Shortcut
    {
        public SpellShortcut()
        {
            
        }

        public SpellShortcut(CharacterRecord owner, int slot, short spellId)
            : base(owner, slot)
        {
            SpellId = spellId;
        }

        public short SpellId
        {
            get;
            set;
        }

        public override DofusProtocol.Types.Shortcut GetNetworkShortcut()
        {
            return new ShortcutSpell(Slot, SpellId);
        }
    }
}