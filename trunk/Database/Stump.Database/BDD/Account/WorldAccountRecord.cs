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
using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.DofusProtocol.Enums;

namespace Stump.Database
{
    [Serializable]
    [AttributeDatabase(DatabaseService.AuthServer)]
    [ActiveRecord("accounts")]
    public sealed class WorldAccountRecord : ActiveRecordBase<WorldAccountRecord>
    {

        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [Property("LastConnection", NotNull = false)]
        public DateTime LastConnection
        {
            get;
            set;
        }

        [Property("LastIp", NotNull = false, Length = 15)]
        public string LastIp
        {
            get;
            set;
        }

        [Property("BankKamas", NotNull = false, Default="0")]
        public uint BankKamas
        {
            get;
            set;
        }

        [HasAndBelongsToMany(typeof(StartupActionRecord), Table = "Accounts_StartupActions", ColumnKey = "AccountId", ColumnRef = "StartupActionId")]
        public IList<WorldAccountRecord> StartupActions
        {
            get;
            set;
        }

        [HasMany(typeof(ItemRecord))]
        public IList<ItemRecord> BankItems
        {
            get;
            set;
        }

        //[HasMany(typeof(WorldAccountRecord))]
        //public IList<ItemRecord> Friends
        //{
        //    get;
        //    set;
        //}

        //[HasMany(typeof(EnemyRecord))]
        //public IList<ItemRecord> Enemies
        //{
        //    get;
        //    set;
        //}


    }
}