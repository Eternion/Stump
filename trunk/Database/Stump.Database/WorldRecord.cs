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
using Stump.DofusProtocol.Enums;

namespace Stump.Database
{
    [Serializable]
    [AttributeDatabase(DatabaseService.AuthServer)]
    [ActiveRecord("worlds")]
    public sealed class WorldRecord : ActiveRecordBase<WorldRecord>
    {
        private int m_charscount=0;

        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public int Id
        {
            get;
            set;
        }

        [Property("Name", NotNull = true, Length = 25)]
        public string Name
        {
            get;
            set;
        }

        [Property("Ip", NotNull = true, Length = 25)]
        public string Ip
        {
            get;
            set;
        }

        [Property("Port", NotNull = true)]
        public ushort Port
        {
            get;
            set;
        }

        public ServerStatusEnum Status
        {
            get;
            set;
        }

        [Property("RequireSubscription", NotNull = true, Default = "0")]
        public bool RequireSubscription
        {
            get;
            set;
        }

        [Property("RequiredRole", NotNull = true, Default = "0")]
        public RoleEnum RequiredRole
        {
            get;
            set;
        }

        [Property("Completion", NotNull = true, Default = "0")]
        public int Completion
        {
            get;
            set;
        }

        [Property("ServerSelectable", NotNull = true, Default = "1")]
        public bool ServerSelectable
        {
            get;
            set;
        }

        [Property("CharCapacity", NotNull = true, Default = "1000")]
        public int CharCapacity
        {
            get;
            set;
        }

        //[Property("CharsCount", NotNull = true, Default = "0")]
        public int CharsCount
        {
            get { return m_charscount; }
            set { m_charscount = value < 0 ? 0 : value; }
        }

        public bool Connected
        {
            get;
            set;
        }

        public static WorldRecord FindWorldRecordById(int id)
        {
            return FindByPrimaryKey(id);
        }

        public static bool Exists(int id)
        {
            return Exists(Restrictions.Eq("Id", id));
        }

    }
}