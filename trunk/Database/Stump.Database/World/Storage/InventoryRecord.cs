
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.Database.WorldServer.Item;

namespace Stump.Database.WorldServer.Storage
{
    [ActiveRecord("inventories")]
    public sealed class InventoryRecord : WorldBaseRecord<InventoryRecord>
    {
        private IList<ItemRecord> m_items;

        [PrimaryKey(PrimaryKeyType.Identity, "Id")]
        public uint Id { get; set; }

        [Property("Kamas", NotNull = true, Default = "0")]
        public long Kamas { get; set; }

        [Property("Capacity", NotNull = true, Default = "1000")]
        public uint Capacity { get; set; }

        [HasMany(typeof (ItemRecord), Table = "inventories_items", ColumnKey = "InventoryId",
            Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<ItemRecord> Items
        {
            get { return m_items ?? new List<ItemRecord>(); }
            set { m_items = value; }
        }
    }
}