using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Characters;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Items
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

        private readonly Queue<ItemRecord> m_itemsToDelete = new Queue<ItemRecord>();

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

        private readonly Dictionary<ItemSuperTypeEnum, CharacterInventoryPositionEnum[]> m_itemsPositioningRules
            = new Dictionary<ItemSuperTypeEnum, CharacterInventoryPositionEnum[]>
          {
              {ItemSuperTypeEnum.SUPERTYPE_AMULET, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_AMULET}},
              {ItemSuperTypeEnum.SUPERTYPE_WEAPON, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON}},
              {ItemSuperTypeEnum.SUPERTYPE_WEAPON_7, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON}},
              {ItemSuperTypeEnum.SUPERTYPE_CAPE, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_CAPE}},
              {ItemSuperTypeEnum.SUPERTYPE_HAT, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT}},
              {ItemSuperTypeEnum.SUPERTYPE_RING, new [] {CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_LEFT, CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT}},
              {ItemSuperTypeEnum.SUPERTYPE_BOOTS, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_BOOTS}},
              {ItemSuperTypeEnum.SUPERTYPE_BELT, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_BELT}},
              {ItemSuperTypeEnum.SUPERTYPE_DOFUS, new [] {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_1, CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_2, CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_3, CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_4, CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_5, CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_6}},
              {ItemSuperTypeEnum.SUPERTYPE_SHIELD, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD}},

          };

        public Inventory(Character owner)
        {
            Owner = owner;
        }

        public IEnumerable<Item> Items
        {
            get { return m_items.Values; }
        }

        public int Count
        {
            get { return m_items.Count; }
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
                if ((weapon = TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON)) != null)
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
            get { return Owner.Kamas; }
            protected set
            {
                Owner.Kamas = value;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            UnLoadInventory();
        }

        #endregion

        internal void LoadInventory()
        {
            var records = ItemRecord.FindAllByOwner(Owner.Id);

            m_items = records.Select(entry => new Item(entry)).ToDictionary(entry => entry.Guid);
            foreach (Item item in m_items.Values)
            {
                m_itemsByPosition[item.Position].Add(item);

                if (item.IsEquiped())
                    ApplyItemEffects(item, false);
            }
        }

        internal void UnLoadInventory()
        {
            m_items.Clear();
            foreach (var item in m_itemsByPosition)
            {
                m_itemsByPosition[item.Key].Clear();
            }
        }

        public void Save()
        {
            lock (m_locker)
            {
                foreach (var item in m_items)
                {
                    item.Value.Record.Save();
                }

                while (m_itemsToDelete.Count > 0)
                {
                    var record = m_itemsToDelete.Dequeue();
                    record.Delete();
                }
            }
        }

        private void ApplyItemEffects(Item item, bool send = true)
        {
            foreach (var effect in item.Effects)
            {
                var handler = EffectManager.Instance.GetItemEffectHandler(effect, Owner, item);

                handler.Apply();
            }

            if (send)
                Owner.RefreshStats();
        }

        private void ApplyItemSetEffects(ItemSetTemplate itemSet, int count, bool apply, bool send = true)
        {
            var effects = itemSet.GetEffects(count);

            foreach (var effect in effects)
            {
                var handler = EffectManager.Instance.GetItemEffectHandler(effect, Owner, itemSet, apply);

                handler.Apply();
            }

            if (send)
                Owner.RefreshStats();
        }

        public int CountItemSetEquiped(ItemSetTemplate itemSet)
        {
            return GetEquipedItems().Count(entry => itemSet.Items.Contains(entry.Template));
        }

        public Item[] GetItemSetEquipped(ItemSetTemplate itemSet)
        {
            return GetEquipedItems().Where(entry => itemSet.Items.Contains(entry.Template)).ToArray();
        }

        public EffectBase[] GetItemSetEffects(ItemSetTemplate itemSet)
        {
            return itemSet.GetEffects(CountItemSetEquiped(itemSet));
        }

        public short[] GetItemsSkins()
        {
            return GetEquipedItems().Where(entry => entry.Position != CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS && entry.Template.AppearanceId != 0).Select(entry => (short)entry.Template.AppearanceId).ToArray();
        }

        public short[] GetPetsSkins()
        {
            return GetItems(CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS).Where(entry => entry.Template.AppearanceId != 0).Select(entry => (short)entry.Template.AppearanceId).ToArray();
        }

        protected override void OnItemAdded(Item item)
        {
            m_itemsByPosition[item.Position].Add(item);

            item.Record.OwnerId = Owner.Id;

            if (item.IsEquiped())
                ApplyItemEffects(item);

            InventoryHandler.SendObjectAddedMessage(Owner.Client, item);
            InventoryHandler.SendInventoryWeightMessage(Owner.Client);

            base.OnItemAdded(item);
        }

        protected override void OnItemRemoved(Item item)
        {
            m_itemsByPosition[item.Position].Remove(item);
            m_itemsToDelete.Enqueue(item.Record);

            // not equiped
            bool wasEquiped = item.IsEquiped();
            item.Position = CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;

            if (wasEquiped)
                ApplyItemEffects(item, item.Template.ItemSet == null);

            if (wasEquiped && item.Template.ItemSet != null)
            {
                var count = CountItemSetEquiped(item.Template.ItemSet);

                if (count >= 0)
                    ApplyItemSetEffects(item.Template.ItemSet, count + 1, false);
                if (count > 0)
                    ApplyItemSetEffects(item.Template.ItemSet, count, true);

                InventoryHandler.SendSetUpdateMessage(Owner.Client, item.Template.ItemSet);
            }

            InventoryHandler.SendObjectDeletedMessage(Owner.Client, item.Guid);
            InventoryHandler.SendInventoryWeightMessage(Owner.Client);

            if (wasEquiped)
                CheckItemsCriterias();

            if (wasEquiped && item.Template.AppearanceId != 0)
                Owner.UpdateLook();

            base.OnItemRemoved(item);
        }

        protected virtual void OnItemMoved(Item item, CharacterInventoryPositionEnum lastPosition)
        {
            m_itemsByPosition[lastPosition].Remove(item);
            m_itemsByPosition[item.Position].Add(item);

            bool wasEquiped = lastPosition != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;
            bool isEquiped = item.IsEquiped();

            if (wasEquiped && !isEquiped ||
                !wasEquiped && isEquiped)
                ApplyItemEffects(item, false);

            if (item.Template.ItemSet != null && !(wasEquiped && isEquiped))
            {
                var count = CountItemSetEquiped(item.Template.ItemSet);

                if (count >= 0)
                    ApplyItemSetEffects(item.Template.ItemSet, count + (wasEquiped ? 1 : -1), false);
                if (count > 0)
                    ApplyItemSetEffects(item.Template.ItemSet, count, true, false);

                InventoryHandler.SendSetUpdateMessage(Owner.Client, item.Template.ItemSet);
            }

            if (lastPosition == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED && !item.AreConditionFilled(Owner))
            {
                BasicHandler.SendTextInformationMessage(Owner.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 19);
                MoveItem(item, lastPosition);
            }

            InventoryHandler.SendObjectMovementMessage(Owner.Client, item);
            InventoryHandler.SendInventoryWeightMessage(Owner.Client);

            if (lastPosition != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                CheckItemsCriterias();

            if ((isEquiped || wasEquiped) && item.Template.AppearanceId != 0)
                Owner.UpdateLook();

            Owner.RefreshStats();
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

        public void MoveItem(Item item, CharacterInventoryPositionEnum position)
        {
            if (!HasItem(item))
                return;

            if (!CanEquip(item, position))
                return;

            CharacterInventoryPositionEnum oldPosition = item.Position;

            Item equipedItem;
            if (position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED &&
                // check if an item is already on the desired position
                ((equipedItem = TryGetItem(position)) != null))
            {
                // if there is one we move it to the inventory
                MoveItem(equipedItem, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
            }

            // second check
            if (!HasItem(item))
                return;

            UnEquipedDouble(item);

            if (item.Stack > 1) // if the item to move is stack we cut it
            {
                CutItem(item, (uint)( item.Stack - 1 ));
                // now we have 2 stack : itemToMove, stack = 1
                //						 newitem, stack = itemToMove.Stack - 1
            }

            Item stacktoitem;
            if (position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED &&
                IsStackable(item.Template, item.Effects, out stacktoitem) && stacktoitem != null)
                // check if we must stack the moved item
            {

                NotifyItemMoved(item, oldPosition);
                StackItem(stacktoitem, (uint)item.Stack); // in all cases Stack = 1 else there is an error
                RemoveItem(item);
            }
            else // else we just move the item
            {
                item.Position = position;
                NotifyItemMoved(item, oldPosition);
            }
        }

        private bool UnEquipedDouble(Item itemToEquip)
        {
            if (itemToEquip.Template.Type.ItemType == ItemTypeEnum.DOFUS)
            {
                var dofus = GetEquipedItems().FirstOrDefault(entry => entry.Guid != itemToEquip.Guid && entry.ItemId == itemToEquip.ItemId);

                if (dofus != null)
                {
                    MoveItem(dofus, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

                    return true;
                }
            }

            if (itemToEquip.Template.Type.ItemType == ItemTypeEnum.RING)
            {
                // we can equip the same ring if it doesn't own to an item set
                var ring = GetEquipedItems().FirstOrDefault(entry => entry.Guid != itemToEquip.Guid && entry.ItemId == itemToEquip.ItemId && entry.Template.ItemSetId > 0);

                if (ring != null)
                {
                    MoveItem(ring, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

                    return true;
                }
            }

            return false;
        }

        public bool CanEquip(Item item, CharacterInventoryPositionEnum position, bool send = true)
        {
            if (position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                return true;

            if (!GetItemPossiblePositions(item).Contains(position))
                return false;

            if (item.Template.Level > Owner.Level)
            {
                if (send)
                    BasicHandler.SendTextInformationMessage(Owner.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 3);

                return false;
            }

            var weapon = TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON);
            if (item.Template.Type.ItemType == ItemTypeEnum.SHIELD && weapon != null && weapon.Template.TwoHanded)
            {
                if (send)
                    BasicHandler.SendTextInformationMessage(Owner.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 78);

                return false;
            }

            var shield = TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD);
            if (item.Template is WeaponTemplate && item.Template.TwoHanded && shield != null)
            {
                if (send)
                    BasicHandler.SendTextInformationMessage(Owner.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 79);

                return false;
            }            
            
            return true;
        }

        public CharacterInventoryPositionEnum[] GetItemPossiblePositions(Item item)
        {
            if (!m_itemsPositioningRules.ContainsKey(item.Template.Type.SuperType))
                return new[] { CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED };

            return m_itemsPositioningRules[item.Template.Type.SuperType];
        }

        public void ChangeItemOwner(Character newOwner, Item item, uint amount)
        {
            if (!HasItem(item.Guid))
                return;

            if (amount > item.Stack)
                amount = (uint)item.Stack;

            newOwner.Inventory.AddItemCopy(item, amount);

            // delete the item if there is no more stack else we unstack it
            if (amount >= item.Stack)
            {
                RemoveItem(item);
            }
            else
            {
                UnStackItem(item, amount);
            }
        }

        public void CheckItemsCriterias()
        {
            foreach (var equipedItem in GetEquipedItems().ToArray())
            {
                if (!equipedItem.AreConditionFilled(Owner))
                    MoveItem(equipedItem, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
            }
        }

        public void UseItem(Item item)
        {
            if (!HasItem(item.Guid) || !item.IsUsable())
                return;

            if (!item.AreConditionFilled(Owner))
            {
                return;
            }

            foreach (var effect in item.Effects)
            {
                var handler = EffectManager.Instance.GetUsableEffectHandler(effect, Owner, item);

                handler.Apply();
            }

            RemoveItem(item, 1);
        }
    }
}