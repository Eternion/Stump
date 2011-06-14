
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.WorldServer.BidHouse
{
    [ActiveRecord("bidhouses_solditems")]
    public sealed class BidHouseSoldItemRecord : WorldBaseRecord<BidHouseSoldItemRecord>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "Id")]
        public long Id { get; set; }

        [BelongsTo("BidHouseId", NotNull = true)]
        public BidHouseRecord BidHouse { get; set; }

        [Property("ItemId", NotNull = true)]
        public uint ItemId { get; set; }

        [Property("SoldPrice", NotNull = true)]
        public uint SoldPrice { get; set; }
    }
}