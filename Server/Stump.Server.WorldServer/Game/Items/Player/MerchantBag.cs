#region License GNU GPL
// MerchantBagOffline.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Merchants;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Items.Player
{
    public class MerchantBag : PersistantItemsCollection<MerchantItem>
    {
        public MerchantBag(Merchant owner)
        {
            Owner = owner;
        }

        public MerchantBag(Merchant owner, IEnumerable<MerchantItem> merchantBag)
        {
            Owner = owner;
            Items = merchantBag.ToDictionary(entry => entry.Guid); // just to copy properly
        }

        public Merchant Owner
        {
            get;
            set;
        }

        /// <summary>
        /// Must be saved 
        /// </summary>
        public bool IsDirty
        {
            get;
            set;
        }

        public override int Count => Items.Count(x => x.Value.Stack != 0);

        protected override void OnItemStackChanged(MerchantItem item, int difference)
        {
            IsDirty = true;
            InventoryHandler.SendExchangeShopStockMovementUpdatedMessage(Owner.OpenDialogs.Select(x => x.Character).ToClients(), item);

            base.OnItemStackChanged(item, difference);
        }

        protected override void OnItemAdded(MerchantItem item, bool sendMessage = true)
        {
            IsDirty = true;
            InventoryHandler.SendExchangeShopStockMovementUpdatedMessage(Owner.OpenDialogs.Select(x => x.Character).ToClients(), item);

            base.OnItemAdded(item, sendMessage);
        }

        public override int RemoveItem(MerchantItem item, int amount, bool delete = true, bool sendMessage = true)
        {
            if (!HasItem(item) || item.Stack == 0)
                return 0;

            if (item.Stack <= amount)
            {
                var removed = item.Stack;

                item.StackSold += removed;
                item.Stack = 0;

                NotifyItemRemoved(item, sendMessage);

                return (int)removed;
            }

            UnStackItem(item, amount, sendMessage);
            return amount;
        }

        public override void UnStackItem(MerchantItem item, int amount, bool sendMessage = true)
        {
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");

            if (item.Stack - amount <= 0)
            {
                item.StackSold += item.Stack;
                item.Stack = 0;

                NotifyItemRemoved(item, sendMessage);
            }
            else
            {
                item.Stack -= (uint) amount;
                item.StackSold += (uint) amount;

                NotifyItemStackChanged(item, -amount);
            }
        }

        protected override void OnItemRemoved(MerchantItem item, bool sendMessage = true)
        {
            IsDirty = true;
            InventoryHandler.SendExchangeShopStockMovementRemovedMessage(Owner.OpenDialogs.Select(x => x.Character).ToClients(), item);

            if (Count == 0)
                Owner.Delete();

            base.OnItemRemoved(item, sendMessage);
        }

        public void LoadRecord()
        {
            var records = ItemManager.Instance.FindPlayerMerchantItems(Owner.Id);
            Items = records.Select(entry => new MerchantItem(entry)).ToDictionary(entry => entry.Guid);
        }

        public override void Save()
        {
            base.Save();

            IsDirty = false;
        }
    }
}