using System;
using Castle.ActiveRecord;

namespace Stump.Server.AuthServer.Database.Account
{
    [ActiveRecord("sanctions")]
    public class Sanction : AuthBaseRecord<Sanction>
    {
        public Sanction()
        {
            Date = DateTime.Now;
        }

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

        [Property("Date")]
        public DateTime Date
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

        [BelongsTo("BannedBy")]
        public Account BannedBy
        {
            get;
            set;
        }

        [Property("BanReason")]
        public string BanReason
        {
            get;
            set;
        }

        public DateTime EndDate
        {
            get { return Date.Add(Duration); }
        }
    }
}