﻿using System.Collections.Generic;
using System.Linq;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Items.Player
{
    public sealed class CharacterMerchantBag : PersistantItemsCollection<MerchantItem>
    {
        public CharacterMerchantBag(Character owner)
        {
            Owner = owner;
        }

        public Character Owner
        {
            get;
            private set;
        }

        public bool IsLoaded
        {
            get;
            private set;
        }

        internal void LoadMerchantBag(MerchantBag bag)
        {
            if (IsLoaded)
                return;

            Items = bag.ToDictionary(x => x.Guid);
            IsLoaded = true;
        }

        internal void LoadMerchantBag()
        {
            if (IsLoaded)
                return;

            var records = ItemManager.Instance.FindPlayerMerchantItems(Owner.Id);
            Items = records.Select(entry => new MerchantItem(entry)).ToDictionary(entry => entry.Guid);
            IsLoaded = true;
        }

        private void UnLoadMerchantBag()
        {
            Items.Clear();
        }

        public int GetMerchantTax()
        {
            var resultTax = Items.Aggregate<KeyValuePair<int, MerchantItem>, double>(0, (current, item) => current + (item.Value.Price*item.Value.Stack));

            resultTax = (resultTax * 0.1);

            return (int)resultTax;
        }

        public bool MoveToInventory(MerchantItem item)
        {
            return MoveToInventory(item, item.Stack);
        }

        public bool MoveToInventory(MerchantItem item, uint quantity)
        {
            if (quantity == 0)
                return false;

            if (quantity > item.Stack)
                quantity = item.Stack;

            RemoveItem(item, quantity);
            BasePlayerItem newItem = ItemManager.Instance.CreatePlayerItem(Owner, item.Template, quantity,
                                                                       item.Effects);

            Owner.Inventory.AddItem(newItem);

            return true;
        }

        protected override void OnItemAdded(MerchantItem item)
        {
            InventoryHandler.SendExchangeShopStockMovementUpdatedMessage(Owner.Client, item);

            base.OnItemAdded(item);
        }

        protected override void OnItemRemoved(MerchantItem item)
        {
            InventoryHandler.SendExchangeShopStockMovementRemovedMessage(Owner.Client, item);

            base.OnItemRemoved(item);
        }

        protected override void OnItemStackChanged(MerchantItem item, int difference)
        {
            InventoryHandler.SendExchangeShopStockMovementUpdatedMessage(Owner.Client, item);

            base.OnItemStackChanged(item, difference);
        }
    }
}