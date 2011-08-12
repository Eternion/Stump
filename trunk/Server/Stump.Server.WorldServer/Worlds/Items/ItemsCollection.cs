using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Worlds.Effects.Instances;

namespace Stump.Server.WorldServer.Worlds.Items
{
    public class ItemsCollection
    {
        #region Events

        #region Delegates

        public delegate void ItemAddedEventHandler(ItemsCollection sender, Item item);
        public delegate void ItemRemovedEventHandler(ItemsCollection sender, Item item);
        public delegate void ItemStackChangedEventHandler(ItemsCollection sender, Item item, int difference);

        #endregion

        protected Dictionary<int, Item> m_items = new Dictionary<int, Item>();
        private readonly object m_locker = new object();

        public event ItemAddedEventHandler ItemAdded;

        public void NotifyItemAdded(Item item)
        {
            OnItemAdded(item);

            ItemAddedEventHandler handler = ItemAdded;
            if (handler != null)
                handler(this, item);
        }

        protected virtual void OnItemAdded(Item item)
        {
        }

        public event ItemRemovedEventHandler ItemRemoved;

        public void NotifyItemRemoved(Item item)
        {
            OnItemRemoved(item);

            ItemRemovedEventHandler handler = ItemRemoved;
            if (handler != null)
                handler(this, item);
        }

        protected virtual void OnItemRemoved(Item itemd)
        {
        }

        public event ItemStackChangedEventHandler ItemStackChanged;

        public void NotifyItemStackChanged(Item item, int difference)
        {
            OnItemStackChanged(item, difference);

            ItemStackChangedEventHandler handler = ItemStackChanged;
            if (handler != null)
                handler(this, item, difference);
        }

        protected virtual void OnItemStackChanged(Item item, int difference)
        {
        }

        #endregion

        public virtual Item AddItem(int itemId, uint amount)
        {
            ItemTemplate itemTemplate = ItemManager.Instance.GetTemplate(itemId);

            return itemTemplate != null ? AddItem(itemTemplate, amount) : null;
        }

        public virtual Item AddItem(ItemTemplate template, uint amount)
        {
            Item item = ItemManager.Instance.Create(template, amount);

            return AddItem(item);
        }

        public virtual Item AddItem(Item item)
        {
            Item stackableWith;
            if (IsStackable(item.ItemId, item.Effects, out stackableWith))
            {
                StackItem(stackableWith, (uint) item.Stack);

                if (ItemManager.Instance.IsLoaded(item.Guid))
                    ItemManager.Instance.RemoveItem(item.Guid);

                return stackableWith;
            }

            if (!ItemManager.Instance.IsLoaded(item.Guid))
                ItemManager.Instance.AddItem(item); // theorically it's impossible, but nvm

            if (!item.CanBeSave)
                throw new Exception("Item cannot be stored because his guid equals -1");

            lock (m_locker)
                m_items.Add(item.Guid, item);

            NotifyItemAdded(item);

            return item;
        }

        public virtual Item AddItemCopy(Item item, uint amount)
        {
            Item stack;
            if (IsStackable(item.ItemId, item.Effects, out stack) && stack != null)
                // if there is same item in inventory we stack it
            {
                StackItem(stack, amount);
                return stack;
            }

            Item newitem = ItemManager.Instance.RegisterAnItemCopy(item, amount);


            if (m_items.ContainsKey(newitem.Guid))
            {
                RemoveItem(newitem.Guid);
                return null;
            }

            lock (m_locker)
                m_items.Add(newitem.Guid, newitem);


            NotifyItemAdded(newitem);

            return newitem;
        }

        public virtual void RemoveItem(Item item)
        {
            RemoveItem(item.Guid);
        }

        public virtual void RemoveItem(int guid, uint amount)
        {
            if (!m_items.ContainsKey(guid))
                return;

            if (m_items[guid].Stack <= amount)
                RemoveItem(guid);
            else
            {
                UnStackItem(m_items[guid], amount);
            }
        }

        public virtual void RemoveItem(int guid)
        {
            if (!m_items.ContainsKey(guid))
                return;

            Item removedItem;
            lock (m_locker)
            {
                removedItem = m_items[guid];
                m_items.Remove(guid);

                ItemManager.Instance.RemoveItem(guid);
            }

            NotifyItemRemoved(removedItem);
        }

        public virtual void StackItem(Item item, uint amount)
        {
            item.StackItem(amount);

            NotifyItemStackChanged(item, (int) (item.Stack - amount));
        }

        public virtual void UnStackItem(Item item, uint amount)
        {
            item.UnStackItem(amount);

            NotifyItemStackChanged(item, (int) (item.Stack - amount));
        }

        public virtual Item CutItem(Item item, uint amount)
        {
            if (amount >= item.Stack)
                return item;

            UnStackItem(item, amount);

            Item newitem = ItemManager.Instance.RegisterAnItemCopy(item, amount);

            lock (m_locker)
                m_items.Add(newitem.Guid, newitem);

            NotifyItemAdded(newitem);

            return newitem;
        }

        public virtual bool IsStackable(int itemId, List<EffectBase> effects, out Item stackableWith)
        {
            Item stack;
            if ((stack = GetItem(itemId, effects, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)) !=
                null)
            {
                stackableWith = stack;
                return true;
            }

            stackableWith = null;
            return false;
        }


        public virtual bool HasItem(int guid)
        {
            return GetItem(guid) != null;
        }

        public virtual Item GetItem(int guid)
        {
            return !m_items.ContainsKey(guid) ? null : m_items[guid];
        }

        public virtual Item GetItem(int itemId, List<EffectBase> effects, CharacterInventoryPositionEnum position)
        {
            IEnumerable<Item> entries;
            lock (m_locker)
            {
                entries = from entry in m_items.Values
                          where entry.ItemId == itemId && entry.Position == position && effects.CompareEnumerable(entry.Effects)
                          select entry;
            }

            return entries.FirstOrDefault();
        }

        public virtual Item GetItem(CharacterInventoryPositionEnum position)
        {
            lock (m_locker)
                return m_items.Values.Where(entry => entry.Position == position).FirstOrDefault();
        }

        public virtual IEnumerable<Item> GetItems(CharacterInventoryPositionEnum position)
        {
            lock (m_locker)
                return m_items.Values.Where(entry => entry.Position == position);
        }

        public IEnumerable<Item> GetEquipedItems()
        {
            lock (m_locker)
                return from entry in m_items
                       where entry.Value.IsEquiped()
                       select entry.Value;
        }
    }
}