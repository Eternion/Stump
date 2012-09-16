using System;
using System.ComponentModel;
using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using NHibernate.Criterion;

namespace Stump.Server.WorldServer.Database
{
    public class StartupActionItemRecordConfiguration : EntityTypeConfiguration<StartupActionItemRecord>
    {
        public StartupActionItemRecordConfiguration()
        {
            ToTable("startup_actions_items");
        }
    }

    public sealed class StartupActionItemRecord
    {
        public uint Id
        {
            get;
            set;
        }

        public int ItemTemplate
        {
            get;
            set;
        }

        public int Amount
        {
            get;
            set;
        }

        [DefaultValue(true)]
        public bool MaxEffects
        {
            get;
            set;
        }
    }
}