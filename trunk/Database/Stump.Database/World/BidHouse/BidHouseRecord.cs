
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.WorldServer.BidHouse
{
    [ActiveRecord("bidhouses")]
    public sealed class BidHouseRecord : WorldBaseRecord<BidHouseRecord>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "BidHouseId")]
        public uint BidHouseId { get; set; }

        [HasMany(typeof (BidHouseItemRecord))]
        public IList<BidHouseItemRecord> Items { get; set; }

        [HasMany(typeof (BidHouseSoldItemRecord))]
        public IList<BidHouseSoldItemRecord> SoldItems { get; set; }
    }
}