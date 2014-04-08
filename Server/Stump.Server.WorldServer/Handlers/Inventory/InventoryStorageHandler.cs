using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;

namespace Stump.Server.WorldServer.Handlers.Inventory
{
    public partial class InventoryHandler
    {
        public static void SendStorageInventoryContentMessage(IPacketReceiver client, TaxCollectorNpc taxCollector)
        {
            client.Send(new ExchangeStartedMessage(8));
            client.Send(taxCollector.GetStorageInventoryContent());
        }
    }
}