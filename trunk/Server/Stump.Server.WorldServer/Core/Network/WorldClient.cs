using System.Collections.Generic;
using System.Net.Sockets;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.World.Actors.RolePlay;
using Stump.Server.WorldServer.World.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Core.Network
{
    public sealed class WorldClient : BaseClient
    {
        public WorldClient(Socket socket)
            : base(socket)
        {
            Send(new ProtocolRequired(VersionExtension.RequiredVersion, VersionExtension.ActualVersion));
            Send(new HelloGameMessage());

            CanReceive = true;
        }

        public bool AutoConnect
        {
            get;
            set;
        }

        public AccountData Account
        {
            get;
            set;
        }

        /*public WorldAccountRecord WorldAccount
        {
            get;
            set;
        }*/

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

        protected override void OnDisconnect()
        {
            //IpcAccessor.Instance.ProxyObject.DecrementConnectedChars(WorldServer.ServerInformation);
            if (ActiveCharacter != null)
            {
                //ActiveCharacter.LogOut();
            }
            base.OnDisconnect();
        }

        public override string ToString()
        {
            return base.ToString() + (Account != null ? " <" + Account.Login + ">" : "");
        }
    }
}