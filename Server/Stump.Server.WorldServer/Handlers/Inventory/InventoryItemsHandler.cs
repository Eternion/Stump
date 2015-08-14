using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Items.Player.Custom.LivingObjects;

namespace Stump.Server.WorldServer.Handlers.Inventory
{
    public partial class InventoryHandler
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

            client.Character.Inventory.RemoveItem(item, message.quantity);
        }

        [WorldHandler(ObjectUseMessage.Id)]
        public static void HandleObjectUseMessage(WorldClient client, ObjectUseMessage message)
        {
            var item = client.Character.Inventory.TryGetItem(message.objectUID);

            if (item == null)
                return;

            client.Character.Inventory.UseItem(item);
        }

        [WorldHandler(ObjectUseMultipleMessage.Id)]
        public static void HandleObjectUseMultipleMessage(WorldClient client, ObjectUseMultipleMessage message)
        {
            var item = client.Character.Inventory.TryGetItem(message.objectUID);

            if (item == null)
                return;

            client.Character.Inventory.UseItem(item,  message.quantity);
        }

        [WorldHandler(ObjectUseOnCellMessage.Id)]
        public static void HandleObjectUseOnCellMessage(WorldClient client, ObjectUseOnCellMessage message)
        {
            var item = client.Character.Inventory.TryGetItem(message.objectUID);

            if (item == null)
                return;

            var cell = client.Character.Map.GetCell(message.cells);

            if (cell == null)
                return;

            client.Character.Inventory.UseItem(item, cell);
        }

        [WorldHandler(ObjectUseOnCharacterMessage.Id)]
        public static void HandleObjectUseOnCharacterMessage(WorldClient client, ObjectUseOnCharacterMessage message)
        {
            var item = client.Character.Inventory.TryGetItem(message.objectUID);

            if (item == null)
                return;

            var character = client.Character.Map.GetActor<Character>(message.characterId);

            if (character == null)
                return;

            client.Character.Inventory.UseItem(item, character);
        }

        [WorldHandler(ObjectFeedMessage.Id)]
        public static void HandleObjectFeedMessage(WorldClient client, ObjectFeedMessage message)
        {
            var item = client.Character.Inventory.TryGetItem(message.objectUID);
            var food = client.Character.Inventory.TryGetItem(message.foodUID);

            if (item == null || food == null)
                return;

            if (food.Stack < message.foodQuantity)
                message.foodQuantity = (short) food.Stack;

            var i = 0;
            for (; i < message.foodQuantity; i++)
            {
                if (!item.Feed(food))
                    break;
            }

            client.Character.Inventory.RemoveItem(food, i);
        }

        [WorldHandler(LivingObjectChangeSkinRequestMessage.Id)]
        public static void HandleLivingObjectChangeSkinRequestMessage(WorldClient client, LivingObjectChangeSkinRequestMessage message)
        {
            var item = client.Character.Inventory.TryGetItem(message.livingUID);

            if (!(item is CommonLivingObject))
                return;

            ((CommonLivingObject) item).SelectedLevel = (short)message.skinId;
        }

        [WorldHandler(LivingObjectDissociateMessage.Id)]
        public static void HandleLivingObjectDissociateMessage(WorldClient client, LivingObjectDissociateMessage message)
        {
            var item = client.Character.Inventory.TryGetItem(message.livingUID);

            if (!(item is BoundLivingObjectItem))
                return;

            ((BoundLivingObjectItem) item).Dissociate();
        }

        [WorldHandler(ObjectDropMessage.Id)]
        public static void HandleObjectDropMessage(WorldClient client, ObjectDropMessage message)
        {
            if (client.Character.IsInFight() || client.Character.IsInExchange())
                return;

            client.Character.DropItem(message.objectUID, message.quantity);
        }

        public static void SendGameRolePlayPlayerLifeStatusMessage(IPacketReceiver client)
        {
            client.Send(new GameRolePlayPlayerLifeStatusMessage());
        }

        public static void SendInventoryContentMessage(WorldClient client)
        {
            client.Send(new InventoryContentMessage(client.Character.Inventory.Select(entry => entry.GetObjectItem()),
                client.Character.Inventory.Kamas));
        }

        public static void SendInventoryContentAndPresetMessage(WorldClient client)
        {
            client.Send(new InventoryContentAndPresetMessage(client.Character.Inventory.Select(entry => entry.GetObjectItem()),
                client.Character.Inventory.Kamas, client.Character.Inventory.Presets.Select(entry => entry.GetNetworkPreset())));
        }

        public static void SendInventoryWeightMessage(WorldClient client)
        {
            client.Send(new InventoryWeightMessage(client.Character.Inventory.Weight,
                                                   (int) client.Character.Inventory.WeightTotal));
        }

        public static void SendExchangeKamaModifiedMessage(IPacketReceiver client, bool remote, int kamasAmount)
        {
            client.Send(new ExchangeKamaModifiedMessage(remote, kamasAmount));
        }

        public static void SendObjectAddedMessage(IPacketReceiver client, BasePlayerItem addedItem)
        {
            client.Send(new ObjectAddedMessage(addedItem.GetObjectItem()));
        }

        public static void SendObjectsAddedMessage(IPacketReceiver client, IEnumerable<BasePlayerItem> addeditems)
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

        public static void SendObjectModifiedMessage(IPacketReceiver client, BasePlayerItem item)
        {
            client.Send(new ObjectModifiedMessage(item.GetObjectItem()));
        }

        public static void SendObjectMovementMessage(IPacketReceiver client, BasePlayerItem movedItem)
        {
            client.Send(new ObjectMovementMessage(movedItem.Guid, (byte) movedItem.Position));
        }

        public static void SendObjectQuantityMessage(IPacketReceiver client, BasePlayerItem item)
        {
            client.Send(new ObjectQuantityMessage(item.Guid, (int) item.Stack));
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

        public static void SendExchangeShopStockMovementUpdatedMessage(IPacketReceiver client, MerchantItem item)
        {
            client.Send(new ExchangeShopStockMovementUpdatedMessage(item.GetObjectItemToSell()));
        }

        public static void SendExchangeShopStockMovementRemovedMessage(IPacketReceiver client, MerchantItem item)
        {
            client.Send(new ExchangeShopStockMovementRemovedMessage(item.Guid));
        }
    }
}