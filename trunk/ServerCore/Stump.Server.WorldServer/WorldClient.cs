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
using System.Net.Sockets;
using Stump.Database;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.IPC;

namespace Stump.Server.WorldServer
{
    public class WorldClient : BaseClient
    {
        public new event Action<WorldClient> ClientDisconnected;

        public override void NotifyClientDisconnected()
        {
            Action<WorldClient> handler = ClientDisconnected;
            if (handler != null) handler(this);
        }

        private string m_login;

        public WorldClient(Socket socket)
            : base(socket)
        {
            Send(new ProtocolRequired(ClientVersion.RequiredVersion, ClientVersion.ActualVersion));
            Send(new HelloGameMessage());

            CanReceive = true;
        }

        public bool AutoConnect
        {
            get;
            set;
        }

        public AccountRecord Account
        {
            get;
            set;
        }

        public List<CharacterRecord> Characters
        {
            get;
            set;
        }

        public Character ActiveCharacter
        {
            get;
            set;
        }

        public override void OnDisconnect()
        {
            IpcAccessor.Instance.ProxyObject.DecrementConnectedChars(WorldServer.ServerInformation);
            if (ActiveCharacter != null)
            {
                ActiveCharacter.LogOut();
            }
            base.OnDisconnect();
        }

        public override string ToString()
        {
            return base.ToString() + (Account != null ? " <" + Account.Login + ">" : "");
        }
    }
}