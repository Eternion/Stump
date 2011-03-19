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
    [ActiveRecord("sanctions")]
    public class SanctionRecord : AuthBaseRecord<SanctionRecord>
    {
        public SanctionRecord()
        {
            Date = DateTime.Now;
        }

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

        [Property("Duration")]
        public TimeSpan Duration
        {
            get;
            set;
        }

        [BelongsTo("BannedBy")]
        public AccountRecord BannedBy
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
            get { return Date.Add(Duration); }
        }
    }
}