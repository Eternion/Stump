
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.WorldServer.Paddock;

namespace Stump.Database.WorldServer.Guild
{
    [ActiveRecord("guilds_paddocks")]
    public sealed class GuildPaddockRecord : PaddockRecord
    {
        private IList<PaddockItemRecord> m_paddockItems;

        [JoinedKey("PaddockId")]
        public uint PaddockId { get; set; }

        [BelongsTo("GuildId", NotNull = true)]
        public GuildRecord Guild { get; set; }


        [HasMany(typeof (PaddockItemRecord))]
        public IList<PaddockItemRecord> PaddockItems
        {
            get { return m_paddockItems ?? new List<PaddockItemRecord>(); }
            set { m_paddockItems = value; }
        }
    }
}