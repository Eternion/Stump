
using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.Database.Types;
using Stump.Database.WorldServer.BidHouse;
using Stump.Database.WorldServer.House;
using Stump.Database.WorldServer.Paddock;
using Stump.Database.WorldServer.StartupAction;
using Stump.Database.WorldServer.Storage;

namespace Stump.Database.WorldServer
{
    [Serializable]
    [ActiveRecord("accounts")]
    public sealed class WorldAccountRecord : WorldBaseRecord<WorldAccountRecord>
    {
        private IList<BidHouseItemRecord> m_bidhouseItems;
        private IList<WorldAccountRecord> m_enemies;
        private IList<WorldAccountRecord> m_friends;
        private IList<MountRecord> m_mounts;
        private IList<StartupActionRecord> m_startupActions;

        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public uint Id { get; set; }

        [Property("Nickname", NotNull = true)]
        public string Nickname { get; set; }

        [Property("LastConnection", NotNull = false)]
        public DateTime LastConnection { get; set; }

        [Property("LastIp", NotNull = false, Length = 15)]
        public string LastIp { get; set; }

        [Property("BanEndDate")]
        public DateTime? BanEndDate { get; set; }

        public TimeSpan BanRemainingTime
        {
            get
            {
                if (BanEndDate.HasValue)
                {
                    TimeSpan date = BanEndDate.Value.Subtract(DateTime.Now);
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

        [HasAndBelongsToMany(typeof (StartupActionRecord), Table = "accounts_startup_actions", ColumnKey = "AccountId",
            ColumnRef = "StartupActionId", Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<StartupActionRecord> StartupActions
        {
            get { return m_startupActions ?? new List<StartupActionRecord>(); }
            set { m_startupActions = value; }
        }

        [HasAndBelongsToMany(typeof (WorldAccountRecord), Table = "accounts_friends", ColumnKey = "AccountId",
            ColumnRef = "FriendAccountId")]
        public IList<WorldAccountRecord> Friends
        {
            get { return m_friends ?? new List<WorldAccountRecord>(); }
            set { m_friends = value; }
        }

        [HasAndBelongsToMany(typeof (WorldAccountRecord), Table = "accounts_enemies", ColumnKey = "AccountId",
            ColumnRef = "EnemyAccountId")]
        public IList<WorldAccountRecord> Enemies
        {
            get { return m_enemies ?? new List<WorldAccountRecord>(); }
            set { m_enemies = value; }
        }

        [HasMany(typeof (BidHouseItemRecord))]
        public IList<BidHouseItemRecord> BidHousesItems
        {
            get { return m_bidhouseItems ?? new List<BidHouseItemRecord>(); }
            set { m_bidhouseItems = value; }
        }

        [HasMany(typeof (MountRecord), Table = "stables_mounts", ColumnKey = "AccountId")]
        public IList<MountRecord> Mounts
        {
            get { return m_mounts ?? new List<MountRecord>(); }
            set { m_mounts = value; }
        }

        [BelongsTo("HouseId", NotNull = false, NotFoundBehaviour = NotFoundBehaviour.Exception)]
        public HouseRecord House { get; set; }

        [BelongsTo("InventoryId", NotNull = true, NotFoundBehaviour = NotFoundBehaviour.Exception)]
        public InventoryRecord Bank { get; set; }

        public uint LastConnectionTimeStamp
        {
            get { return (uint) DateTime.Now.Subtract(LastConnection).TotalHours; }
        }

        public bool IsRevertFriend(WorldAccountRecord account)
        {
            if (Friends.Contains(account) && account.Friends.Contains(this))
                return true;
            return false;
        }


        public static WorldAccountRecord FindById(uint id)
        {
            return FindByPrimaryKey(id);
        }

        public static WorldAccountRecord FindByNickname(string nickname)
        {
            return FindOne(Restrictions.Eq("Nickname", nickname));
        }

        public static bool Exists(string nickname)
        {
            return Exists(Restrictions.Eq("Nickname", nickname));
        }
    }
}