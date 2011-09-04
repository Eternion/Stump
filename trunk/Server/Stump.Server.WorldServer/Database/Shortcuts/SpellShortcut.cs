using System;
using Castle.ActiveRecord;
using Stump.Server.WorldServer.Database.Characters;
using DofusSpellShortcut = Stump.DofusProtocol.Types.ShortcutSpell;

namespace Stump.Server.WorldServer.Database.Shortcuts
{
    [ActiveRecord(DiscriminatorValue = "Spell")]
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

        [Property("SpellId")]
        public short SpellId
        {
            get;
            set;
        }

        public override DofusProtocol.Types.Shortcut GetNetworkShortcut()
        {
            return new DofusSpellShortcut(Slot, SpellId);
        }
    }
}