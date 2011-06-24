using System;
using Castle.ActiveRecord;

namespace Stump.Server.AuthServer.Database.Account
{
    [ActiveRecord("subscriptions")]
    public class SubscriptionLog : AuthBaseRecord<SubscriptionLog>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [BelongsTo("Account")]
        public Account Account
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