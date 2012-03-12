using Castle.ActiveRecord;

namespace Stump.Server.WorldServer.Database.Characters
{
    [ActiveRecord("experiences")]
    public class ExperienceRecord : WorldBaseRecord<ExperienceRecord>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "Level")]
        public byte Level
        {
            get;
            set;
        }

        [Property("CharacterExp", NotNull = false)]
        public long? CharacterExp
        {
            get;
            set;
        }

        [Property("GuildExp", NotNull = false)]
        public long? GuildExp
        {
            get;
            set;
        }

        [Property("MountExp", NotNull = false)]
        public long? MountExp
        {
            get;
            set;
        }

        [Property("AlignmentHonor", NotNull = false)]
        public ushort? AlignmentHonor
        {
            get;
            set;
        }
    }
}