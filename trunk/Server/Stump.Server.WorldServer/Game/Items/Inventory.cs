using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Core.IPC;
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
    ///   Represents the Inventory of a character
    /// </summary>
    public sealed class Inventory : ItemsStorage<PlayerItem>, IDisposable
    {
        [Variable]
        public static readonly bool ActiveTokens = true;

        [Variable]
        public static readonly int TokenTemplateId = (int)ItemIdEnum.GameMasterToken;
        private static ItemTemplate TokenTemplate;

        [Initialization(typeof(ItemManager), Silent=true)]
        private static void InitializeTokenTemplate()
        {
            if (ActiveTokens)
                TokenTemplate = ItemManager.Instance.TryGetTemplate(TokenTemplateId);
        }

        #region Events

        #region Delegates

        public delegate void ItemMovedEventHandler(Inventory sender, PlayerItem item, CharacterInventoryPositionEnum lastPosition);

        #endregion

        public event ItemMovedEventHandler ItemMoved;

        public void NotifyItemMoved(PlayerItem item, CharacterInventoryPositionEnum lastPosition)
        {
            OnItemMoved(item, lastPosition);

            ItemMovedEventHandler handler = ItemMoved;
            if (handler != null) handler(this, item, lastPosition);
        }


        #endregion

        private readonly Dictionary<CharacterInventoryPositionEnum, List<PlayerItem>> m_itemsByPosition
            = new Dictionary<CharacterInventoryPositionEnum, List<PlayerItem>>
                  {
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_CAPE, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_BELT, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_BOOTS, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_AMULET, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_LEFT, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_1, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_2, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_3, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_4, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_5, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_6, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_MOUNT, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_MUTATION, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_BOOST_FOOD, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_FIRST_BONUS, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_SECOND_BONUS, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_FIRST_MALUS, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_SECOND_MALUS, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_ROLEPLAY_BUFFER, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_FOLLOWER, new List<PlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, new List<PlayerItem>()},
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
              {ItemSuperTypeEnum.SUPERTYPE_PET, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS}},
              {ItemSuperTypeEnum.SUPERTYPE_DOFUS, new [] {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_1, CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_2, CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_3, CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_4, CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_5, CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_6}},
              {ItemSuperTypeEnum.SUPERTYPE_SHIELD, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD}},

          };

        public Inventory(Character owner)
        {
            Owner = owner;
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

        public PlayerItem this[int guid]
        {
            get
            {
                return TryGetItem(guid);
            }
        }

        public int Weight
        {
            get
            {
                int weight = Items.Values.Sum(entry => entry.Weight);

                if (Tokens != null)
                {
                    weight -= Tokens.Weight;
                }

                return weight > 0 ? weight : 0;
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
                PlayerItem weapon;
                if ((weapon = TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON)) != null)
                {
                    return weapon.Template is WeaponTemplate
                               ? (uint) (weapon.Template as WeaponTemplate).CriticalHitBonus
                               : 0;
                }

                return 0;
            }
        }

        public PlayerItem Tokens
        {
            get;
            private set;
        }

        internal void LoadInventory()
        {
            var records = PlayerItemRecord.FindAllByOwner(Owner.Id);

            Items = records.Select(entry => new PlayerItem(Owner, entry)).ToDictionary(entry => entry.Guid);
            foreach (var item in this)
            {
                m_itemsByPosition[item.Position].Add(item);

                if (item.IsEquiped())
                    ApplyItemEffects(item, false);
            }

            foreach (var itemSet in GetEquipedItems().
                Where(entry => entry.Template.ItemSet != null).
                Select(entry => entry.Template.ItemSet).Distinct())
            {
                ApplyItemSetEffects(itemSet, CountItemSetEquiped(itemSet), true, false);
            }

            if (TokenTemplate != null && ActiveTokens && Owner.Account.Tokens > 0)
            {
                Tokens = ItemManager.Instance.CreatePlayerItem(Owner, TokenTemplate, (uint)Owner.Account.Tokens);
                Items.Add(Tokens.Guid, Tokens); // cannot stack
            }
        }

        private void UnLoadInventory()
        {
            Items.Clear();
            foreach (var item in m_itemsByPosition)
            {
                m_itemsByPosition[item.Key].Clear();
            }
        }

        public override void Save()
        {
            lock (Locker)
            {
                foreach (var item in Items)
                {
                    if (Tokens != null && item.Value == Tokens)
                        continue;

                    item.Value.Record.Save();
                }

                while (ItemsToDelete.Count > 0)
                {
                    var item = ItemsToDelete.Dequeue();

                    item.Record.Delete();
                }

                if (Tokens == null && Owner.Account.Tokens > 0 || (Tokens != null && Owner.Account.Tokens != Tokens.Stack))
                {
                    Owner.Account.Tokens = Tokens == null ? 0 : Tokens.Stack;
                    IpcAccessor.Instance.ProxyObject.UpdateAccount(Owner.Account);
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            UnLoadInventory();
        }

        #endregion

        public PlayerItem AddItem(ItemTemplate template, uint amount = 1)
        {
            var item = TryGetItem(template);

            if (!item.IsEquiped())
            {
                StackItem(item, (int) amount);
            }
            else
            {
                item = ItemManager.Instance.CreatePlayerItem(Owner, template, amount);
                return AddItem(item);
            }

            return item;
        }

        public bool CanEquip(PlayerItem item, CharacterInventoryPositionEnum position, bool send = true)
        {
            if (Owner.IsInFight())
                return false;

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

        public CharacterInventoryPositionEnum[] GetItemPossiblePositions(PlayerItem item)
        {
            if (!m_itemsPositioningRules.ContainsKey(item.Template.Type.SuperType))
                return new[] { CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED };

            return m_itemsPositioningRules[item.Template.Type.SuperType];
        }

        public void MoveItem(PlayerItem item, CharacterInventoryPositionEnum position)
        {
            if (!HasItem(item))
                return;

            if (!CanEquip(item, position))
                return;

            if (position == item.Position)
                return;

            CharacterInventoryPositionEnum oldPosition = item.Position;

            PlayerItem equipedItem;
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

            if (position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                UnEquipedDouble(item);

            if (item.Stack > 1) // if the item to move is stack we cut it
            {
                CutItem(item, (uint)( item.Stack - 1 ));
                // now we have 2 stack : itemToMove, stack = 1
                //						 newitem, stack = itemToMove.Stack - 1
            }

            item.Position = position;

            PlayerItem stacktoitem;
            if (position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED &&
                IsStackable(item, out stacktoitem) && stacktoitem != null)
                // check if we must stack the moved item
            {

                NotifyItemMoved(item, oldPosition);
                StackItem(stacktoitem, item.Stack); // in all cases Stack = 1 else there is an error
                RemoveItem(item);
            }
            else // else we just move the item
            {
                NotifyItemMoved(item, oldPosition);
            }
        }

        private bool UnEquipedDouble(PlayerItem itemToEquip)
        {
            if (itemToEquip.Template.Type.ItemType == ItemTypeEnum.DOFUS)
            {
                var dofus = GetEquipedItems().FirstOrDefault(entry => entry.Guid != itemToEquip.Guid && entry.Template.Id == itemToEquip.Template.Id);

                if (dofus != null)
                {
                    MoveItem(dofus, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

                    return true;
                }
            }

            if (itemToEquip.Template.Type.ItemType == ItemTypeEnum.RING)
            {
                // we can equip the same ring if it doesn't own to an item set
                var ring = GetEquipedItems().FirstOrDefault(entry => entry.Guid != itemToEquip.Guid && entry.Template.Id == itemToEquip.Template.Id && entry.Template.ItemSetId > 0);

                if (ring != null)
                {
                    MoveItem(ring, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

                    return true;
                }
            }

            return false;
        }


        public void ChangeItemOwner(Character newOwner, PlayerItem item, uint amount)
        {
            if (!HasItem(item.Guid))
                return;

            if (amount > item.Stack)
                amount = (uint)item.Stack;

            // delete the item if there is no more stack else we unstack it
            if (amount >= item.Stack)
            {
                RemoveItem(item, false);
                item.ChangeOwner(newOwner);
                newOwner.Inventory.AddItem(item);
            }
            else
            {
                UnStackItem(item, (int) amount);

                var copy = ItemManager.Instance.CreatePlayerItem(newOwner, item, amount);
                newOwner.Inventory.AddItem(copy);
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

        public bool CanUseItem(PlayerItem item, bool send = true)
        {
            if (!HasItem(item.Guid) || !item.IsUsable())
                return false;

            if (Owner.IsInFight())
                return false;

            if (!item.AreConditionFilled(Owner))
            {
                if (send)
                    BasicHandler.SendTextInformationMessage(Owner.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 19);
                return false;
            }

            if (item.Template.Level > Owner.Level)
            {
                if (send)
                    BasicHandler.SendTextInformationMessage(Owner.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 3);
                return false;
            }

            return true;
        }

        public void UseItem(PlayerItem item)
        {
            if (!CanUseItem(item))
                return;

            bool remove = false;
            foreach (var effect in item.Effects)
            {
                var handler = EffectManager.Instance.GetUsableEffectHandler(effect, Owner, item);

                if (handler.Apply())
                    remove = true;
            }

            if (remove)
                RemoveItem(item, 1);
        }

        /// <summary>
        /// Cut an item into two parts
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public PlayerItem CutItem(PlayerItem item, uint amount)
        {
            if (amount >= item.Stack)
                return item;

            UnStackItem(item, (int)amount);

            var newitem = ItemManager.Instance.CreatePlayerItem(Owner, item, amount);

            Items.Add(newitem.Guid, newitem);

            NotifyItemAdded(newitem);

            return newitem;
        }

        private void ApplyItemEffects(PlayerItem item, bool send = true)
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

        protected override void DeleteItem(PlayerItem item)
        {
            if (item == Tokens)
                return;

            base.DeleteItem(item);
        }

        protected override void OnItemAdded(PlayerItem item)
        {
            m_itemsByPosition[item.Position].Add(item);

            if (item.IsEquiped())
                ApplyItemEffects(item);

            InventoryHandler.SendObjectAddedMessage(Owner.Client, item);
            InventoryHandler.SendInventoryWeightMessage(Owner.Client);

            base.OnItemAdded(item);
        }

        protected override void OnItemRemoved(PlayerItem item)
        {
            m_itemsByPosition[item.Position].Remove(item);

            if (item == Tokens)
                Tokens = null;

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

        private void OnItemMoved(PlayerItem  item, CharacterInventoryPositionEnum lastPosition)
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

        protected override void OnItemStackChanged(PlayerItem item, int difference)
        {
            InventoryHandler.SendObjectQuantityMessage(Owner.Client, item);

            base.OnItemStackChanged(item, difference);
        }

        protected override void OnKamasAmountChanged(int amount)
        {
            InventoryHandler.SendKamasUpdateMessage(Owner.Client, amount);

            base.OnKamasAmountChanged(amount);
        }

        public override bool IsStackable(PlayerItem item, out PlayerItem stackableWith)
        {
            PlayerItem stack;
            if (( stack = TryGetItem(item.Template, item.Effects, item.Position, item) ) != null)
            {
                stackableWith = stack;
                return true;
            }

            stackableWith = null;
            return false;
        }

        public PlayerItem TryGetItem(CharacterInventoryPositionEnum position)
        {
            return Items.Values.Where(entry => entry.Position == position).FirstOrDefault();
        }

        public PlayerItem TryGetItem(ItemTemplate template, IEnumerable<EffectBase> effects, CharacterInventoryPositionEnum position, PlayerItem except)
        {
            IEnumerable<PlayerItem> entries = from entry in Items.Values
                                              where entry != except && entry.Template.Id == template.Id && entry.Position == position && effects.CompareEnumerable(entry.Effects)
                                              select entry;

            return entries.FirstOrDefault();
        }

        public IEnumerable<PlayerItem> GetItems(CharacterInventoryPositionEnum position)
        {
            return Items.Values.Where(entry => entry.Position == position);
        }

        public IEnumerable<PlayerItem> GetEquipedItems()
        {
            return from entry in Items
                   where entry.Value.IsEquiped()
                   select entry.Value;
        }

        public int CountItemSetEquiped(ItemSetTemplate itemSet)
        {
            return GetEquipedItems().Count(entry => itemSet.Items.Contains(entry.Template));
        }

        public PlayerItem[] GetItemSetEquipped(ItemSetTemplate itemSet)
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
    }
}