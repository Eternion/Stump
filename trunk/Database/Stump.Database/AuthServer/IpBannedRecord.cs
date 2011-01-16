// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using Castle.ActiveRecord;
using NHibernate.Criterion;

namespace Stump.Database
{

    [AttributeDatabase(DatabaseService.AuthServer)]
    [ActiveRecord("ip_banned")]
    public class IpBannedRecord : ActiveRecordBase<IpBannedRecord>
    {

        [PrimaryKey(PrimaryKeyType.Native, "ip", Length=15)]
        public string Ip
        {
            get;
            set;
        }

        [Property("bandate")]
        public DateTime BanDate
        {
            get;
            set;
        }

        [Property("unbandate")]
        public DateTime UnbanDate
        {
            get;
            set;
        }

        [Property("bannedby")]
        public string BannedBy
        {
            get;
            set;
        }

        [Property("banreason")]
        public string BanReason
        {
            get;
            set;
        }

        public static bool Exists(string ip)
        {
            return Exists(Restrictions.Eq("Ip", ip));
        }

        public static void UpdateAll()
        {
            //TODO Delete all
        }

    }
}