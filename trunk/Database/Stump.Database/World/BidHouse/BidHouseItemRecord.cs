
using System;
using Castle.ActiveRecord;
using Stump.Database.WorldServer.Item;

namespace Stump.Database.WorldServer.BidHouse
{
    [ActiveRecord("bidhouses_items")]
    public sealed class BidHouseItemRecord : ItemRecord
    {
        public BidHouseItemRecord()
        {
            PushDate = DateTime.Now;
        }

        [JoinedKey("ItemGuid")]
        private long ItemGuid { get; set; }

        [BelongsTo("AccountId", NotNull = true)]
        public WorldAccountRecord Account { get; set; }

        [BelongsTo("BidHouseId", NotNull = true)]
        public BidHouseRecord BidHouse { get; set; }

        [Property("Price", NotNull = true)]
        public uint Price { get; set; }

        [Property("PushDate", NotNull = true)]
        public DateTime PushDate { get; set; }
    }
}