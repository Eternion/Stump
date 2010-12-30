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
    [AttributeDatabase(DatabaseService.WorldServer)]
    [ActiveRecord("accounts")]
    public sealed class WorldAccountRecord : ActiveRecordBase<WorldAccountRecord>
    {

        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [Property("Nickname", NotNull = true)]
        public string Nickname
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

        [HasAndBelongsToMany(typeof(WorldAccountRecord), Table = "accounts_friends", ColumnKey = "AccountId", ColumnRef = "FriendAccountId")]
        public IList<WorldAccountRecord> Friends
        {
            get;
            set;
        }

        [HasAndBelongsToMany(typeof(WorldAccountRecord), Table = "accounts_enemies", ColumnKey = "AccountId", ColumnRef = "EnemyAccountId")]
        public IList<WorldAccountRecord> Enemies
        {
            get;
            set;
        }

        [HasAndBelongsToMany(typeof(WorldAccountRecord), Table = "accounts_ignored", ColumnKey = "AccountId", ColumnRef = "IgnoredAccountId")]
        public IList<WorldAccountRecord> Ignored
        {
            get;
            set;
        }


        public uint LastConnectionTimeStamp
        {
            get
            {
                return (uint)LastConnection.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMinutes;          
            }
        }

        public bool IsRevertFriend(WorldAccountRecord account)
        {
            if (Friends.Contains(account) && account.Friends.Contains(this))
                return true;
            return false;
        }


        public static WorldAccountRecord FindWorldAccountById(uint id)
        {
            return FindByPrimaryKey(id);
        }

        public static WorldAccountRecord FindWorldAccountByNickname(string nickname)
        {
            return FindOne(Restrictions.Eq("Nickname", nickname));
        }

        public static bool Exists(string nickname)
        {
            return Exists(Restrictions.Eq("Nickname", nickname));
        }
    }
}