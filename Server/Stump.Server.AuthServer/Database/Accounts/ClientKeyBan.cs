﻿using System;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.AuthServer.Database.Accounts
{
    public class ClientKeyBanRelator
    {
        public static string FetchQuery = "SELECT * FROM clientbans";
        public static string FindByIP = "SELECT * FROM clientbans WHERE ClientKey={0}";
    }

    [TableName("clientbans")]
    public partial class ClientKeyBan : IAutoGeneratedRecord
    {
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

        public String ClientKey
        {
            get;
            set;
        }

        public DateTime GetEndDate()
        {
            return LifeBan ? DateTime.MaxValue : Date.AddMinutes(Duration.Value);
        }

        public TimeSpan GetRemainingTime()
        {
            var endDate = GetEndDate();
            var remainingTime = (endDate - DateTime.Now);

            return remainingTime;
        }
    }
}