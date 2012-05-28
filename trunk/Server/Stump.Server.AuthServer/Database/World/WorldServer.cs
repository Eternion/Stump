using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.IPC;

namespace Stump.Server.AuthServer.Database.World
{
    [Serializable]
    [ActiveRecord("worlds")]
    public sealed class WorldServer : AuthBaseRecord<WorldServer>
    {
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

        public string Address
        {
            get;
            set;
        }

        public ushort Port
        {
            get;
            set;
        }

        #region Session

        public string SessionId
        {
            get;
            set;
        }

        public RemoteEndpointMessageProperty RemoteEndpoint
        {
            get;
            set;
        }

        public IContextChannel Channel
        {
            get;
            set;
        }

        public IRemoteWorldOperations RemoteOperations
        {
            get;
            set;
        }

        public void SetSession(IContextChannel channel, string sessionId, RemoteEndpointMessageProperty remoteEndpoint)
        {
            Channel = channel;
            SessionId = sessionId;
            RemoteEndpoint = remoteEndpoint;
        }

        public void CloseSession()
        {
            if (RemoteOperations == null)
                return;

            try
            {
                if (Channel != null)
                    Channel.Close();
            }
            catch
            {
            }

            try
            {
                //if (RemoteOperations != null)
                  //  RemoteOperations.Close();
            }
            catch
            {
            }

            RemoteOperations = null;
            Channel = null;
            SessionId = null;
            RemoteEndpoint = null;
        }

        #endregion

        #region Status

        [Property]
        public ServerStatusEnum Status
        {
            get;
            set;
        }

        public bool Connected
        {
            get { return Status == ServerStatusEnum.ONLINE; }
        }

        public DateTime LastPing
        {
            get;
            set;
        }

        [Property]
        public int CharsCount
        {
            get { return m_charsCount; }
            set { m_charsCount = value < 0 ? 0 : value; }
        }

        public void SetOnline()
        {
            Status = ServerStatusEnum.ONLINE;
            LastPing = DateTime.Now;

            Update();
        }

        public void SetOnline(string address, ushort port)
        {
            Status = ServerStatusEnum.ONLINE;
            LastPing = DateTime.Now;
            Address = address;
            Port = port;

            UpdateAndFlush();
        }

        public void SetOffline()
        {
            Status = ServerStatusEnum.OFFLINE;
            CharsCount = 0;

            CloseSession();
            Update();
        }

        #endregion

        public static WorldServer FindWorldById(int id)
        {
            return FindByPrimaryKey(id);
        }

        public static bool Exists(int id)
        {
            return Exists(Restrictions.Eq("Id", id));
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", Name, Id);
        }
    }
}