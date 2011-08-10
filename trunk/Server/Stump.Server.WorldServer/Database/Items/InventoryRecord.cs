using System.Collections.Generic;
using Castle.ActiveRecord;

namespace Stump.Server.WorldServer.Database.Items
{
    [ActiveRecord("inventories")]
    public sealed class InventoryRecord : WorldBaseRecord<InventoryRecord>
    {
        private IList<ItemRecord> m_items;

        [PrimaryKey(PrimaryKeyType.Identity, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [Property("Kamas", NotNull = true, Default = "0")]
        public long Kamas
        {
            get;
            set;
        }

        [HasMany(typeof (ItemRecord), Table = "inventories_items", ColumnKey = "InventoryId",
            Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<ItemRecord> Items
        {
            get { return m_items ?? new List<ItemRecord>(); }
            set { m_items = value; }
        }
    }
}