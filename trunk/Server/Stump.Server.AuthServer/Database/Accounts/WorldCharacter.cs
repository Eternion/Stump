using System;
using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;

namespace Stump.Server.AuthServer.Database
{
    public class WorldCharacterConfiguration : EntityTypeConfiguration<WorldCharacter>
    {
        public WorldCharacterConfiguration()
        {
            ToTable("worlds_characters");
        }
    }

    public partial class WorldCharacter
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