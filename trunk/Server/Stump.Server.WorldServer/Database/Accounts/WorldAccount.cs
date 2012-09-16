using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;

namespace Stump.Server.WorldServer.Database
{
    public class WorldAccountConfiguration : EntityTypeConfiguration<WorldAccount>
    {
        public WorldAccountConfiguration()
        {
            ToTable("accounts");
            HasMany(x => x.Friends).WithMany(x => x.Followers).Map(x => x.MapLeftKey("AccountId").MapRightKey("FriendAccountId").ToTable("accounts_friends"));
            HasMany(x => x.IgnoredAccounts).WithMany(x => x.IgnoredBy).Map(x => x.MapLeftKey("AccountId").MapRightKey("IgnoredAccountId").ToTable("accounts_ignoreds"));
            HasMany(x => x.StartupActions).WithMany().Map(x => x.MapLeftKey("AccountId").MapRightKey("StartupActionId").ToTable("accounts_startup_actions"));
        }
    }

    public partial class WorldAccount
    {
        public WorldAccount()
        {
            Followers = new HashSet<WorldAccount>();
            Friends = new HashSet<WorldAccount>();
            IgnoredBy = new HashSet<WorldAccount>();
            IgnoredAccounts = new HashSet<WorldAccount>();
            StartupActions = new HashSet<StartupAction>();
        }

        // Primitive properties

        public int Id
        {
            get;
            set;
        }

        public string Nickname
        {
            get;
            set;
        }

        public DateTime? LastConnection
        {
            get;
            set;
        }

        public string LastIp
        {
            get;
            set;
        }

        public int? ConnectedCharacter
        {
            get;
            set;
        }

        // Navigation properties

        public virtual ICollection<WorldAccount> Followers
        {
            get;
            set;
        }

        public virtual ICollection<WorldAccount> Friends
        {
            get;
            set;
        }

        public virtual ICollection<WorldAccount> IgnoredBy
        {
            get;
            set;
        }

        public virtual ICollection<WorldAccount> IgnoredAccounts
        {
            get;
            set;
        }

        public virtual ICollection<StartupAction> StartupActions
        {
            get;
            set;
        }

        public int LastConnectionTimeStamp
        {
            get { return LastConnection.HasValue ? (int) (DateTime.Now - LastConnection.Value).TotalHours : 0; }
        }

        public StartupAction[] GetStartupActions()
        {
            return StartupActions.Select(entry => new StartupAction(entry)).ToArray();
        }
    }
}