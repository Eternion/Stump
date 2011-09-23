using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
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

        public static void SendExchangeKamaModifiedMessage(WorldClient client, bool remote, int kamasAmount)
        {
            client.Send(new ExchangeKamaModifiedMessage(remote, kamasAmount));
        }

        public static void SendObjectAddedMessage(WorldClient client, Item addedItem)
        {
            client.Send(new ObjectAddedMessage(addedItem.GetObjectItem()));
        }

        public static void SendObjectsAddedMessage(WorldClient client, IEnumerable<Item> addeditems)
        {
            client.Send(new ObjectsAddedMessage(addeditems.Select(entry => entry.GetObjectItem())));
        }

        public static void SendObjectDeletedMessage(WorldClient client, int guid)
        {
            client.Send(new ObjectDeletedMessage(guid));
        }

        public static void SendObjectsDeletedMessage(WorldClient client, IEnumerable<int> guids)
        {
            client.Send(new ObjectsDeletedMessage(guids.Select(entry => entry).ToList()));
        }

        public static void SendObjectModifiedMessage(WorldClient client, Item item)
        {
            client.Send(new ObjectModifiedMessage(item.GetObjectItem()));
        }

        public static void SendObjectMovementMessage(WorldClient client, Item movedItem)
        {
            client.Send(new ObjectMovementMessage(movedItem.Guid, (byte) movedItem.Position));
        }

        public static void SendObjectQuantityMessage(WorldClient client, Item modifieditem)
        {
            client.Send(new ObjectQuantityMessage(modifieditem.Guid, modifieditem.Stack));
        }

        public static void SendObjectErrorMessage(WorldClient client, ObjectErrorEnum error)
        {
            client.Send(new ObjectErrorMessage((sbyte) error));
        }
    }
}