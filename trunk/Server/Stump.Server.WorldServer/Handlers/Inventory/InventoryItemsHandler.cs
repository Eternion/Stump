using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Worlds.Items;

namespace Stump.Server.WorldServer.Handlers.Inventory
{
    public partial class InventoryHandler : WorldHandlerContainer
    {
        [WorldHandler(ObjectSetPositionMessage.Id)]
        public static void HandleObjectSetPositionMessage(WorldClient client, ObjectSetPositionMessage message)
        {
            if (!Enum.IsDefined(typeof(CharacterInventoryPositionEnum), (int) message.position))
            {
                return;
            }

            client.ActiveCharacter.Inventory.MoveItem(message.objectUID,
                                                      (CharacterInventoryPositionEnum) message.position);
            SendInventoryWeightMessage(client);
        }

        [WorldHandler(ObjectDeleteMessage.Id)]
        public static void HandleObjectDeleteMessage(WorldClient client, ObjectDeleteMessage message)
        {
            client.ActiveCharacter.Inventory.RemoveItem(message.objectUID, (uint) message.quantity);

            SendInventoryWeightMessage(client);
        }

        public static void SendInventoryContentMessage(WorldClient client)
        {
            client.Send(
                new InventoryContentMessage(
                    client.ActiveCharacter.Inventory.Items.Select(entry => entry.GetObjectItem()),
                    client.ActiveCharacter.Inventory.Kamas));
        }

        public static void SendInventoryWeightMessage(WorldClient client)
        {
            client.Send(new InventoryWeightMessage((int) client.ActiveCharacter.Inventory.Weight,
                                                   (int) client.ActiveCharacter.Inventory.WeightTotal));
        }

        public static void SendExchangeKamaModifiedMessage(IPacketReceiver client, bool remote, int kamasAmount)
        {
            client.Send(new ExchangeKamaModifiedMessage(remote, kamasAmount));
        }

        public static void SendObjectAddedMessage(IPacketReceiver client, Item addedItem)
        {
            client.Send(new ObjectAddedMessage(addedItem.GetObjectItem()));
        }

        public static void SendObjectsAddedMessage(IPacketReceiver client, IEnumerable<Item> addeditems)
        {
            client.Send(new ObjectsAddedMessage(addeditems.Select(entry => entry.GetObjectItem())));
        }

        public static void SendObjectDeletedMessage(IPacketReceiver client, int guid)
        {
            client.Send(new ObjectDeletedMessage(guid));
        }

        public static void SendObjectsDeletedMessage(IPacketReceiver client, IEnumerable<int> guids)
        {
            client.Send(new ObjectsDeletedMessage(guids.Select(entry => entry).ToList()));
        }

        public static void SendObjectModifiedMessage(IPacketReceiver client, Item item)
        {
            client.Send(new ObjectModifiedMessage(item.GetObjectItem()));
        }

        public static void SendObjectMovementMessage(IPacketReceiver client, Item movedItem)
        {
            client.Send(new ObjectMovementMessage(movedItem.Guid, (byte) movedItem.Position));
        }

        public static void SendObjectQuantityMessage(IPacketReceiver client, Item modifieditem)
        {
            client.Send(new ObjectQuantityMessage(modifieditem.Guid, modifieditem.Stack));
        }

        public static void SendObjectErrorMessage(IPacketReceiver client, ObjectErrorEnum error)
        {
            client.Send(new ObjectErrorMessage((sbyte) error));
        }
    }
}