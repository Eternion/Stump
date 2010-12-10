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
using Castle.ActiveRecord;
using NHibernate.Criterion;

namespace Stump.Database
{
    /// <summary>
    ///   Ban record represents an IP Banned Record, not an account banned record !
    /// </summary>
    [AttributeDatabase(DatabaseService.AuthServer)]
    [ActiveRecord("bans")]
    public class BanRecord : ActiveRecordBase<BanRecord>
    {
        /// <summary>
        ///   Account's Identifier
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        ///   IP Banned
        /// </summary>
        [Property("IP")]
        public string IP
        {
            get;
            set;
        }

        /// <summary>
        ///   Search for a record with the given ip.
        ///   Returns null if no IP is found.
        /// </summary>
        /// <param name = "ip"></param>
        /// <returns></returns>
        public static BanRecord FindByIP(string ip)
        {
            // Note that we use the property name, _not_ the column name
            return FindOne(Restrictions.Eq("IP", ip));
        }

        /// <summary>
        ///   Returns the numbers of ip banned records.
        /// </summary>
        /// <returns></returns>
        public static int GetCount()
        {
            return Count();
        }
    }
}