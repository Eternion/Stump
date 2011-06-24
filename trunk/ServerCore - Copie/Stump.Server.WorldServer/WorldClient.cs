
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Stump.Database;
using Stump.Database.AuthServer;
using Stump.Database.WorldServer;
using Stump.Database.WorldServer.Character;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.IPC;

namespace Stump.Server.WorldServer
{
    public sealed class WorldClient : BaseClient
    {
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

        public WorldAccountRecord WorldAccount
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

        public bool WarnOnFriendConnection
        {
            get;
            set;
        }

        public bool WarnOnFriendLevelGain
        {
            get;
            set;
        }

        protected override void OnDisconnect()
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