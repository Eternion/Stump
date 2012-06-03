using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Collections;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Items
{
    public class ItemsCollection<T> : IEnumerable<T> where T : IItem
    {
        #region Events

        #region Delegates

        public delegate void ItemAddedEventHandler(ItemsCollection<T> sender, T item);

        public delegate void ItemRemovedEventHandler(ItemsCollection<T> sender, T item);

        public delegate void ItemStackChangedEventHandler(ItemsCollection<T> sender, T item, int difference);

        #endregion

        protected ItemsCollection()
        {
            Locker = new object();
            Items = new Dictionary<int, T>();
            ItemsToDelete = new Queue<T>();
        }

        protected object Locker
        {
            get;
            set;
        }

        protected Dictionary<int, T> Items
        {
            get;
            set;
        }

        protected Queue<T> ItemsToDelete
        {
            get;
            set;
        }

        public int Count
        {
            get { return Items.Count; }
        }

        public event ItemAddedEventHandler ItemAdded;

        public void NotifyItemAdded(T item)
        {
            OnItemAdded(item);

            ItemAddedEventHandler handler = ItemAdded;
            if (handler != null)
                handler(this, item);
        }

        protected virtual void OnItemAdded(T item)
        {
        }

        public event ItemRemovedEventHandler ItemRemoved;

        public void NotifyItemRemoved(T item)
        {
            OnItemRemoved(item);

            ItemRemovedEventHandler handler = ItemRemoved;
            if (handler != null)
                handler(this, item);
        }

        protected virtual void OnItemRemoved(T item)
        {
        }

        public event ItemStackChangedEventHandler ItemStackChanged;

        public void NotifyItemStackChanged(T item, int difference)
        {
            OnItemStackChanged(item, difference);

            ItemStackChangedEventHandler handler = ItemStackChanged;
            if (handler != null)
                handler(this, item, difference);
        }

        protected virtual void OnItemStackChanged(T item, int difference)
        {
        }

        #endregion

        /*public virtual T AddItem(int itemId, uint amount)
        {
            ItemTemplate itemTemplate = ItemManager.Instance.TryGetTemplate(itemId);

            return itemTemplate != null ? AddItem(itemTemplate, amount) : null;
        }

        public virtual T AddItem(ItemTemplate template, uint amount, bool maxEffect = false)
        {
            List<EffectBase> effects = ItemManager.Instance.GenerateItemEffects(template, maxEffect);

            PlayerItem stackableWith;
            if (IsStackable(template, effects, out stackableWith))
            {
                StackItem(stackableWith, amount);

                return stackableWith;
            }

            PlayerItem item = ItemManager.Instance.CreatePlayerItem(template, amount, effects);

            lock (m_locker)
                m_items.Add(item.Guid, item);

            NotifyItemAdded(item);

            return item;
        }*/

        /// <summary>
        /// Add an item to the collection
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual T AddItem(T item)
        {
            if (HasItem(item))
                throw new Exception("Cannot add an item that is already in the collection");

            lock (Locker)
            {
                T stackableWith;
                if (IsStackable(item, out stackableWith))
                {
                    StackItem(stackableWith, item.Stack);
                    DeleteItem(item);

                    return stackableWith;
                }

                Items.Add(item.Guid, item);

                NotifyItemAdded(item);
            }

            return item;
        }

        /// <summary>
        /// Remove an item from the collection
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <param name="delete"></param>
        public virtual void RemoveItem(T item, uint amount, bool delete = true)
        {
            if (!HasItem(item))
                throw new Exception("Cannot remove an item that is not in the collection");

            if (item.Stack <= amount)
                RemoveItem(item, delete);
            else
            {
                UnStackItem(item, (int) amount);
            }
        }

        /// <summary>
        /// Remove an item from the collection
        /// </summary>
        /// <param name="item"></param>
        /// <param name="delete"></param>
        public virtual void RemoveItem(T item, bool delete = true)
        {
            if (!HasItem(item))
                throw new Exception("Cannot remove an item that is not in the collection");

            lock (Locker)
            {
                Items.Remove(item.Guid);

                if (delete)
                    DeleteItem(item);

                NotifyItemRemoved(item);
            }
        }


        /// <summary>
        /// Delete an item persistently.
        /// </summary>
        protected virtual void DeleteItem(T item)
        {
            // theorically the item is removed before
            if (Items.ContainsKey(item.Guid))
            {
                Items.Remove(item.Guid);
                NotifyItemRemoved(item);
            }

            ItemsToDelete.Enqueue(item);
        }
    
        /// <summary>
        /// Increase the stack of an item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        public virtual void StackItem(T item, int amount)
        {
            if (amount < 0)
                UnStackItem(item, -amount);
            else
            {
                item.Stack += amount;

                NotifyItemStackChanged(item, amount);
            }
        }

        /// <summary>
        /// Decrease the stack of an item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        public virtual void UnStackItem(T item, int amount)
        {
            if (item.Stack - amount <= 0)
                RemoveItem(item);
            else
            {
                item.Stack -= amount;

                NotifyItemStackChanged(item, -amount);
            }
        }
        /*
        /// <summary>
        /// Cut an item into two parts
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public T CutItem(T item, uint amount)
        {
            if (amount >= item.Stack)
                return item;

            UnStackItem(item, (int) amount);

            var newitem = ItemManager.Instance.RegisterAnItemCopy(item, amount);

            Items.Add(newitem.Guid, newitem);

            NotifyItemAdded(newitem);

            return newitem;
        }*/

        public virtual void Save()
        {
            lock (Locker)
            {
                foreach (var item in Items)
                {
                    item.Value.Record.Save();
                }

                while (ItemsToDelete.Count > 0)
                {
                    var item = ItemsToDelete.Dequeue();

                    item.Record.Delete();
                }
            }
        }

        public virtual bool IsStackable(T item, out T stackableWith)
        {
            T stack;
            if (( stack = TryGetItem(item.Template, item.Effects) ) != null)
            {
                stackableWith = stack;
                return true;
            }

            stackableWith = default(T);
            return false;
        }

        public bool HasItem(int guid)
        {
            return Items.ContainsKey(guid);
        }

        public bool HasItem(ItemTemplate template)
        {
            return Items.Any(entry => entry.Value.Template.Id == template.Id);
        }

        public bool HasItem(T item)
        {
            return HasItem(item.Guid);
        }

        public T TryGetItem(int guid)
        {
            return !Items.ContainsKey(guid) ? default(T) : Items[guid];
        }

        public T TryGetItem(ItemTemplate template)
        {
            IEnumerable<T> entries = from entry in Items.Values
                                     where entry.Template.Id == template.Id
                                     select entry;

            return entries.FirstOrDefault();
        }

        public T TryGetItem(ItemTemplate template, IEnumerable<EffectBase> effects)
        {   
            IEnumerable<T> entries = from entry in Items.Values
                                        where entry.Template.Id == template.Id && effects.CompareEnumerable(entry.Effects)
                                        select entry;

            return entries.FirstOrDefault();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}