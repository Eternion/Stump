
using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.Database.AuthServer;
using Stump.Database.Types;

namespace Stump.Database.WorldServer.StartupAction
{
    [Serializable]
    [ActiveRecord("startup_actions")]
    public sealed class StartupActionRecord : WorldBaseRecord<StartupActionRecord>
    {
        private IList<WorldAccountRecord> m_accounts;
        private IList<StartupActionItemRecord> m_items;

        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id { get; set; }

        [Property("Title", NotNull = true, Length = 25)]
        public string Title { get; set; }

        [Property("Text", NotNull = true, Length = 250)]
        public string Text { get; set; }

        [Property("DescUrl", NotNull = true, Length = 50)]
        public string DescUrl { get; set; }

        [Property("PictureUrl", NotNull = true, Length = 50)]
        public string PictureUrl { get; set; }

        [HasMany(typeof (StartupActionItemRecord), Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<StartupActionItemRecord> Items
        {
            get { return m_items ?? new List<StartupActionItemRecord>(); }
            set { m_items = value; }
        }

        [HasAndBelongsToMany(typeof (WorldAccountRecord), Table = "accounts_startup_actions",
            ColumnKey = "StartupActionId", ColumnRef = "AccountId", Inverse = true)]
        public IList<WorldAccountRecord> Accounts
        {
            get { return m_accounts ?? new List<WorldAccountRecord>(); }
            set { m_accounts = value; }
        }


        public static StartupActionRecord FindStartupActionById(uint id)
        {
            return FindByPrimaryKey(id);
        }

        public static StartupActionRecord[] FindStartupActionByTitle(string title)
        {
            return FindAll(Restrictions.Eq("Title", title));
        }
    }
}