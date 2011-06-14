
using System;
using System.Linq;
using System.Net.Sockets;
using Stump.BaseCore.Framework.Extensions;
using Stump.Database;
using Stump.Database.AuthServer;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.AuthServer
{
    public class AuthClient : BaseClient
    {
        private string m_login;

        public AuthClient(Socket socket)
            : base(socket)
        {
            Key = new Random().RandomString(32);

            Send(new ProtocolRequired(ClientVersion.RequiredVersion, ClientVersion.ActualVersion));
            Send(new HelloConnectMessage(1, Key));

            CanReceive = true;
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

        public AccountRecord Account
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

        public void Save()
        {
            Account.UpdateAndFlush();
        }


        public override string ToString()
        {
            return base.ToString() + (Account != null ? " <" + Account.Login + ">" : "");
        }
    }
}