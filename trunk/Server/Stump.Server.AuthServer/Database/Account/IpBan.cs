using System;
using Castle.ActiveRecord;
using NHibernate.Criterion;

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

        public DateTime EndDate
        {
            get
            {
                return Duration.Ticks == long.MaxValue ? DateTime.MaxValue : Date.Add(Duration);
            }
        }

        public uint BanRemainingTime
        {
            get
            {
                var remainingTime = ( EndDate - DateTime.Now ).TotalMinutes;

                if (remainingTime < 0)
                    return 0;

                return (uint)remainingTime;
            }
        }

        public static IpBan FindByIp(string ip)
        {
            return FindOne(Restrictions.Eq("Ip", ip.ToLower()));
        }
    }
}