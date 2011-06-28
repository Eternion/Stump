using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.DofusProtocol.Enums;
using Stump.Server.AuthServer.Database.Account;

namespace Stump.Server.AuthServer.Database.World
{
    [Serializable]
    [ActiveRecord("worlds")]
    public sealed class WorldServer : AuthBaseRecord<WorldServer>
    {
        private IList<WorldCharacter> m_characters;
        private IList<DeletedWorldCharacter> m_deletedCharacters;
        private IList<ConnectionLog> m_connections;
        private int m_charsCount;

        public WorldServer()
        {
            Status = ServerStatusEnum.OFFLINE;
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
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

        [Property("Password", NotNull = true)]
        public string Password
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

        [Property("RequiredRole", NotNull = true, Default = "1")]
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

        [HasMany(typeof(WorldCharacter), Lazy = true)]
        public IList<WorldCharacter> Characters
        {
            get { return m_characters ?? new List<WorldCharacter>(); }
            set { m_characters = value; }
        }

        [HasMany(typeof(DeletedWorldCharacter), Lazy = true)]
        public IList<DeletedWorldCharacter> DeletedCharacters
        {
            get { return m_deletedCharacters ?? new List<DeletedWorldCharacter>(); }
            set { m_deletedCharacters = value; }
        }

        [HasMany(typeof(ConnectionLog), Lazy=true)]
        public IList<ConnectionLog> Connections
        {
            get { return m_connections ?? new List<ConnectionLog>(); }
            set { m_connections = value; }
        }

        public ServerStatusEnum Status
        {
            get;
            set;
        }

        public int CharsCount
        {
            get { return m_charsCount; }
            set { m_charsCount = value < 0 ? 0 : value; }
        }

        public bool Connected
        {
            get;
            set;
        }

        public DateTime LastPing
        {
            get;
            set;
        }

        public DateTime LastUpdate
        {
            get;
            set;
        }

        public static WorldServer FindWorldById(int id)
        {
            return FindByPrimaryKey(id);
        }

        public static bool Exists(int id)
        {
            return Exists(Restrictions.Eq("Id", id));
        }

    }
}