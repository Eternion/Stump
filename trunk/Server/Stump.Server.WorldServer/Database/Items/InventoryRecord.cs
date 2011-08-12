using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Server.WorldServer.Database.Characters;

namespace Stump.Server.WorldServer.Database.Items
{
    [ActiveRecord("inventories")]
    public sealed class InventoryRecord : WorldBaseRecord<InventoryRecord>
    {
        private IList<ItemRecord> m_items;

        public InventoryRecord()
        {
            
        }

        public InventoryRecord(CharacterRecord character)
        {
            Owner = character;
        }

        [PrimaryKey(PrimaryKeyType.Foreign, "Id")]
        public int Id
        {
            get;
            set;
        }

        [OneToOne]
        public CharacterRecord Owner
        {
            get;
            set;
        }

        [Property("Kamas", NotNull = true, Default = "0")]
        public int Kamas
        {
            get;
            set;
        }

        [HasMany(typeof (ItemRecord), Table = "items", ColumnKey = "InventoryId",
            Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<ItemRecord> Items
        {
            get { return m_items ?? new List<ItemRecord>(); }
            set { m_items = value; }
        }
    }
}