
using System;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.Database.Types;

namespace Stump.Database.AuthServer
{
    [ActiveRecord("subscriptions")]
    public class SubscriptionRecord : AuthBaseRecord<SubscriptionRecord>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [BelongsTo("Account")]
        public AccountRecord Account
        {
            get;
            set;
        }

        [Property("BuyDate")]
        public DateTime BuyDate
        {
            get;
            set;
        }

        [Property("Duration")]
        public TimeSpan Duration
        {
            get;
            set;
        }

        [Property("PaymentType")]
        public string PaymentType
        {
            get;
            set;
        }

        public DateTime EndDate
        {
            get { return BuyDate.Add(Duration); }
        }
    }
}