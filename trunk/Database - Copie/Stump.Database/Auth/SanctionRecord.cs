
using System;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.Database.Types;

namespace Stump.Database.AuthServer
{
    [ActiveRecord("sanctions")]
    public class SanctionRecord : AuthBaseRecord<SanctionRecord>
    {
        public SanctionRecord()
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
        public AccountRecord Account
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
        public AccountRecord BannedBy
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