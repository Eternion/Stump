using System;
using System.Net;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.AuthServer.Database
{
    public class IpBanRelator
    {
        public static string FetchQuery = "SELECT * FROM ipbans";
        public static string FindByIP = "SELECT * FROM ipbans WHERE IPAsString={0}";
    }

    [TableName("ipbans")]
    public partial class IpBan : IAutoGeneratedRecord
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

        /// <summary>
        /// Duration in minutes
        /// </summary>
        public int? Duration
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

        [Ignore]
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
            return LifeBan ? DateTime.MaxValue : Date.AddMinutes(Duration.Value);
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