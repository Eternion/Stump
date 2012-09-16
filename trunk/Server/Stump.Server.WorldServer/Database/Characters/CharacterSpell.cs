using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Spells;

namespace Stump.Server.WorldServer.Database
{
    public class CharacterSpellConfiguration : EntityTypeConfiguration<CharacterSpell>
    {
        public CharacterSpellConfiguration()
        {
            ToTable("characters_spells");
        }
    }

    public class CharacterSpell : ISpellRecord
    {
        // Primitive properties
    
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int SpellId { get; set; }
        public sbyte Level { get; set; }
        public byte Position { get; set; }
    
        // Navigation properties
    
        public virtual CharacterRecord Character { get; set; }

        public override string ToString()
        {
            return (SpellIdEnum) SpellId + " (" + SpellId + ")";
        }
    }
}