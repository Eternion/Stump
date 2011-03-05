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
using Stump.Database.Types;

namespace Stump.Database.AuthServer
{
    [ActiveRecord("banned_ips")]
    public class IpBannedRecord : AuthRecord<IpBannedRecord>
    {

        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [Property("Ip", Length=15)]
        public string Ip
        {
            get;
            set;
        }

        [Property("BanDate")]
        public DateTime BanDate
        {
            get;
            set;
        }

        [Property("UnbanDate")]
        public DateTime UnbanDate
        {
            get;
            set;
        }

        [Property("BannedBy")]
        public string BannedBy
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


        public static bool Exists(string ip)
        {
            return Exists(Restrictions.Eq("Ip", ip));
        }
    }
}