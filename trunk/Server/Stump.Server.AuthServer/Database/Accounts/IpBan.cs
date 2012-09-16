using System;
using System.Data.Entity.ModelConfiguration;
using System.Net;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.AuthServer.Database
{
    public class IpBanConfiguration : EntityTypeConfiguration<IpBan>
    {
        public IpBanConfiguration()
        {
            ToTable("ipbans");
            Ignore(x => x.IP);
        }
    }

    public partial class IpBan
    {
        private IPAddressRange m_ip;

        // Primitive properties

        private string m_ipAsString;

        public long Id
        {
            get;
            set;
        }

        public string IPAsString
        {
            get { return m_ipAsString; }
            set
            {
                m_ipAsString = value;
                m_ip = IPAddressRange.Parse(IPAsString);
            }
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

        public int? BannedBy
        {
            get;
            set;
        }

        public bool LifeBan
        {
            get { return !Duration.HasValue; }
        }

        public IPAddressRange IP
        {
            get { return m_ip; }
            set
            {
                m_ip = value;
                IPAsString = m_ip.ToString();
            }
        }

        public DateTime GetEndDate()
        {
            return LifeBan ? DateTime.MaxValue : Date.Add(Duration.Value);
        }

        public bool Match(IPAddress ip)
        {
            return IP.Match(ip);
        }

        public TimeSpan GetRemainingTime()
        {
            DateTime endDate = GetEndDate();
            TimeSpan remainingTime = (endDate - DateTime.Now);

            return remainingTime;
        }
    }
}