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
using Stump.Database.AuthServer.World;
using Stump.Database.Types;

namespace Stump.Database.AuthServer
{
    [ActiveRecord("connections")]
    public class ConnectionRecord : AuthBaseRecord<ConnectionRecord>
    {
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

        [Property("Ip", NotNull = false, Length = 15)]
        public string Ip
        {
            get;
            set;
        }

        [BelongsTo("World")]
        public WorldRecord World
        {
            get;
            set;
        }

    }
}