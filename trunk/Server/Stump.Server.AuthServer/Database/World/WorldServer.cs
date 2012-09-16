using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.IPC;

namespace Stump.Server.AuthServer.Database
{
    public class WorldServerConfiguration : EntityTypeConfiguration<WorldServer>
    {
        public WorldServerConfiguration()
        {
            ToTable("worlds");
            Ignore(x => x.RemoteEndpoint);
            Ignore(x => x.RemoteOperations);
            Ignore(x => x.SessionId);
            Ignore(x => x.LastPing);
            Ignore(x => x.Address);
        }
    }

    public partial class WorldServer
    {
        // Primitive properties

        private int m_charsCount;

        public WorldServer()
        {
            Status = ServerStatusEnum.OFFLINE;
        }

        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public bool RequireSubscription
        {
            get;
            set;
        }

        private int RequiredRoleAsInt
        {
            get;
            set;
        }

        public int Completion
        {
            get;
            set;
        }

        public bool ServerSelectable
        {
            get;
            set;
        }

        public int CharCapacity
        {
            get;
            set;
        }

        private int StatusAsInt
        {
            get;
            set;
        }

        public int? CharsCount
        {
            get;
            set;
        }

        public RoleEnum RequiredRole
        {
            get { return (RoleEnum) RequiredRoleAsInt; }
            set { RequiredRoleAsInt = (byte) value; }
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

        public ServerStatusEnum Status
        {
            get { return (ServerStatusEnum) StatusAsInt; }
            set { StatusAsInt = (int) value; }
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

        public ushort Port
        {
            get;
            private set;
        }

        public string Address
        {
            get;
            private set;
        }

        public void SetOnline(string address, ushort port)
        {
            Status = ServerStatusEnum.ONLINE;
            LastPing = DateTime.Now;
            Address = address;
            Port = port;

            AuthServer.Instance.SaveDatabaseChanges();
        }

        public void SetOffline()
        {
            Status = ServerStatusEnum.OFFLINE;
            CharsCount = 0;

            CloseSession();
            AuthServer.Instance.SaveDatabaseChanges();
        }

        #endregion

        public override string ToString()
        {
            return string.Format("{0}({1})", Name, Id);
        }
    }
}