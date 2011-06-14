
using Castle.ActiveRecord;
using Stump.Database.WorldServer.Storage;

namespace Stump.Database.WorldServer.Item
{
    [ActiveRecord("priceditems"), JoinedBase]
    public class PricedItemRecord : ItemRecord
    {
        [JoinedKey("ItemGuid")]
        private long ItemGuid { get; set; }

        [BelongsTo("CharacterId", NotNull = true)]
        public SellBagRecord SellBag { get; set; }

        [Property("Price")]
        public uint Price { get; set; }
    }
}