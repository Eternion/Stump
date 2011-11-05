using System;
using Castle.ActiveRecord;

namespace Stump.Server.AuthServer.Database.Account
{
    [ActiveRecord("ipbans")]
    public class IpBan : AuthBaseRecord<IpBan>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [Property("Ip", Length = 28)]
        public string Ip
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
    }
}