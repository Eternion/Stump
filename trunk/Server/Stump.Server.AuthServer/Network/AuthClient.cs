using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Messages.Custom;
using Stump.Server.AuthServer.Database;
using Stump.Server.AuthServer.Handlers.Connection;
using Stump.Server.AuthServer.Managers;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.AuthServer.Network
{
    public sealed class AuthClient : BaseClient
    {
        private string m_login;

        public AuthClient(Socket socket)
            : base(socket)
        {
            Key = new Random().RandomString(32);

            var patch = AuthServer.Instance.GetConnectionSwfPatch();
            if (patch != null)
                Send(new RawDataMessageFixed(patch));

            Send(new ProtocolRequired(VersionExtension.ProtocolRequired, VersionExtension.ActualProtocol));
            Send(new HelloConnectMessage(CredentialManager.Instance.GetSalt(), CredentialManager.Instance.GetRSAPublicKey()));

            CanReceive = true;
            lock (ConnectionHandler.ConnectionQueue.SyncRoot)
                ConnectionHandler.ConnectionQueue.Add(this);
        }
        
        public string Login
        {
            get { return m_login; }
            set { m_login = value.ToLower(); }
        }

        public string Password
        {
            get;
            set;
        }

        public string Key
        {
            get;
            set;
        }

        public Account Account
        {
            get;
            set;
        }

        /// <summary>
        ///   True when the client is choising a server
        /// </summary>
        public bool LookingOfServers
        {
            get;
            set;
        }

        public DateTime InQueueUntil
        {
            get;
            set;
         }

        protected override void OnMessageReceived(Message message)
        {
            AuthPacketHandler.Instance.Dispatch(this, message); 

            base.OnMessageReceived(message);
        }

        public void Save()
        {
            AuthServer.Instance.IOTaskPool.AddMessage(() => SaveNow());
        }

        public void SaveNow()
        {
            AuthServer.Instance.IOTaskPool.EnsureContext();

            Account.Tokens += Account.NewTokens;
            Account.NewTokens = 0;

            AuthServer.Instance.DBAccessor.Database.Save(Account);
        }


        public override string ToString()
        {
            return base.ToString() + (Account != null ? " (" + Account.Login + ")" : "");
        }
    }
}