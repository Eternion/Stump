
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.WorldServer.Storage
{
    [ActiveRecord("storages")]
    public class StorageRecord : WorldBaseRecord<StorageRecord>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "StorageId")]
        public uint StorageId { get; set; }

        [Property("Password", NotNull = false)]
        public string Password { get; set; }

        [BelongsTo("InventoryId", NotNull = false)]
        public InventoryRecord Inventory { get; set; }
    }
}