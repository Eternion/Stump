using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Handlers.Inventory
{
    public partial class InventoryHandler
    {
        public static void SendStorageInventoryContentMessage(IPacketReceiver client, TaxCollectorNpc taxCollector)
        {
            client.Send(taxCollector.GetStorageInventoryContent());
        }

        public static void SendStorageInventoryContentMessage(IPacketReceiver client, Bank bank)
        {
            client.Send(new StorageInventoryContentMessage(bank.Select(x => x.GetObjectItem()), bank.Kamas));
        }

        public static void SendStorageKamasUpdateMessage(IPacketReceiver client, int kamas)
        {
            client.Send(new StorageKamasUpdateMessage(kamas));
        }

        public static void SendStorageObjectRemoveMessage(IPacketReceiver client, IItem item)
        {
            client.Send(new StorageObjectRemoveMessage(item.Guid));
        }

        public static void SendStorageObjectUpdateMessage(IPacketReceiver client, IItem item)
        {
            client.Send(new StorageObjectUpdateMessage(item.GetObjectItem()));
        }

        public static void SendStorageObjectsRemoveMessage(IPacketReceiver client, IEnumerable<int> guids)
        {
            client.Send(new StorageObjectsRemoveMessage(guids));
        }

        public static void SendStorageObjectsUpdateMessage(IPacketReceiver client, IEnumerable<IItem> items)
        {
            client.Send(new StorageObjectsUpdateMessage(items.Select(x => x.GetObjectItem())));
        }
    }
}