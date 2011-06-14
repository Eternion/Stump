
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.Database.WorldServer.Item;

namespace Stump.Database.WorldServer.Paddock
{
    [ActiveRecord("paddocks"), JoinedBase]
    public class PaddockRecord : WorldBaseRecord<PaddockRecord>
    {
        private IList<ItemRecord> m_items;
        private IList<MountRecord> m_mounts;

        [PrimaryKey(PrimaryKeyType.Assigned, "PaddockId")]
        public uint PaddockId { get; set; }

        [HasMany(typeof (MountRecord), Table = "paddocks_mount", ColumnKey = "PaddockId",
            Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<MountRecord> Mounts
        {
            get { return m_mounts ?? new List<MountRecord>(); }
            set { m_mounts = value; }
        }

        [HasMany(typeof (ItemRecord), Table = "paddocks_items", ColumnKey = "PaddockId",
            Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<ItemRecord> Items
        {
            get { return m_items ?? new List<ItemRecord>(); }
            set { m_items = value; }
        }
    }
}