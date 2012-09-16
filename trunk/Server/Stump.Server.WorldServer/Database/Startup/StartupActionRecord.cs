using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using NHibernate.Criterion;

namespace Stump.Server.WorldServer.Database
{
    public class StartupActionRecordConfiguration : EntityTypeConfiguration<StartupActionRecord>
    {
        public StartupActionRecordConfiguration()
        {
            ToTable("startup_actions");
            HasMany(x => x.Items);
        }
    }
    public sealed class StartupActionRecord : WorldBaseRecord<StartupActionRecord>
    {
        private IList<WorldAccount> m_accounts;
        private IList<StartupActionItemRecord> m_items;

        public int Id
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Text
        {
            get;
            set;
        }

        public string DescUrl
        {
            get;
            set;
        }

        public string PictureUrl
        {
            get;
            set;
        }

        public IList<StartupActionItemRecord> Items
        {
            get { return m_items ?? new List<StartupActionItemRecord>(); }
            set { m_items = value; }
        }
    }
}