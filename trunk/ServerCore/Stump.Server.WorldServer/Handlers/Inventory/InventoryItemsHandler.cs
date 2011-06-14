
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Items;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class InventoryHandler : WorldHandlerContainer
    {
        // todo : manage amounts
        [WorldHandler(typeof (ObjectSetPositionMessage))]
        public static void HandleObjectSetPositionMessage(WorldClient client, ObjectSetPositionMessage message)
        {
            if (!Enum.IsDefined(typeof (CharacterInventoryPositionEnum), (int) message.position))
            {
                return;
            }

            client.ActiveCharacter.Inventory.MoveItem(message.objectUID,
                                                      (CharacterInventoryPositionEnum) message.position);
            SendInventoryWeightMessage(client);
        }

        [WorldHandler(typeof (ObjectDeleteMessage))]
        public static void HandleObjectDeleteMessage(WorldClient client, ObjectDeleteMessage message)
        {
            client.ActiveCharacter.Inventory.DeleteItem(message.objectUID, message.quantity);

            SendInventoryWeightMessage(client);
        }

        public static void SendInventoryContentMessage(WorldClient client)
        {
            client.Send(
                new InventoryContentMessage(
                    client.ActiveCharacter.Inventory.Items.Select(entry => entry.ToNetworkItem()).ToList(),
                    (uint) client.ActiveCharacter.Inventory.Kamas));
        }

        public static void SendInventoryWeightMessage(WorldClient client)
        {
            client.Send(new InventoryWeightMessage(client.ActiveCharacter.Inventory.Weight,
                                                   client.ActiveCharacter.Inventory.WeightTotal));
        }

        public static void SendExchangeKamaModifiedMessage(WorldClient client, bool remote, uint kamasAmount)
        {
            client.Send(new ExchangeKamaModifiedMessage(remote, kamasAmount));
        }

        public static void SendObjectAddedMessage(WorldClient client, Item addedItem)
        {
            client.Send(new ObjectAddedMessage(addedItem.ToNetworkItem()));
        }

        public static void SendObjectsAddedMessage(WorldClient client, IEnumerable<Item> addeditems)
        {
            client.Send(new ObjectsAddedMessage(addeditems.Select(entry => entry.ToNetworkItem()).ToList()));
        }

        public static void SendObjectDeletedMessage(WorldClient client, long guid)
        {
            client.Send(new ObjectDeletedMessage((uint) guid));
        }

        public static void SendObjectsDeletedMessage(WorldClient client, IEnumerable<long> guids)
        {
            client.Send(new ObjectsDeletedMessage(guids.Select(entry => (uint) entry).ToList()));
        }

        public static void SendObjectModifiedMessage(WorldClient client, Item item)
        {
            client.Send(new ObjectModifiedMessage(item.ToNetworkItem()));
        }

        public static void SendObjectMovementMessage(WorldClient client, Item movedItem)
        {
            client.Send(new ObjectMovementMessage((uint) movedItem.Guid, (uint) movedItem.Position));
        }

        public static void SendObjectQuantityMessage(WorldClient client, Item modifieditem)
        {
            client.Send(new ObjectQuantityMessage((uint) modifieditem.Guid, modifieditem.Stack));
        }

        public static void SendObjectErrorMessage(WorldClient client, ObjectErrorEnum error)
        {
            client.Send(new ObjectErrorMessage((byte) error));
        }
    }
}