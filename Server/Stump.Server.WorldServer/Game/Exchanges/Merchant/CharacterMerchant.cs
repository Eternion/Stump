using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Exchanges.Merchant
{
    public class CharacterMerchant : Exchanger
    {
        public CharacterMerchant(Character character, MerchantExchange merchantTrade)
            : base(merchantTrade)
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
            if (quantity >= 0)
                return false;

            var deleteItem = Character.MerchantBag.TryGetItem(id);

            return deleteItem != null && Character.MerchantBag.TakeBack(deleteItem, -quantity);
        }

        public bool MovePricedItem(int id, int quantity, uint price)
        {
            if (quantity <= 0)
                return false;

            var item = Character.Inventory.TryGetItem(id);

            return item != null && Character.MerchantBag.StoreItem(item, quantity, price);
        }

        public bool ModifyItem(int id, int quantity, uint price)
        {
            var item = Character.MerchantBag.TryGetItem(id);

            return item != null && Character.MerchantBag.ModifyItem(item, quantity, price);
        }

        public override bool SetKamas(int amount)
        {
            return false;
        }
    }
}
