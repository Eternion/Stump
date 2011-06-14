
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.WorldServer.Guild
{
    [ActiveRecord("guilds")]
    public sealed class GuildRecord : WorldBaseRecord<GuildRecord>
    {
        private IList<CollectorRecord> m_collectors;
        private IList<GuildHouseRecord> m_houses;
        private IList<GuildMemberRecord> m_members;
        private IList<GuildPaddockRecord> m_paddocks;

        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id { get; set; }

        [Property("Name", NotNull = true)]
        public string Name { get; set; }

        [OneToOne(Cascade = CascadeEnum.All)]
        public GuildEmblemRecord Emblem { get; set; }

        [Property("Experience", NotNull = true, Default = "0")]
        public long Experience { get; set; }


        [HasMany(typeof (GuildMemberRecord))]
        public IList<GuildMemberRecord> Members
        {
            get { return m_members ?? new List<GuildMemberRecord>(); }
            set { m_members = value; }
        }

        [HasMany(typeof (GuildHouseRecord))]
        public IList<GuildHouseRecord> Houses
        {
            get { return m_houses ?? new List<GuildHouseRecord>(); }
            set { m_houses = value; }
        }

        [HasMany(typeof (GuildPaddockRecord))]
        public IList<GuildPaddockRecord> Paddocks
        {
            get { return m_paddocks ?? new List<GuildPaddockRecord>(); }
            set { m_paddocks = value; }
        }

        [HasMany(typeof (CollectorRecord))]
        public IList<CollectorRecord> TaxCollectors
        {
            get { return m_collectors ?? new List<CollectorRecord>(); }
            set { m_collectors = value; }
        }
    }
}