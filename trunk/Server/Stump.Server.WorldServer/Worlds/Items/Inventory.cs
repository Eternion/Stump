using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Handlers.Characters;
using Stump.Server.WorldServer.Handlers.Inventory;
using Stump.Server.WorldServer.Worlds.Actors.Interfaces;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Effects;
using Stump.Server.WorldServer.Worlds.Effects.Instances;

namespace Stump.Server.WorldServer.Worlds.Items
{
    /// <summary>
    ///   Represent the Inventory of a character
    /// </summary>
    public class Inventory : ItemsStorage, IDisposable
    {
        #region Events

        #region Delegates

        public delegate void ItemMovedEventHandler(Inventory sender, Item item, CharacterInventoryPositionEnum lastPosition);

        #endregion

        public event ItemMovedEventHandler ItemMoved;

        public void NotifyItemMoved(Item item, CharacterInventoryPositionEnum lastPosition)
        {
            OnItemMoved(item, lastPosition);

            ItemMovedEventHandler handler = ItemMoved;
            if (handler != null) handler(this, item, lastPosition);
        }


        #endregion

        internal readonly InventoryRecord Record;

        private readonly Dictionary<CharacterInventoryPositionEnum, List<Item>> m_itemsByPosition
            = new Dictionary<CharacterInventoryPositionEnum, List<Item>>
                  {
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT, new List<Item>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_CAPE, new List<Item>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_BELT, new List<Item>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_BOOTS, new List<Item>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_AMULET, new List<Item>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD, new List<Item>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON, new List<Item>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS, new List<Item>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_LEFT, new List<Item>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT, new List<Item>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_1, new List<Item>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_2, new List<Item>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_3, new List<Item>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_4, new List<Item>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_5, new List<Item>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_6, new List<Item>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_MOUNT, new List<Item>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_MUTATION, new List<Item>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_BOOST_FOOD, new List<Item>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_FIRST_BONUS, new List<Item>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_SECOND_BONUS, new List<Item>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_FIRST_MALUS, new List<Item>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_SECOND_MALUS, new List<Item>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_ROLEPLAY_BUFFER, new List<Item>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_FOLLOWER, new List<Item>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, new List<Item>()},
                  };

        private readonly object m_locker = new object();

        public Inventory(Character owner, InventoryRecord record)
        {
            Owner = owner;
            Record = record;

            LoadInventory();
        }

        public IEnumerable<Item> Items
        {
            get { return m_items.Values; }
        }

        public Item this[int guid]
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
                                                            (uint) (current + item.Template.Weight*item.Stack));
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

        public Character Owner
        {
            get;
            private set;
        }

        /// <summary>
        ///   Amount of kamas owned by this character.
        /// </summary>
        public override int Kamas
        {
            get { return Record.Kamas; }
            protected set { Record.Kamas = value; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            UnLoadInventory();
        }

        #endregion

        private void LoadInventory()
        {
            m_items = Record.Items.Select(i => ItemManager.Instance.LoadItem(i)).ToDictionary(entry => entry.Guid);
            foreach (Item item in m_items.Values)
            {
                m_itemsByPosition[item.Position].Add(item);
            }
        }

        private void UnLoadInventory()
        {
            ItemManager.Instance.UnLoadItems(m_items.Keys.ToArray());
            m_items.Clear();
            foreach (var item in m_itemsByPosition)
            {
                m_itemsByPosition[item.Key].Clear();
            }
        }

        public void Save()
        {
            Record.Save();
            foreach (var item in m_items)
            {
                item.Value.Save();
            }
        }

        /*public void ApplyItemsEffect()
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
        }*/

        private void ApplyItemEffects(Item item)
        {
            foreach (var effect in item.Effects)
            {
                var handler = EffectManager.Instance.GetItemEffectHandler(Owner, item, effect);

                handler.Apply();
            }
        }

        protected override void OnItemAdded(Item item)
        {
            lock (m_locker)
                m_itemsByPosition[item.Position].Add(item);

            Record.Items.Add(item.Record);
            item.Record.Inventory = Record;

            item.Record.Update();

            InventoryHandler.SendObjectAddedMessage(Owner.Client, item);
            InventoryHandler.SendInventoryWeightMessage(Owner.Client);

            base.OnItemAdded(item);
        }

        protected override void OnItemRemoved(Item item)
        {
            lock (m_locker)
                m_itemsByPosition[item.Position].Remove(item);

            Record.Items.Remove(item.Record);
            item.Record.Inventory = null;

            item.Record.Update();

            InventoryHandler.SendObjectDeletedMessage(Owner.Client, item.Guid);
            InventoryHandler.SendInventoryWeightMessage(Owner.Client);

            base.OnItemRemoved(item);
        }

        protected virtual void OnItemMoved(Item item, CharacterInventoryPositionEnum lastPosition)
        {
            m_itemsByPosition[lastPosition].Remove(item);
            m_itemsByPosition[item.Position].Add(item);

            // Update entity skin
            if (lastPosition != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED && item.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED ||
                lastPosition == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED && item.Position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                ApplyItemEffects(item);

            InventoryHandler.SendObjectMovementMessage(Owner.Client, item);
            InventoryHandler.SendInventoryWeightMessage(Owner.Client);
            CharacterHandler.SendCharacterStatsListMessage(Owner.Client);
        }

        protected override void OnItemStackChanged(Item item, int difference)
        {
            InventoryHandler.SendObjectQuantityMessage(Owner.Client, item);

            base.OnItemStackChanged(item, difference);
        }

        protected override void OnKamasAmountChanged(int amount)
        {
            InventoryHandler.SendKamasUpdateMessage(Owner.Client, amount);

            base.OnKamasAmountChanged(amount);
        }

        public void MoveItem(int guid, CharacterInventoryPositionEnum position)
        {
            if (!m_items.ContainsKey(guid))
                return;

            CharacterInventoryPositionEnum oldPosition = m_items[guid].Position;

            Item equipedItem;
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
                CutItem(itemToMove, (uint) (itemToMove.Stack - 1));
                // now we have 2 stack : itemToMove, stack = 1
                //						 newitem, stack = itemToMove.Stack - 1
            }

            Item stacktoitem;
            if (position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED &&
                IsStackable(itemToMove.Template.Id, itemToMove.Effects, out stacktoitem) && stacktoitem != null)
                // check if we must stack the moved item
            {
                StackItem(stacktoitem, (uint) itemToMove.Stack); // in all cases Stack = 1 else there is an error
                RemoveItem(itemToMove.Guid);
            }
            else // else we just move the item
            {
                itemToMove.Position = position;

                NotifyItemMoved(itemToMove, oldPosition);
            }
        }

        public void ChangeItemOwner(Character newOwner, int guid, uint amount)
        {
            if (!m_items.ContainsKey(guid))
                return;

            Item itemToMove = m_items[guid];

            if (amount > itemToMove.Stack)
                amount = (uint) itemToMove.Stack;

            newOwner.Inventory.AddItemCopy(itemToMove, amount);

            // delete the item if there is no more stack else we unstack it
            if (amount >= itemToMove.Stack)
            {
                RemoveItem(guid);
            }
            else
            {
                UnStackItem(itemToMove, amount);
            }
        }
    }
}