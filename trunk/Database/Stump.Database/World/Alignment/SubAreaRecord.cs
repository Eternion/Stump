
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.WorldServer.Alignment
{
    [ActiveRecord("subareas")]
    public sealed class SubAreaRecord : WorldBaseRecord<SubAreaRecord>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "SubAreaId")]
        public int SubAreaId { get; set; }

        [Property("AlignmentSide", NotNull = true, Default = "0")]
        public int AlignmentSide { get; set; }

        [OneToOne(Cascade = CascadeEnum.Delete)]
        public PrismRecord Prism { get; set; }
    }
}