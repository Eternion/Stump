using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Database.Startup;

namespace Stump.Server.WorldServer.Database.Accounts
{
    [Serializable]
    [ActiveRecord("accounts")]
    public class WorldAccount : WorldBaseRecord<WorldAccount>
    {
        private IList<WorldAccount> m_enemies;
        private IList<WorldAccount> m_friends;
        private IList<StartupActionRecord> m_startupActions;

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

        public uint LastConnectionTimeStamp
        {
            get { return (uint) DateTime.Now.Subtract(LastConnection).TotalHours; }
        }

        [Property("LastIp", NotNull = false, Length = 28)]
        public string LastIp
        {
            get;
            set;
        }

        [HasAndBelongsToMany(typeof (StartupActionRecord), Table = "accounts_startup_actions", ColumnKey = "AccountId",
            ColumnRef = "StartupActionId", Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<StartupActionRecord> StartupActions
        {
            get { return m_startupActions ?? new List<StartupActionRecord>(); }
            set { m_startupActions = value; }
        }

        [HasAndBelongsToMany(typeof (WorldAccount), Table = "accounts_friends", ColumnKey = "AccountId",
            ColumnRef = "FriendAccountId")]
        public IList<WorldAccount> Friends
        {
            get { return m_friends ?? new List<WorldAccount>(); }
            set { m_friends = value; }
        }

        [HasAndBelongsToMany(typeof (WorldAccount), Table = "accounts_enemies", ColumnKey = "AccountId",
            ColumnRef = "EnemyAccountId")]
        public IList<WorldAccount> Enemies
        {
            get { return m_enemies ?? new List<WorldAccount>(); }
            set { m_enemies = value; }
        }

        public bool IsRevertFriend(WorldAccount account)
        {
            return Friends.Contains(account) && account.Friends.Contains(this);
        }

        public static WorldAccount FindById(uint id)
        {
            return FindByPrimaryKey(id);
        }

        public static WorldAccount FindByNickname(string nickname)
        {
            return FindOne(Restrictions.Eq("Nickname", nickname));
        }

        public static bool Exists(string nickname)
        {
            return Exists(Restrictions.Eq("Nickname", nickname));
        }
    }
}