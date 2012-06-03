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

        [Property("Title", NotNull = true)]
        public string Title
        {
            get;
            set;
        }

        [Property("Text", NotNull = true)]
        public string Text
        {
            get;
            set;
        }

        [Property("DescUrl", NotNull = true)]
        public string DescUrl
        {
            get;
            set;
        }

        [Property("PictureUrl", NotNull = true)]
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