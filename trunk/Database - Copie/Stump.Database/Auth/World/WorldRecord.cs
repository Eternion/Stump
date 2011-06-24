
using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.Database.Types;
using Stump.DofusProtocol.Enums;

namespace Stump.Database.AuthServer.World
{
    [Serializable]
    [ActiveRecord("worlds")]
    public sealed class WorldRecord : AuthBaseRecord<WorldRecord>
    {
        private IList<WorldCharacterRecord> m_characters;
        private IList<DeletedWorldCharacterRecord> m_deletedCharacters;
        private IList<ConnectionRecord> m_connections;
        private int m_charsCount;
        private bool m_connected;
        private ServerStatusEnum m_state = ServerStatusEnum.OFFLINE;

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

        [HasMany(typeof(WorldCharacterRecord), Lazy = true)]
        public IList<WorldCharacterRecord> Characters
        {
            get { return m_characters ?? new List<WorldCharacterRecord>(); }
            set { m_characters = value; }
        }

        [HasMany(typeof(DeletedWorldCharacterRecord), Lazy = true)]
        public IList<DeletedWorldCharacterRecord> DeletedCharacters
        {
            get { return m_deletedCharacters ?? new List<DeletedWorldCharacterRecord>(); }
            set { m_deletedCharacters = value; }
        }

        [HasMany(typeof(ConnectionRecord), Lazy=true)]
        public IList<ConnectionRecord> Connections
        {
            get { return m_connections ?? new List<ConnectionRecord>(); }
            set { m_connections = value; }
        }

        public ServerStatusEnum Status
        {
            get { return m_state; }
            set { m_state = value; }
        }

        public int CharsCount
        {
            get { return m_charsCount; }
            set { m_charsCount = value < 0 ? 0 : value; }
        }

        public bool Connected
        {
            get { return m_connected; }
            set { m_connected = value; }
        }


        public static WorldRecord FindWorldById(int id)
        {
            return FindByPrimaryKey(id);
        }

        public static bool Exists(int id)
        {
            return Exists(Restrictions.Eq("Id", id));
        }

    }
}