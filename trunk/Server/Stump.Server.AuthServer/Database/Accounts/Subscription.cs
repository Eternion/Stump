using System;
using System.Data.Entity.ModelConfiguration;

namespace Stump.Server.AuthServer.Database
{
    public class SubscriptionConfiguration : EntityTypeConfiguration<Subscription>
    {
        public SubscriptionConfiguration()
        {
            ToTable("accounts_subscriptions");
        }
    }

    public partial class Subscription
    {
        // Primitive properties

        public long Id
        {
            get;
            set;
        }

        public DateTime BuyDate
        {
            get;
            set;
        }

        public TimeSpan? Duration
        {
            get;
            set;
        }

        public string PaymentType
        {
            get;
            set;
        }

        public int AccountId
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

        public bool IsInfinite
        {
            get { return !Duration.HasValue; }
        }

        public DateTime GetEndDate()
        {
            return IsInfinite ? DateTime.MaxValue : BuyDate.Add(Duration.Value);
        }

        public TimeSpan GetRemainingTime()
        {
            DateTime endDate = GetEndDate();
            TimeSpan remainingTime = (endDate - DateTime.Now);

            if (remainingTime.Ticks < 0)
                return TimeSpan.Zero;

            return remainingTime;
        }
    }
}