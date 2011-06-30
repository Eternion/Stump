using System.Collections.Generic;
using System.Net.Sockets;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.IPC;

namespace Stump.Server.WorldServer.Network
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

        /*public AccountRecord Account
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
        }*/

        protected override void OnDisconnect()
        {
            /*IpcAccessor.Instance.ProxyObject.DecrementConnectedChars(WorldServer.ServerInformation);
            if (ActiveCharacter != null)
            {
                ActiveCharacter.LogOut();
            }*/
            base.OnDisconnect();
        }

        /*public override string ToString()
        {
            return base.ToString() + (Account != null ? " <" + Account.Login + ">" : "");
        }*/
    }
}