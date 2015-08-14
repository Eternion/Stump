using System;
using System.Linq;
using Stump.DofusProtocol.Enums;
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
            if (!BidHouseManager.Quantities.Contains(quantity))
                return false;

            var item = Character.Inventory.TryGetItem(id);

            if (item == null)
                return false;

            if (item.IsLinkedToPlayer() || item.IsLinkedToAccount())
                return false;

            if (item.Template.Level > ((BidHouseExchange) Dialog).MaxItemLevel)
                return false;

            if (BidHouseManager.Instance.GetBidHouseItems(Character.Account.Id, ((BidHouseExchange)Dialog).Types).Count() >= Character.Level)
                return false;

            if (quantity > item.Stack)
                quantity = (int)item.Stack;

            var tax = (int)Math.Round((price * BidHouseManager.TaxPercent) / 100);

            if (Character.Kamas < tax)
            {
                //Vous ne disposez pas d'assez de kamas pour acquiter la taxe de mise en vente...
                Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 57);

                return false;
            }

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

            if (item.Template.Level > ((BidHouseExchange)Dialog).MaxItemLevel)
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
