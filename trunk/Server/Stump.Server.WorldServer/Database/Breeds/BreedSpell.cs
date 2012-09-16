using System.Data.Entity.ModelConfiguration;

namespace Stump.Server.WorldServer.Database
{
    public class BreedSpellConfiguration : EntityTypeConfiguration<BreedSpell>
    {
        public BreedSpellConfiguration()
        {
            ToTable("breeds_spells");
        }
    }

    public partial class BreedSpell
    {
        // Primitive properties

        public int Id
        {
            get;
            set;
        }

        public int? Spell
        {
            get;
            set;
        }

        public int? ObtainLevel
        {
            get;
            set;
        }

        public int BreedId
        {
            get;
            set;
        }

        // Navigation properties

        public virtual Breed Breed
        {
            get;
            set;
        }
    }
}