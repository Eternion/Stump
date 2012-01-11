using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;

namespace Stump.Server.WorldServer.Handlers.Inventory
{
    public partial class InventoryHandler : WorldHandlerContainer
    {
        public static void SendKamasUpdateMessage(IPacketReceiver client, int kamasAmount)
        {
            client.Send(new KamasUpdateMessage(kamasAmount));
        }
    }
}