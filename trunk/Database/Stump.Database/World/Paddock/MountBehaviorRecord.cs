
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.WorldServer.Paddock
{
    [ActiveRecord("mounts_behaviors")]
    public sealed class MountBehaviorRecord : WorldBaseRecord<MountBehaviorRecord>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public long Id { get; set; }

        [BelongsTo("MountId", NotNull = true)]
        public MountRecord Mount { get; set; }

        [Property("Behavior", NotNull = true)]
        public byte Behavior { get; set; }
    }
}