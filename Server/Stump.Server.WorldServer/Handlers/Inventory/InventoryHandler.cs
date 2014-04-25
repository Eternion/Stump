using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.WorldServer.Handlers.Inventory
{
    public partial class InventoryHandler
    {
        public static void SendKamasUpdateMessage(IPacketReceiver client, int kamasAmount)
        {
            client.Send(new KamasUpdateMessage(kamasAmount));
        }
    }
}