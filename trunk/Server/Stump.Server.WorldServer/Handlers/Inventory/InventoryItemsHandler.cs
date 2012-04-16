using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Handlers.Inventory
{
    public partial class InventoryHandler : WorldHandlerContainer
    {
        [WorldHandler(ObjectSetPositionMessage.Id)]
        public static void HandleObjectSetPositionMessage(WorldClient client, ObjectSetPositionMessage message)
        {
            if (!Enum.IsDefined(typeof(CharacterInventoryPositionEnum), (int) message.position))
                return;

            var item = client.Character.Inventory.TryGetItem(message.objectUID);

            if (item == null)
                return;

            client.Character.Inventory.MoveItem(item, (CharacterInventoryPositionEnum) message.position);
        }

        [WorldHandler(ObjectDeleteMessage.Id)]
        public static void HandleObjectDeleteMessage(WorldClient client, ObjectDeleteMessage message)
        {
            var item = client.Character.Inventory.TryGetItem(message.objectUID);

            if (item == null)
                return;

            client.Character.Inventory.RemoveItem(item, (uint) message.quantity);
        }

        [WorldHandler(ObjectUseMessage.Id)]
        public static void HandleObjectUseMessage(WorldClient client, ObjectUseMessage message)
        {
            var item = client.Character.Inventory.TryGetItem(message.objectUID);

            if (item == null)
                return;

            client.Character.Inventory.UseItem(item);
        }

        public static void SendGameRolePlayPlayerLifeStatusMessage(IPacketReceiver client)
        {
            client.Send(new GameRolePlayPlayerLifeStatusMessage());
        }

        public static void SendInventoryContentMessage(WorldClient client)
        {
            client.Send(
                new InventoryContentMessage(
                    client.Character.Inventory.Select(entry => entry.GetObjectItem()),
                    client.Character.Inventory.Kamas));
        }

        public static void SendInventoryWeightMessage(WorldClient client)
        {
            client.Send(new InventoryWeightMessage((int) client.Character.Inventory.Weight,
                                                   (int) client.Character.Inventory.WeightTotal));
        }

        public static void SendExchangeKamaModifiedMessage(IPacketReceiver client, bool remote, int kamasAmount)
        {
            client.Send(new ExchangeKamaModifiedMessage(remote, kamasAmount));
        }

        public static void SendObjectAddedMessage(IPacketReceiver client, PlayerItem addedItem)
        {
            client.Send(new ObjectAddedMessage(addedItem.GetObjectItem()));
        }

        public static void SendObjectsAddedMessage(IPacketReceiver client, IEnumerable<PlayerItem> addeditems)
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

        public static void SendObjectModifiedMessage(IPacketReceiver client, PlayerItem item)
        {
            client.Send(new ObjectModifiedMessage(item.GetObjectItem()));
        }

        public static void SendObjectMovementMessage(IPacketReceiver client, PlayerItem movedItem)
        {
            client.Send(new ObjectMovementMessage(movedItem.Guid, (byte) movedItem.Position));
        }

        public static void SendObjectQuantityMessage(IPacketReceiver client, PlayerItem modifieditem)
        {
            client.Send(new ObjectQuantityMessage(modifieditem.Guid, modifieditem.Stack));
        }

        public static void SendObjectErrorMessage(IPacketReceiver client, ObjectErrorEnum error)
        {
            client.Send(new ObjectErrorMessage((sbyte) error));
        }

        public static void SendSetUpdateMessage(WorldClient client, ItemSetTemplate itemSet)
        {
            client.Send(new SetUpdateMessage((short) itemSet.Id,
                client.Character.Inventory.GetItemSetEquipped(itemSet).Select(entry => (short)entry.Template.Id),
                client.Character.Inventory.GetItemSetEffects(itemSet).Select(entry => entry.GetObjectEffect())));
        }
    }
}