
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.WorldServer.Item
{
    [ActiveRecord("items_effects")]
    public sealed class ItemEffectRecord : WorldBaseRecord<ItemEffectRecord>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public long Id { get; set; }

        [BelongsTo("ItemId", NotNull = true)]
        public ItemRecord Item { get; set; }

        [Property("Effect", NotNull = true)]
        public byte Effect { get; set; }
    }
}