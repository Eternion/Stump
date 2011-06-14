
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.Database.WorldServer.Character;

namespace Stump.Database.WorldServer.Job
{
    [ActiveRecord("characters_jobs")]
    public sealed class JobRecord : WorldBaseRecord<JobRecord>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "Id")]
        public uint Id { get; set; }

        [BelongsTo("CharacterId", NotNull = true)]
        public CharacterRecord Character { get; set; }

        [Property("JobId", NotNull = true)]
        public uint JobId { get; set; }

        [Property("Experience", NotNull = true, Default = "0")]
        public uint Experience { get; set; }
    }
}