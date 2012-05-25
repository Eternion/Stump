using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Basic;

namespace Stump.Server.WorldServer.Core.Network
{
    public sealed class WorldClient : BaseClient
    {
        public WorldClient(Socket socket)
            : base(socket)
        {
            Send(new ProtocolRequired(VersionExtension.ProtocolRequired, VersionExtension.ActualProtocol));
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

        public WorldAccount WorldAccount
        {
            get;
            set;
        }

        public List<CharacterRecord> Characters
        {
            get;
            set;
        }

        public Character Character
        {
            get;
            set;
        }

        public bool DebugLag
        {
            get;
            set;
        }

        public void ToggleDebugLag(bool state)
        {
            DebugLag = state;
        }

        public override void Send(Message message)
        {
            if (DebugLag)
                Thread.Sleep(new Random().Next(50, 250));

            base.Send(message);
        }

        protected override void OnMessageReceived(Message message)
        {
            if (DebugLag)
                Thread.Sleep(new Random().Next(50, 250));

            WorldPacketHandler.Instance.Dispatch(this, message);

            base.OnMessageReceived(message);
        }

        public void DisconnectAfk()
        {
            BasicHandler.SendSystemMessageDisplayMessage(this, true, 1);

            Disconnect();
        }

        protected override void OnDisconnect()
        {
            if (Character != null)
            {
                Character.LogOut();
            }

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                if (WorldAccount != null)
                {
                    WorldAccount.ConnectedCharacterId = null;
                    WorldAccount.Update();
                }
            });

            base.OnDisconnect();
        }

        public override string ToString()
        {
            return base.ToString() + (Account != null ? " (" + Account.Login + ")" : "");
        }
    }
}