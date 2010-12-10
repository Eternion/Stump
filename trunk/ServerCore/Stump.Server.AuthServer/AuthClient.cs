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
using System.Linq;
using System.Net.Sockets;
using Stump.BaseCore.Framework.Extensions;
using Stump.Database;
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

            Send(new ProtocolRequired().initProtocolRequired(ClientVersion.ActualVersion, ClientVersion.RequiredVersion));
            Send(new HelloConnectMessage().initHelloConnectMessage(1, Key));

            CanReceive = true;
        }

        public bool AutoConnect
        {
            get;
            set;
        }

        public int SelectedServerId
        {
            get;
            set;
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

        public WorldCharacterRecord[] Characters
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

        public byte GetCharactersCount(int serverid)
        {
            if (Characters.Length == 0)
                return 0;

            return  (byte)Characters.Where(entry => entry.ServerId==serverid).Count();
        }

        public override string ToString()
        {
            return base.ToString() + (Account != null ? " <" + Account.Login + ">" : "");
        }
    }
}