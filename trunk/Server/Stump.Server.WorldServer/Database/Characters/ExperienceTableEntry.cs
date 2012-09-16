using System.Data.Entity.ModelConfiguration;

namespace Stump.Server.WorldServer.Database
{
    public class ExperienceTableEntryConfiguration : EntityTypeConfiguration<ExperienceTableEntry>
    {
        public ExperienceTableEntryConfiguration()
        {
            ToTable("experiences");
        }
    }

    public class ExperienceTableEntry
    {
        // Primitive properties

        public byte Level
        {
            get;
            set;
        }

        public long? CharacterExp
        {
            get;
            set;
        }

        public long? GuildExp
        {
            get;
            set;
        }

        public long? MountExp
        {
            get;
            set;
        }

        public int? AlignmentHonor
        {
            get;
            set;
        }
    }
}