using System;
using System.Data.Entity.ModelConfiguration;

namespace Stump.Server.AuthServer.Database
{
    public class SanctionConfiguration : EntityTypeConfiguration<Sanction>
    {
        public SanctionConfiguration()
        {
            ToTable("accounts_sanctions");
        }
    }

    public partial class Sanction
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

        public TimeSpan? Duration
        {
            get;
            set;
        }

        public string BanReason
        {
            get;
            set;
        }

        public int AccountId
        {
            get;
            set;
        }

        public int? BannedBy
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

        public bool LifeBan
        {
            get { return !Duration.HasValue; }
        }

        public DateTime GetEndDate()
        {
            return LifeBan ? DateTime.MaxValue : Date.Add(Duration.Value);
        }

        public TimeSpan GetRemainingTime()
        {
            DateTime endDate = GetEndDate();
            TimeSpan remainingTime = (endDate - DateTime.Now);

            return remainingTime;
        }
    }
}