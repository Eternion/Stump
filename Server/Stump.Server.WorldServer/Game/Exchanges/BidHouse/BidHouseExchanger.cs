using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.BidHouse;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges.BidHouse
{
    public class BidHouseExchanger : Exchanger
    {
        public BidHouseExchanger(Character character, BidHouseExchange exchange)
            : base(exchange)
        {
            Character = character;
        }

        public Character Character
        {
            get;
            private set;
        }

        public override bool MoveItem(int id, int quantity)
        {
            var item = BidHouseManager.Instance.GetBidHouseItem(id);

            if (item == null)
                return false;

            BidHouseManager.Instance.RemoveBidHouseItem(item);

            var newItem = ItemManager.Instance.CreatePlayerItem(Character, item.Template, (int)item.Stack,
                                                                       item.Effects);

            Character.Inventory.AddItem(newItem);


            InventoryHandler.SendExchangeBidHouseItemRemoveOkMessage(Character.Client, item.Guid);

            return true;
        }

        public bool MovePricedItem(int id, int quantity, uint price)
        {
            if (quantity != 1 && quantity != 10 && quantity != 100)
                return false;

            var item = Character.Inventory.TryGetItem(id);

            if (item == null)
                return false;

            if (item.IsLinkedToPlayer() || item.IsLinkedToAccount())
                return false;

            if (quantity > item.Stack)
                quantity = (int)item.Stack;

            var tax = (int)(price*BidHouseManager.TaxPercent)/100;

            if (Character.Kamas < tax)
                return false;

            Character.Inventory.SubKamas(tax);

            var bidItem = BidHouseManager.Instance.CreateBidHouseItem(Character, item, quantity, price);
            BidHouseManager.Instance.AddBidHouseItem(bidItem);

            Character.Inventory.RemoveItem(item, quantity);

            InventoryHandler.SendExchangeBidHouseItemAddOkMessage(Character.Client, bidItem.GetObjectItemToSellInBid());

            return true;
        }

        public bool ModifyItem(int id, uint price)
        {
            var item = BidHouseManager.Instance.GetBidHouseItem(id);

            if (item == null)
                return false;

            item.Price = price;

            InventoryHandler.SendExchangeBidHouseItemRemoveOkMessage(Character.Client, item.Guid);
            InventoryHandler.SendExchangeBidHouseItemAddOkMessage(Character.Client, item.GetObjectItemToSellInBid());

            return true;
        }

        public override bool SetKamas(int amount)
        {
            return false;
        }
    }
}
