// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.BaseCore.Framework.Extensions;
using Stump.Database;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Effects;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Handlers;

namespace Stump.Server.WorldServer.Items
{
    /// <summary>
    ///   Represent the Inventory of a character
    /// </summary>
    public class Inventory : IOwned
    {
        #region Events

        public delegate void ItemAddedEventHandler(Inventory sender, Item item);
        public delegate void ItemRemovedEventHandler(Inventory sender, long guid);
        public delegate void ItemMovedEventHandler(Inventory sender, Item item, CharacterInventoryPositionEnum oldPosition);
        public delegate void ItemStackChangedEventHandler(Inventory sender, Item item, uint oldStack);

        public event ItemAddedEventHandler ItemAdded;

        public void NotifyItemAdded(Item item)
        {
            ItemAddedEventHandler handler = ItemAdded;
            if (handler != null) handler(this, item);
        }

        public event ItemRemovedEventHandler ItemRemoved;

        public void NotifyItemRemoved(long guid)
        {
            ItemRemovedEventHandler handler = ItemRemoved;
            if (handler != null) handler(this, guid);
        }

        public event ItemMovedEventHandler ItemMoved;

        public void NotifyItemMoved(Item item, CharacterInventoryPositionEnum oldPosition)
        {
            ItemMovedEventHandler handler = ItemMoved;
            if (handler != null) handler(this, item, oldPosition);
        }

        public event ItemStackChangedEventHandler ItemStackChanged;

        public void NotifyItemStackChanged(Item item, uint oldstack)
        {
            ItemStackChangedEventHandler handler = ItemStackChanged;
            if (handler != null) handler(this, item, oldstack);
        }

        #endregion

        private readonly object m_sync = new object();
        private Dictionary<long, Item> m_items = new Dictionary<long, Item>();

        public Inventory(Character owner)
        {
            Owner = owner;

            ItemMoved += OnItemMove;
        }

        public Item[] Items
        {
            get { return m_items.Values.ToArray(); }
        }

        public Item this[long guid]
        {
            get
            {
                Item item;
                m_items.TryGetValue(guid, out item);
                return item;
            }
        }

        public uint Weight
        {
            get
            {
                return m_items.Values.Aggregate<Item, uint>(0,
                                                            (current, item) =>
                                                            current + (uint) item.Template.Weight*item.Stack);
            }
        }

        public uint WeightTotal
        {
            get { return 1000; }
        }

        public uint WeaponCriticalHit
        {
            get
            {
                Item weapon;
                if ((weapon = GetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON)) != null)
                {
                    return weapon.Template is WeaponTemplate
                               ? (uint) (weapon.Template as WeaponTemplate).CriticalHitBonus
                               : 0;
                }

                return 0;
            }
        }

        #region IOwned Members

        Entity IOwned.Owner
        {
            get { return Owner; }
        }

        public Character Owner
        {
            get;
            set;
        }

        #endregion

    

        public void LoadInventory()
        {
            lock (m_sync)
            {
                CharacterItemRecord[] records = CharacterItemRecord.FindItemsByCharacter(Owner.Id);

                m_items = ItemManager.LoadItem(Owner, records).ToDictionary(entry => entry.Guid);
            }
        }

        public void UnLoadInventory()
        {
            lock (m_sync)
            {
                ItemManager.UnLoadItem(m_items.Keys.ToArray());
            }
        }

        public void ApplyItemsEffect()
        {
            lock (m_sync)
            {
                Owner.Stats.ClearAllEquipedEffect();

                foreach (Item item in GetEquipedItems())
                {
                    for (int i = 0; i < item.Effects.Count; i++)
                    {
                        if (!Owner.Stats.TryAddEffect(item.Effects[i], EffectContext.Inventory))
                        {
                            // cannot apply effect, have we to log error ? 
                        }
                    }
                }
            }
        }

        public Item AddItem(ItemTemplate itemTemplate, uint amount)
        {
            List<EffectBase> effects = ItemManager.GenerateItemEffect(itemTemplate);

            Item stack = null;
            if (IsStackable(itemTemplate.Id, effects, out stack) && stack != null)
                // if there is same item in inventory we stack it
            {
                StackItem(stack, amount);
                return stack;
            }


            Item newitem = ItemManager.Create(itemTemplate.Id, Owner, amount, effects);

            lock (m_sync)
            {
                if (m_items.ContainsKey(newitem.Guid))
                {
                    DeleteItem(newitem.Guid);
                    return null;
                }

                m_items.Add(newitem.Guid, newitem);

                NotifyItemAdded(newitem);
            }

            InventoryHandler.SendObjectAddedMessage(Owner.Client, newitem);
            return newitem;
        }

        public Item AddItem(int itemId, uint amount)
        {
            List<EffectBase> effects = ItemManager.GenerateItemEffect(ItemManager.GetTemplate(itemId));

            Item stack = null;
            if (IsStackable(itemId, effects, out stack) && stack != null)
            // if there is same item in inventory we stack it
            {
                StackItem(stack, amount);
                return stack;
            }


            Item newitem = ItemManager.Create(itemId, Owner, amount, effects);

            lock (m_sync)
            {
                if (m_items.ContainsKey(newitem.Guid))
                {
                    DeleteItem(newitem.Guid);
                    return null;
                }

                m_items.Add(newitem.Guid, newitem);

                NotifyItemAdded(newitem);
            }

            InventoryHandler.SendObjectAddedMessage(Owner.Client, newitem);
            return newitem;
        }

        public Item AddItemCopy(Item item, uint amount)
        {
            Item stack = null;
            if (IsStackable(item.ItemId, item.Effects, out stack) && stack != null)
                // if there is same item in inventory we stack it
            {
                StackItem(stack, amount);
                return stack;
            }

            Item newitem = ItemManager.RegisterAnItemCopy(item, Owner, amount);

            lock (m_sync)
            {
                if (m_items.ContainsKey(newitem.Guid))
                {
                    DeleteItem(newitem.Guid);
                    return null;
                }

                m_items.Add(newitem.Guid, newitem);

                NotifyItemAdded(newitem);
            }

            InventoryHandler.SendObjectAddedMessage(Owner.Client, newitem);
            return newitem;
        }

        public void DeleteItem(long guid)
        {
            if (m_items.ContainsKey(guid))
            {
                DeleteItem(guid, m_items[guid].Stack);
            }
            else return;
        }

        public void DeleteItem(long guid, uint amount)
        {
            lock (m_sync)
            {
                if (m_items.ContainsKey(guid))
                {
                    if (m_items[guid].Stack > amount)
                    {
                        UnStackItem(m_items[guid], amount);
                    }
                    else
                    {
                        m_items.Remove(guid);

                        ItemManager.RemoveItem(guid);
                        InventoryHandler.SendObjectDeletedMessage(Owner.Client, guid);

                        NotifyItemRemoved(guid);
                    }
                }
                else return;
            }
        }

        public void MoveItem(long guid, CharacterInventoryPositionEnum position)
        {
            if (!m_items.ContainsKey(guid))
                return;

            var oldPosition = m_items[guid].Position;

            Item equipedItem = null;
            if (position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED &&
                // check if an item is already on the desired position
                ((equipedItem = GetItem(position)) != null))
            {
                // if there is one we move it to the inventory
                MoveItem(equipedItem.Guid, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
            }

            Item itemToMove = m_items[guid];
            if (itemToMove.Stack > 1) // if the item to move is stack we cut it
            {
                Item newitem = CutItem(itemToMove, itemToMove.Stack - 1);
                // now we have 2 stack : itemToMove, stack = 1
                //						 newitem, stack = itemToMove.Stack - 1
            }

            Item stacktoitem = null;
            if (position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED &&
                IsStackable(itemToMove.Template.Id, itemToMove.Effects, out stacktoitem) && stacktoitem != null)
                // check if we must stack the moved item
            {
                StackItem(stacktoitem, itemToMove.Stack); // in all cases Stack = 1 else there is an error
                DeleteItem(itemToMove.Guid);
            }
            else // else we just move the item
            {
                

                itemToMove.Position = position;
                InventoryHandler.SendObjectMovementMessage(Owner.Client, itemToMove);

                itemToMove.Save();

                
            }

            NotifyItemMoved(itemToMove, oldPosition);
        }

        // TODO : Get binded skin to item
        private void OnItemMove(Inventory sender, Item item, CharacterInventoryPositionEnum oldPosition)
        {
            // Update entity skin
            List<uint> newskins = Owner.Skins.Take(1).ToList();

            Item hatItem;
            if ((hatItem = GetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT)) != null)
                newskins.Add((uint)hatItem.Template.AppearanceId);

            Item capeItem;
            if ((capeItem = GetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_CAPE)) != null)
                newskins.Add((uint)capeItem.Template.AppearanceId);

            Item weaponItem;
            if ((weaponItem = GetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON)) != null)
                newskins.Add((uint)weaponItem.Template.AppearanceId);

            Item petsItem;
            if ((petsItem = GetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS)) != null)
                newskins.Add((uint)petsItem.Template.AppearanceId);

            Item shieldItem;
            if ((shieldItem = GetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD)) != null)
                newskins.Add((uint)shieldItem.Template.AppearanceId);

            Owner.Skins = newskins;

            ApplyItemsEffect();
            InventoryHandler.SendInventoryWeightMessage(Owner.Client);
            CharacterHandler.SendCharacterStatsListMessage(Owner.Client);
        }

        public void ChangeItemOwner(Character newOwner, long guid, uint amount)
        {
            if (!m_items.ContainsKey(guid))
                return;

            Item itemToMove = m_items[guid];

            if (amount > itemToMove.Stack)
                amount = itemToMove.Stack;

            newOwner.Inventory.AddItemCopy(itemToMove, amount);

            // delete the item if there is no more stack else we unstack it
            if (amount >= itemToMove.Stack)
            {
                DeleteItem(guid);
            }
            else
            {
                UnStackItem(itemToMove, amount);
            }
        }

        public void StackItem(Item item, uint amount)
        {
            var oldStack = item.Stack;

            item.StackItem(amount);
            InventoryHandler.SendObjectQuantityMessage(Owner.Client, item);

            item.Save();

            NotifyItemStackChanged(item, oldStack);
        }


        public void UnStackItem(Item item, uint amount)
        {
            var oldStack = item.Stack;

            item.UnStackItem(amount);
            InventoryHandler.SendObjectQuantityMessage(Owner.Client, item);

            item.Save();

            NotifyItemStackChanged(item, oldStack);
        }

        public Item CutItem(Item item, uint amount)
        {
            if (amount >= item.Stack)
                return item;

            UnStackItem(item, amount);

            Item newitem = ItemManager.RegisterAnItemCopy(item, Owner, amount);

            lock (m_sync)
            {
                m_items.Add(newitem.Guid, newitem);
            }


            InventoryHandler.SendObjectAddedMessage(Owner.Client, newitem);
            return newitem;
        }

        public bool IsStackable(int itemId, List<EffectBase> effects, out Item outStack)
        {
            Item stack;
            if ((stack = GetItem(itemId, effects, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)) !=
                null)
            {
                outStack = stack;
                return true;
            }

            outStack = null;
            return false;
        }

        public bool HasItem(long guid)
        {
            return GetItem(guid) != null;
        }

        public Item GetItem(long guid)
        {
            if (!m_items.ContainsKey(guid))
                return null;

            return m_items[guid];
        }

        public Item GetItem(int itemId, List<EffectBase> effects, CharacterInventoryPositionEnum position)
        {
            IEnumerable<Item> entrys = from entry in m_items
                                       where entry.Value.ItemId == itemId &&
                                             entry.Value.Position == position &&
                                             effects.CompareEnumerable(entry.Value.Effects)
                                       select entry.Value;

            if (entrys.Count() <= 0)
                return null;

            return entrys.First();
        }

        public Item GetItem(CharacterInventoryPositionEnum position)
        {
            Item[] items = GetItems(position);

            if (items.Count() <= 0)
                return null;

            return items.First();
        }

        public Item[] GetItems(CharacterInventoryPositionEnum position)
        {
            return (from entry in m_items
                    where entry.Value.Position == position
                    select entry.Value).ToArray();
        }

        public Item[] GetEquipedItems()
        {
            return (from entry in m_items
                    where entry.Value.Position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED
                    select entry.Value).ToArray();
        }
    }
}