using System;
using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using NHibernate.Criterion;

namespace Stump.Server.AuthServer.Database
{
    public class WorldCharacterDeletedConfiguration : EntityTypeConfiguration<WorldCharacterDeleted>
    {
        public WorldCharacterDeletedConfiguration()
        {
            ToTable("worlds_characters_deleted");
        }
    }

    public partial class WorldCharacterDeleted
    {
        // Primitive properties

        public long Id
        {
            get;
            set;
        }
        public int CharacterId
        {
            get;
            set;
        }
        public System.DateTime DeletionDate
        {
            get;
            set;
        }
        public int AccountId
        {
            get;
            set;
        }
        public int WorldId
        {
            get;
            set;
        }

        // Navigation properties

        public virtual Account Account
        {
            get;
            set;
        }
    }
}