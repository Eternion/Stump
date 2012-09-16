

using System;
using System.Data.Entity.ModelConfiguration;

namespace Stump.Server.AuthServer.Database
{
    public class ConnectionConfiguration : EntityTypeConfiguration<Connection>
    {
        public ConnectionConfiguration()
        {
            ToTable("accounts_connections");
        }
    }

    public partial class Connection
    {
        // Primitive properties

        public long Id
        {
            get;
            set;
        }
        public DateTime Date
        {
            get;
            set;
        }
        public string Ip
        {
            get;
            set;
        }
        public int AccountId
        {
            get;
            set;
        }
        public int? WorldId
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