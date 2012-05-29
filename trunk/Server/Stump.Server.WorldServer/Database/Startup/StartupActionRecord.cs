using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Accounts;

namespace Stump.Server.WorldServer.Database.Startup
{
    [Serializable]
    [ActiveRecord("startup_actions")]
    public sealed class StartupActionRecord : WorldBaseRecord<StartupActionRecord>
    {
        private IList<WorldAccount> m_accounts;
        private IList<StartupActionItemRecord> m_items;

        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public int Id
        {
            get;
            set;
        }

        [Property("Title", NotNull = true, Length = 25)]
        public string Title
        {
            get;
            set;
        }

        [Property("Text", NotNull = true, Length = 250)]
        public string Text
        {
            get;
            set;
        }

        [Property("DescUrl", NotNull = true, Length = 50)]
        public string DescUrl
        {
            get;
            set;
        }

        [Property("PictureUrl", NotNull = true, Length = 50)]
        public string PictureUrl
        {
            get;
            set;
        }

        [HasMany(typeof (StartupActionItemRecord), Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<StartupActionItemRecord> Items
        {
            get { return m_items ?? new List<StartupActionItemRecord>(); }
            set { m_items = value; }
        }

        [HasAndBelongsToMany(typeof (WorldAccount), Table = "accounts_startup_actions",
            ColumnKey = "StartupActionId", ColumnRef = "AccountId", Inverse = true)]
        public IList<WorldAccount> Accounts
        {
            get { return m_accounts ?? new List<WorldAccount>(); }
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