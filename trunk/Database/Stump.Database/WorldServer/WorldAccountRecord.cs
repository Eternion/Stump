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
        private IList<WorldAccountRecord> m_friends;
        private IList<WorldAccountRecord> m_enemies;
        private IList<BidHouseItemRecord> m_bidhouseItems;

        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
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

        [Property("BanEndDate")]
        public DateTime? BanEndDate
        {
            get;
            set;
        }

        public TimeSpan BanRemainingTime
        {
            get
            {
                if (BanEndDate.HasValue)
                {
                    var date = BanEndDate.Value.Subtract(DateTime.Now);
                    if (date.TotalSeconds <= 0)
                    {
                        BanEndDate = null;
                        SaveAndFlush();
                        return TimeSpan.Zero;
                    }
                    return date;
                }
                return TimeSpan.Zero;
            }
        }

        [HasAndBelongsToMany(typeof(WorldAccountRecord), Table = "accounts_friends", ColumnKey = "AccountId", ColumnRef = "FriendAccountId")]
        public IList<WorldAccountRecord> Friends
        {
            get { return m_friends ?? new List<WorldAccountRecord>(); }
            set { m_friends = value; }
        }

        [HasAndBelongsToMany(typeof(WorldAccountRecord), Table = "accounts_enemies", ColumnKey = "AccountId", ColumnRef = "EnemyAccountId")]
        public IList<WorldAccountRecord> Enemies
        {
            get { return m_enemies ?? new List<WorldAccountRecord>(); }
            set { m_enemies = value; }
        }

        [HasMany(typeof(BidHouseItemRecord))]
        public IList<BidHouseItemRecord> BidHousesItems
        {
            get { return m_bidhouseItems ?? new List<BidHouseItemRecord>(); }
            set { m_bidhouseItems = value; }
        }
        
        [BelongsTo("HouseId", NotNull = false, NotFoundBehaviour = NotFoundBehaviour.Exception)]
        public HouseRecord House
        {
            get;
            set;
        }

        [BelongsTo("InventoryId", NotNull = true, NotFoundBehaviour = NotFoundBehaviour.Exception)]
        public InventoryRecord Bank
        {
            get;
            set;
        }

        public uint LastConnectionTimeStamp
        {
            get
            {
                return (uint)DateTime.Now.Subtract(LastConnection).TotalHours;
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