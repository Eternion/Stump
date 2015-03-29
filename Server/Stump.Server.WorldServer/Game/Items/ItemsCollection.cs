using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Extensions;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Items
{
    public class PersistantItemsCollection<T> : ItemsCollection<T> where T : IPersistantItem
    {
        public virtual void Save()
        {
            lock (Locker)
            {
                var database = WorldServer.Instance.DBAccessor.Database;
                foreach (var item in Items.Where(item => !item.Value.IsTemporarily))
                {
                    if (item.Value.Record.IsNew)
                    {
                        database.Insert(item.Value.Record);
                        item.Value.Record.IsNew = false;
                    }
                    else if (item.Value.Record.IsDirty)
                    {
                        database.Update(item.Value.Record);
                    }
                }

                while (ItemsToDelete.Count > 0)
                {
                    var item = ItemsToDelete.Dequeue();

                    database.Delete(item.Record);
                }
            }
        }
    }

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

        public void NotifyItemAdded(T item, bool addItemMsg)
        {
            OnItemAdded(item, addItemMsg);

            var handler = ItemAdded;
            if (handler != null)
                handler(this, item);
        }

        protected virtual void OnItemAdded(T item, bool addItemMsg)
        {
        }

        public event ItemRemovedEventHandler ItemRemoved;

        public void NotifyItemRemoved(T item, bool removeItemMsg)
        {
            OnItemRemoved(item, removeItemMsg);

            var handler = ItemRemoved;
            if (handler != null)
                handler(this, item);
        }

        protected virtual void OnItemRemoved(T item, bool removeItemMsg)
        {
        }

        public event ItemStackChangedEventHandler ItemStackChanged;

        public void NotifyItemStackChanged(T item, int difference, bool removeMsg = true)
        {
            OnItemStackChanged(item, difference, removeMsg);

            var handler = ItemStackChanged;
            if (handler != null)
                handler(this, item, difference);
        }

        protected virtual void OnItemStackChanged(T item, int difference, bool removeMsg = true)
        {
        }

        #endregion

        /// <summary>
        /// Add an item to the collection
        /// </summary>
        /// <param name="item"></param>
        /// <param name="addItemMsg"></param>
        /// <returns></returns>
        public virtual T AddItem(T item, bool addItemMsg = true)
        {
            if (HasItem(item))
                throw new Exception("Cannot add an item that is already in the collection");

            lock (Locker)
            {
                T stackableWith;
                if (IsStackable(item, out stackableWith))
                {
                    StackItem(stackableWith, (int)item.Stack, addItemMsg);
                    DeleteItem(item);

                    return stackableWith;
                }

                Items.Add(item.Guid, item);

                NotifyItemAdded(item, addItemMsg);
            }

            return item;
        }

        /// <summary>
        /// Remove an item from the collection
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <param name="delete"></param>
        public virtual int RemoveItem(T item, int amount, bool delete = true)
        {
            if (!HasItem(item))
                return 0;

            if (item.Stack <= amount)
            {
                RemoveItem(item, delete);
                return (int)item.Stack;
            }

            UnStackItem(item, amount);
            return amount;
        }

        /// <summary>
        /// Remove an item from the collection
        /// </summary>
        /// <param name="item"></param>
        /// <param name="delete"></param>
        /// <param name="removeItemMsg"></param>
        public virtual bool RemoveItem(T item, bool delete = true, bool removeItemMsg = true)
        {
            if (!HasItem(item))
                return false;

            lock (Locker)
            {
                var deleted = Items.Remove(item.Guid);

                if (delete)
                    DeleteItem(item);

                if (deleted)
                    NotifyItemRemoved(item, removeItemMsg);

                return deleted;
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
                NotifyItemRemoved(item, true);
            }

            ItemsToDelete.Enqueue(item);
        }

        /// <summary>
        /// Increase the stack of an item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        public virtual void StackItem(T item, int amount, bool stackMsg = true)
        {
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");

            item.Stack += (uint)amount;

            NotifyItemStackChanged(item, amount, stackMsg);
        }

        /// <summary>
        /// Decrease the stack of an item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        public virtual void UnStackItem(T item, int amount, bool stackMsg = true)
        {
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");

            if (item.Stack - amount <= 0)
                RemoveItem(item, true, stackMsg);
            else
            {
                item.Stack -= (uint)amount;

                NotifyItemStackChanged(item, -amount, stackMsg);
            }
        }

        public void DeleteAll(bool notify = true)
        {
            if (notify)
                foreach(var item in this)
                    NotifyItemRemoved(item, true);

            ItemsToDelete = new Queue<T>(ItemsToDelete.Concat(Items.Values));
            Items.Clear();
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