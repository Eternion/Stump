
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.Database.WorldServer.Guild;

namespace Stump.Database.WorldServer.Paddock
{
    [ActiveRecord("paddocks_items"), JoinedBase]
    public class PaddockItemRecord : WorldBaseRecord<PaddockItemRecord>
    {
        private IList<MountRecord> m_mounts;

        [PrimaryKey(PrimaryKeyType.Identity, "Id")]
        public uint Id { get; set; }

        [BelongsTo("PaddockId")]
        public GuildPaddockRecord Paddock { get; set; }

        // [Property("ItemId")]
        //   public ItemRecord Item
    }
}