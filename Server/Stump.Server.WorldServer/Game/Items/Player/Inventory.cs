using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.Core.Extensions;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Accounts;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Items;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Items.Player
{
    /// <summary>
    ///   Represents the Inventory of a character
    /// </summary>
    public sealed class Inventory : ItemsStorage<BasePlayerItem>, IDisposable
    {
        [Variable(true)]
        private const int MaxInventoryKamas = 150000000;

        [Variable(true)]
        private const int MaxPresets = 8;

        [Variable]
        public static readonly bool ActiveTokens = true;

        [Variable]
        public static readonly int TokenTemplateId = (int)ItemIdEnum.GameMasterToken;
        public static ItemTemplate TokenTemplate;

        [Initialization(typeof(ItemManager), Silent=true)]
        private static void InitializeTokenTemplate()
        {
            if (ActiveTokens)
                TokenTemplate = ItemManager.Instance.TryGetTemplate(TokenTemplateId);
        }

        #region Events

        #region Delegates

        public delegate void ItemMovedEventHandler(Inventory sender, BasePlayerItem item, CharacterInventoryPositionEnum lastPosition);

        #endregion

        public event ItemMovedEventHandler ItemMoved;

        public void NotifyItemMoved(BasePlayerItem item, CharacterInventoryPositionEnum lastPosition)
        {
            OnItemMoved(item, lastPosition);

            var handler = ItemMoved;
            if (handler != null) handler(this, item, lastPosition);
        }


        #endregion

        private readonly Dictionary<CharacterInventoryPositionEnum, List<BasePlayerItem>> m_itemsByPosition
            = new Dictionary<CharacterInventoryPositionEnum, List<BasePlayerItem>>
                  {
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_CAPE, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_BELT, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_BOOTS, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_AMULET, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_LEFT, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_1, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_2, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_3, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_4, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_5, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_6, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_MOUNT, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_MUTATION, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_BOOST_FOOD, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_FIRST_BONUS, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_SECOND_BONUS, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_FIRST_MALUS, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_SECOND_MALUS, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_ROLEPLAY_BUFFER, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_FOLLOWER, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, new List<BasePlayerItem>()},
                  };

        private readonly Dictionary<ItemSuperTypeEnum, CharacterInventoryPositionEnum[]> m_itemsPositioningRules
            = new Dictionary<ItemSuperTypeEnum, CharacterInventoryPositionEnum[]>
            {
                {ItemSuperTypeEnum.SUPERTYPE_AMULET, new[] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_AMULET}},
                {ItemSuperTypeEnum.SUPERTYPE_WEAPON, new[] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON}},
                {ItemSuperTypeEnum.SUPERTYPE_WEAPON_8, new[] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON}},
                {ItemSuperTypeEnum.SUPERTYPE_CAPE, new[] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_CAPE}},
                {ItemSuperTypeEnum.SUPERTYPE_HAT, new[] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_HAT}},
                {
                    ItemSuperTypeEnum.SUPERTYPE_RING,
                    new[]
                    {
                        CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_LEFT,
                        CharacterInventoryPositionEnum.INVENTORY_POSITION_RING_RIGHT
                    }
                },
                {ItemSuperTypeEnum.SUPERTYPE_BOOTS, new[] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_BOOTS}},
                {ItemSuperTypeEnum.SUPERTYPE_BELT, new[] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_BELT}},
                {ItemSuperTypeEnum.SUPERTYPE_PET, new[] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS}},
                {
                    ItemSuperTypeEnum.SUPERTYPE_DOFUS,
                    new[]
                    {
                        CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_1,
                        CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_2,
                        CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_3,
                        CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_4,
                        CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_5,
                        CharacterInventoryPositionEnum.INVENTORY_POSITION_DOFUS_6
                    }
                },
                {ItemSuperTypeEnum.SUPERTYPE_SHIELD, new[] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD}},
                {ItemSuperTypeEnum.SUPERTYPE_BOOST, new[] {CharacterInventoryPositionEnum.INVENTORY_POSITION_BOOST_FOOD}},
            };

        public Inventory(Character owner)
        {
            Owner = owner;
            InitializeEvents();
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

        public BasePlayerItem this[int guid]
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
                var weight = Items.Values.Sum(entry => entry.Weight);

                if (Tokens != null)
                {
                    weight -= Tokens.Weight;
                }

                return weight > 0 ? weight : 0;
            }
        }

        public uint WeightTotal
        {
            get { return 1000; } // todo : manage weight properly
        }

        public uint WeaponCriticalHit
        {
            get
            {
                BasePlayerItem weapon;
                if ((weapon = TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON)) != null)
                {
                    return weapon.Template is WeaponTemplate
                               ? (uint) ((WeaponTemplate) weapon.Template).CriticalHitBonus
                               : 0;
                }

                return 0;
            }
        }

        public BasePlayerItem Tokens
        {
            get;
            set;
        }

        public List<PlayerPresetRecord> Presets
        {
            get;
            private set;
        }

        private Queue<PlayerPresetRecord> PresetsToDelete
        {
            get;
            set;
        }

        internal void LoadInventory()
        {
            var records = ItemManager.Instance.FindPlayerItems(Owner.Id);

            Items = records.Select(entry => ItemManager.Instance.LoadPlayerItem(Owner, entry)).ToDictionary(entry => entry.Guid);
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

            if (TokenTemplate == null || !ActiveTokens || Owner.Account.Tokens <= 0)
                return;

            CreateTokenItem(Owner.Account.Tokens);
        }

        internal void LoadPresets()
        {
            PresetsToDelete = new Queue<PlayerPresetRecord>();
            Presets = ItemManager.Instance.FindPlayerPresets(Owner.Id);

            foreach (var preset in Presets)
            {
                foreach (var item in preset.Objects.Where(item => !HasItem(item.objUid)).ToArray())
                {
                    preset.RemoveObject(item);
                }
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
                var database = WorldServer.Instance.DBAccessor.Database;
                foreach (var item in Items.Where(item => Tokens == null || item.Value != Tokens).Where(item => !item.Value.IsTemporarily))
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

                foreach (var preset in Presets)
                {
                    if (preset.IsNew)
                    {
                        database.Insert(preset);
                        preset.IsNew = false;
                    }
                    else if (preset.IsDirty)
                    {
                        database.Update(preset);
                    }
                }

                while (ItemsToDelete.Count > 0)
                {
                    var item = ItemsToDelete.Dequeue();

                    database.Delete(item.Record);
                }

                while (PresetsToDelete.Count > 0)
                {
                    var preset = PresetsToDelete.Dequeue();

                    database.Delete(preset);
                }

                // update tokens amount
                if ((Tokens != null || Owner.Account.Tokens <= 0) && (Tokens == null || Owner.Account.Tokens == Tokens.Stack))
                {
                    Owner.IsAuthSynced = true;
                    return;
                }

                Owner.Account.Tokens = Tokens == null ? 0 : Tokens.Stack;

                IPCAccessor.Instance.SendRequest<CommonOKMessage>(new UpdateAccountMessage(Owner.Account),
                    msg =>
                    {
                        Owner.OnSaved();
                    });
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            UnLoadInventory();
            TeardownEvents();
        }

        #endregion

        public override void SetKamas(int amount)
        {
            if (amount >= MaxInventoryKamas)
            {
                Kamas = MaxInventoryKamas;            
                //344	Vous avez atteint le seuil maximum de kamas dans votre inventaire.
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 344);
            }

            base.SetKamas(amount);
        }

        public BasePlayerItem AddItem(ItemTemplate template, List<EffectBase> effects, int amount = 1, bool addItemMsg = true)
        {
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");

            var item = ItemManager.Instance.CreatePlayerItem(Owner, template, amount, effects);

            var itemStack = TryGetItem(template);

            if (itemStack != null && !itemStack.IsEquiped() && IsStackable(item, out itemStack))
            {
                if (!itemStack.OnAddItem())
                    return null;

                StackItem(itemStack, amount);
            }
            else
            {
                item = ItemManager.Instance.CreatePlayerItem(Owner, template, amount, effects);

                return !item.OnAddItem() ? null : AddItem(item, addItemMsg);
            }

            return item;
        }

        public BasePlayerItem AddItem(ItemTemplate template, int amount = 1, bool addItemMsg = true)
        {
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");

            var item = TryGetItem(template);

            if (item != null && !item.IsEquiped())
            {            
                if (!item.OnAddItem())
                    return null;

                StackItem(item, amount);
            }
            else
            {
                item = ItemManager.Instance.CreatePlayerItem(Owner, template, amount);

                return !item.OnAddItem() ? null : AddItem(item, addItemMsg);
            }

            return item;
        }

        public override bool RemoveItem(BasePlayerItem item, bool delete = true, bool removeItemMsg = true)
        {
            return item.OnRemoveItem() && base.RemoveItem(item, delete, removeItemMsg);
        }

        public void CreateTokenItem(uint amount)
        {
            Tokens = ItemManager.Instance.CreatePlayerItem(Owner, TokenTemplate, (int)amount);
            Items.Add(Tokens.Guid, Tokens); // cannot stack
        }

        public PlayerPresetRecord GetPreset(int presetId)
        {
            return Presets.FirstOrDefault(x => x.PresetId == presetId);
        }

        public bool IsPresetExist(int presetId)
        {
            return Presets.Any(x => x.PresetId == presetId);
        }

        public void DeleteItemFromPresets(BasePlayerItem item)
        {
            var presets = GetPresetsByItemGuid(item.Guid);

            foreach (var preset in presets)
            {
                preset.RemoveObject(item.Guid);

                InventoryHandler.SendInventoryPresetUpdateMessage(Owner.Client, preset.GetNetworkPreset());
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 255, item.Template.Id, (preset.PresetId + 1));
            }
        }

        public PresetSaveResultEnum AddPreset(int presetId, int symbolId, bool saveEquipement)
        {
            if (presetId < 0 || presetId > 8)
                return PresetSaveResultEnum.PRESET_SAVE_ERR_UNKNOWN;

            if (Presets.Count > MaxPresets)
                return PresetSaveResultEnum.PRESET_SAVE_ERR_TOO_MANY;

            var preset = new PlayerPresetRecord
            {
                OwnerId = Owner.Id,
                PresetId = presetId,
                SymbolId = symbolId,
                Objects = new List<PresetItem>(),
                IsNew = true
            };

            if (IsPresetExist(presetId) && !saveEquipement)
            {
                var oldPreset = GetPreset(presetId);
                preset.Objects = oldPreset.Objects;
            }
            else
            {
                foreach (var item in GetEquipedItems())
                    preset.AddObject(new PresetItem((byte)item.Position, item.Template.Id, item.Guid));       
            }

            RemovePreset(presetId);
            Presets.Add(preset);

            InventoryHandler.SendInventoryPresetUpdateMessage(Owner.Client, preset.GetNetworkPreset());

            return PresetSaveResultEnum.PRESET_SAVE_OK;
        }

        public PresetDeleteResultEnum RemovePreset(int presetId)
        {
            if (presetId < 0 || presetId > 8)
                return PresetDeleteResultEnum.PRESET_DEL_ERR_UNKNOWN;

            var preset = GetPreset(presetId);

            if (preset == null)
                return PresetDeleteResultEnum.PRESET_DEL_ERR_BAD_PRESET_ID;

            Presets.Remove(preset);
            PresetsToDelete.Enqueue(preset);

            var shortcut = Owner.Shortcuts.PresetShortcuts.FirstOrDefault(x => x.Value.PresetId == presetId);
            if (shortcut.Value != null)
                Owner.Shortcuts.RemoveShortcut(ShortcutBarEnum.GENERAL_SHORTCUT_BAR, shortcut.Key);

            return PresetDeleteResultEnum.PRESET_DEL_OK;
        }

        public PresetSaveUpdateErrorEnum RemovePresetItem(int presetId, int position)
        {
            var preset = GetPreset(presetId);

            if (preset == null)
                return PresetSaveUpdateErrorEnum.PRESET_UPDATE_ERR_BAD_PRESET_ID;

            var item = preset.Objects.FirstOrDefault(x => x.position == position);

            if (item == null)
                return PresetSaveUpdateErrorEnum.PRESET_UPDATE_ERR_BAD_POSITION;

            preset.RemoveObject(item);

            InventoryHandler.SendInventoryPresetUpdateMessage(Owner.Client, preset.GetNetworkPreset());

            return PresetSaveUpdateErrorEnum.PRESET_UPDATE_ERR_UNKNOWN;
        }

        public PresetUseResultEnum EquipPreset(int presetId)
        {
            var preset = GetPreset(presetId);

            if (preset == null)
                return PresetUseResultEnum.PRESET_USE_ERR_BAD_PRESET_ID;

            var itemsToMove = new List<Pair<BasePlayerItem, CharacterInventoryPositionEnum>>();

            var partial = false;

            foreach (var item in GetEquipedItems())
            {
                if (item.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_MOUNT ||
                    item.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_BOOST_FOOD ||
                    item.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_FIRST_BONUS ||
                    item.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_FIRST_MALUS ||
                    item.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_FOLLOWER ||
                    item.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_MUTATION ||
                    item.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_ROLEPLAY_BUFFER ||
                    item.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_SECOND_BONUS ||
                    item.Position == CharacterInventoryPositionEnum.INVENTORY_POSITION_SECOND_MALUS)
                    continue;

                MoveItem(item, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
            }

            foreach (var presetItem in preset.Objects.OrderByDescending(x => x.position))
            {
                var item = TryGetItem(presetItem.objUid);

                if (item == null)
                {
                    Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 228, presetItem.objGid, (presetId + 1));
                    partial = true;
                    continue;
                }

                if (!CanEquip(item, (CharacterInventoryPositionEnum)presetItem.position))
                    return PresetUseResultEnum.PRESET_USE_ERR_CRITERION;

                itemsToMove.Add(new Pair<BasePlayerItem, CharacterInventoryPositionEnum>(item, (CharacterInventoryPositionEnum)presetItem.position));
            }

            foreach (var item in itemsToMove)
            {
                MoveItem(item.First, item.Second);
            }

            return partial ? PresetUseResultEnum.PRESET_USE_OK_PARTIAL : PresetUseResultEnum.PRESET_USE_OK;
        }

        public PlayerPresetRecord[] GetPresetsByItemGuid(int itemGuid)
        {
            return Presets.Where(x => x.Objects.Exists(y => y.objUid == itemGuid)).ToArray();
        }

        public BasePlayerItem RefreshItemInstance(BasePlayerItem item)
        {
            if (!Items.ContainsKey(item.Guid))
                return null;

            Items.Remove(item.Guid);

            var newInstance = ItemManager.Instance.RecreateItemInstance(item);
            Items.Add(newInstance.Guid, newInstance);

            RefreshItem(item);

            return newInstance;
        }

        public bool CanEquip(BasePlayerItem item, CharacterInventoryPositionEnum position, bool send = true)
        {
            if (Owner.IsInFight() && Owner.Fight.State != FightState.Placement)
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
            if (!(item.Template is WeaponTemplate) || !item.Template.TwoHanded || shield == null)
                return true;
 
            if (send)
                BasicHandler.SendTextInformationMessage(Owner.Client,
                    TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 79);

            return false;
        }

        public CharacterInventoryPositionEnum[] GetItemPossiblePositions(BasePlayerItem item)
        {
            return !m_itemsPositioningRules.ContainsKey(item.Template.Type.SuperType) ? new[] { CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED } : m_itemsPositioningRules[item.Template.Type.SuperType];
        }

        public void MoveItem(BasePlayerItem item, CharacterInventoryPositionEnum position)
        {
            if (!HasItem(item))
                return;

            if (position == item.Position)
                return;

            var oldPosition = item.Position;

            BasePlayerItem equipedItem;
            if (position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED &&
                // check if an item is already on the desired position
                ((equipedItem = TryGetItem(position)) != null))
            {
                if (equipedItem.AllowFeeding)
                {
                    if (!equipedItem.Feed(item))
                        return;

                    RemoveItem(item);
                    return;
                }

                if (item.AllowDropping)
                {
                    if (!item.Drop(equipedItem))
                        return;

                    RemoveItem(item);
                    return;
                }

                // if there is one we move it to the inventory
                if (CanEquip(item, position, false))
                    MoveItem(equipedItem, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
            }

            if (!CanEquip(item, position))
                return;

            // second check
            if (!HasItem(item))
                return;

            if (position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                UnEquipedDouble(item);

            if (item.Stack > 1) // if the item to move is stack we cut it
            {
                var newItem = CutItem(item);
                // now we have 2 stack : itemToMove, stack = 1
                //						 newitem, stack = itemToMove.Stack - 1

                //Update PresetItem
                var presets = GetPresetsByItemGuid(item.Guid);

                foreach (var preset in presets)
                {
                    var presetItem = preset.GetPresetItem(item.Guid);

                    if (presetItem == null)
                        continue;

                    presetItem.objUid = newItem.Guid;
                    preset.IsDirty = true;
                }

                item = newItem;
            }

            item.Position = position;

            BasePlayerItem stacktoitem;
            if (position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED &&
                IsStackable(item, out stacktoitem) && stacktoitem != null)
                // check if we must stack the moved item
            {
                //Update PresetItem
                var presets = GetPresetsByItemGuid(item.Guid);

                foreach (var preset in presets)
                {
                    var presetItem = preset.GetPresetItem(item.Guid);

                    if (presetItem == null)
                        continue;

                    presetItem.objUid = stacktoitem.Guid;
                    preset.IsDirty = true;
                }

                NotifyItemMoved(item, oldPosition);
                StackItem(stacktoitem, (int)item.Stack, false); // in all cases Stack = 1 else there is an error
                RemoveItem(item, true, false);
            }
            else // else we just move the item
            {
                NotifyItemMoved(item, oldPosition);
            }
        }

        private void UnEquipedDouble(IItem itemToEquip)
        {
            if (itemToEquip.Template.Type.ItemType == ItemTypeEnum.DOFUS || itemToEquip.Template.Type.ItemType == ItemTypeEnum.TROPHY || itemToEquip.Template.Type.ItemType == ItemTypeEnum.DOFUS_SHOP)
            {
                var item = GetEquipedItems().FirstOrDefault(entry => entry.Guid != itemToEquip.Guid && entry.Template.Id == itemToEquip.Template.Id);

                if (item != null)
                {
                    MoveItem(item, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

                    return;
                }
            }

            if (itemToEquip.Template.Type.ItemType != ItemTypeEnum.RING)
                return;

            // we can equip the same ring if it doesn't own to an item set
            var ring = GetEquipedItems().FirstOrDefault(entry => entry.Guid != itemToEquip.Guid && entry.Template.Id == itemToEquip.Template.Id && entry.Template.ItemSetId > 0);

            if (ring == null)
                return;

            MoveItem(ring, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
        }


        public void ChangeItemOwner(Character newOwner, BasePlayerItem item, int amount)
        {            
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");

            if (!HasItem(item.Guid))
                return;

            if (amount > item.Stack)
                amount = (int)item.Stack;

            // delete the item if there is no more stack else we unstack it
            if (amount >= item.Stack)
            {
                RemoveItem(item, true, false);
            }
            else
            {
                UnStackItem(item, amount);
            }

            DeleteItemFromPresets(item);

            var copy = ItemManager.Instance.CreatePlayerItem(newOwner, item, amount);
            newOwner.Inventory.AddItem(copy, false);
        }

        public void CheckItemsCriterias()
        {
            foreach (var equipedItem in GetEquipedItems().ToArray().Where(equipedItem => !equipedItem.AreConditionFilled(Owner)))
            {
                MoveItem(equipedItem, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
            }
        }

        public bool CanUseItem(BasePlayerItem item, bool send = true)
        {
            if (!HasItem(item.Guid) || !item.IsUsable())
                return false;

            if (Owner.IsInFight() && Owner.Fight.State != FightState.Placement)
                return false;

            if (!item.AreConditionFilled(Owner))
            {
                if (send)
                    BasicHandler.SendTextInformationMessage(Owner.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 19);
                return false;
            }

            if (item.Template.Level <= Owner.Level)
                return true;

            if (send)
                BasicHandler.SendTextInformationMessage(Owner.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 3);
            return false;
        }

        public void UseItem(BasePlayerItem item, int amount = 1)
        {
            UseItem(item, amount, null, null);
        }        
        
        public void UseItem(BasePlayerItem item, Cell targetCell, int amount = 1)
        {
            UseItem(item, amount, targetCell, null);
        }        
        
        public void UseItem(BasePlayerItem item, Character target, int amount = 1)
        {
            UseItem(item, amount, null, target);
        }        
        
        public void UseItem(BasePlayerItem item, int amount , Cell targetCell, Character target)
        {
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");

            if (!CanUseItem(item))
                return;

            if (amount > item.Stack)
                amount = (int)item.Stack;

            var removeAmount = (int)item.UseItem(amount, targetCell, target);

            if (removeAmount > 0)
                RemoveItem(item, removeAmount);
        }

        /// <summary>
        /// Cut an item into two parts
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public BasePlayerItem CutItem(BasePlayerItem item)
        {
            if (item.Stack <= 1)
                return item;

            UnStackItem(item, 1, false);

            var newitem = ItemManager.Instance.CreatePlayerItem(Owner, item, 1);

            Items.Add(newitem.Guid, newitem);

            NotifyItemAdded(newitem, false);

            return newitem;
        }

        public void ApplyItemEffects(BasePlayerItem item, bool send = true, bool forceApply = false)
        {
            foreach (var handler in item.Effects.Select(effect => EffectManager.Instance.GetItemEffectHandler(effect, Owner, item)))
            {
                if (forceApply)
                    handler.Operation = ItemEffectHandler.HandlerOperation.APPLY;

                handler.Apply();
            }

            if (send)
                Owner.RefreshStats();
        }

        private void ApplyItemSetEffects(ItemSetTemplate itemSet, int count, bool apply, bool send = true)
        {
            var effects = itemSet.GetEffects(count);

            foreach (var handler in effects.Select(effect => EffectManager.Instance.GetItemEffectHandler(effect, Owner, itemSet, apply)))
            {
                handler.Apply();
            }

            if (send)
                Owner.RefreshStats();
        }

        protected override void DeleteItem(BasePlayerItem item)
        {
            if (item == Tokens)
                return;

            base.DeleteItem(item);
        }

        protected override void OnItemAdded(BasePlayerItem item, bool addItemMsg)
        {
            m_itemsByPosition[item.Position].Add(item);

            if (item.IsEquiped())
                ApplyItemEffects(item);

            InventoryHandler.SendObjectAddedMessage(Owner.Client, item);
            InventoryHandler.SendInventoryWeightMessage(Owner.Client);

            //Vous avez obtenu %1 '$item%2'.
            if (addItemMsg)
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 21, item.Stack, item.Template.Id);

            base.OnItemAdded(item, addItemMsg);
        }

        protected override void OnItemRemoved(BasePlayerItem item, bool removeItemMsg)
        {
            m_itemsByPosition[item.Position].Remove(item);

            if (item == Tokens)
                Tokens = null;

            // not equiped
            var wasEquiped = item.IsEquiped();
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

            //Vous avez perdu %1 '$item%2'.
            if (removeItemMsg)
            {
                DeleteItemFromPresets(item);
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 22, item.Stack, item.Template.Id);
            }

            if (wasEquiped)
                CheckItemsCriterias();

            if (wasEquiped && item.AppearanceId != 0)
                Owner.UpdateLook();

            base.OnItemRemoved(item, removeItemMsg);
        }

        private void OnItemMoved(BasePlayerItem  item, CharacterInventoryPositionEnum lastPosition)
        {
            m_itemsByPosition[lastPosition].Remove(item);
            m_itemsByPosition[item.Position].Add(item);

            var wasEquiped = lastPosition != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED;
            var isEquiped = item.IsEquiped();

            if (wasEquiped && !isEquiped ||
                !wasEquiped && isEquiped)
                ApplyItemEffects(item, false);

            if (!item.OnEquipItem(wasEquiped))
                return;
            
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

            if ((isEquiped || wasEquiped) && item.AppearanceId != 0)
                Owner.UpdateLook();

            Owner.RefreshActor();
            Owner.RefreshStats();
        }

        protected override void OnItemStackChanged(BasePlayerItem item, int difference, bool removeMsg = true)
        {
            InventoryHandler.SendObjectQuantityMessage(Owner.Client, item);
            InventoryHandler.SendInventoryWeightMessage(Owner.Client);

            //Vous avez perdu %1 '$item%2'.
            //Vous avez obtenu %1 '$item%2'.
            if (removeMsg && difference != 0)
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, difference > 0 ? (short)21 : (short)22, Math.Abs(difference), item.Template.Id);

            base.OnItemStackChanged(item, difference);
        }

        protected override void OnKamasAmountChanged(int amount)
        {
            if (amount != 0)
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, amount > 0 ? (short)45 : (short)46, Math.Abs(amount));
            
            InventoryHandler.SendKamasUpdateMessage(Owner.Client, Kamas);

            base.OnKamasAmountChanged(amount);
        }

        public void RefreshItem(BasePlayerItem item)
        {
            InventoryHandler.SendObjectModifiedMessage(Owner.Client, item);
        }

        public override bool IsStackable(BasePlayerItem item, out BasePlayerItem stackableWith)
        {
            BasePlayerItem stack;
            if (( stack = TryGetItem(item.Template, item.Effects, item.Position, item) ) != null)
            {
                stackableWith = stack;
                return true;
            }

            stackableWith = null;
            return false;
        }

        public BasePlayerItem TryGetItem(CharacterInventoryPositionEnum position)
        {
            return Items.Values.FirstOrDefault(entry => entry.Position == position);
        }
        
        public BasePlayerItem TryGetItem(ItemTemplate template, IEnumerable<EffectBase> effects, CharacterInventoryPositionEnum position)
        {
            var entries = from entry in Items.Values
                                              where entry.Template.Id == template.Id && entry.Position == position && effects.CompareEnumerable(entry.Effects)
                                              select entry;

            return entries.FirstOrDefault();
        }

        public BasePlayerItem TryGetItem(ItemTemplate template, IEnumerable<EffectBase> effects, CharacterInventoryPositionEnum position, BasePlayerItem except)
        {
            var entries = from entry in Items.Values
                                              where entry != except && entry.Template.Id == template.Id && entry.Position == position && effects.CompareEnumerable(entry.Effects)
                                              select entry;

            return entries.FirstOrDefault();
        }

        public BasePlayerItem[] GetItems(CharacterInventoryPositionEnum position)
        {
            return Items.Values.Where(entry => entry.Position == position).ToArray();
        }

        public BasePlayerItem[] GetEquipedItems()
        {
            return (from entry in Items
                   where entry.Value.IsEquiped()
                   select entry.Value).ToArray();
        }

        public int CountItemSetEquiped(ItemSetTemplate itemSet)
        {
            return GetEquipedItems().Count(entry => itemSet.Items.Contains(entry.Template));
        }

        public BasePlayerItem[] GetItemSetEquipped(ItemSetTemplate itemSet)
        {
            return GetEquipedItems().Where(entry => itemSet.Items.Contains(entry.Template)).ToArray();
        }

        public EffectBase[] GetItemSetEffects(ItemSetTemplate itemSet)
        {
            return itemSet.GetEffects(CountItemSetEquiped(itemSet));
        }

        public short[] GetItemsSkins()
        {
            return GetEquipedItems().Where(entry => entry.Position != CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS && entry.AppearanceId != 0).Select(entry => (short)entry.AppearanceId).ToArray();
        }

        public Tuple<short?, bool> GetPetSkin()
        {
            var pet = TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS);

            if (pet == null || pet.AppearanceId == 0)
                return null;

            return Tuple.Create((short?)pet.AppearanceId, pet.Template.TypeId == (int)ItemTypeEnum.PET);
        }

        #region Events

        private void InitializeEvents()
        {
            Owner.FightEnded += OnFightEnded;
        }
        private void TeardownEvents()
        {
            Owner.FightEnded -= OnFightEnded;
        }

        private void OnFightEnded(Character character, CharacterFighter fighter)
        {
            // update boosts
            foreach (var boost in GetItems(CharacterInventoryPositionEnum.INVENTORY_POSITION_BOOST_FOOD))
            {
                var effect = boost.Effects.OfType<EffectDice>().FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_RemainingFights);

                if (effect == null)
                    continue;

                var newEffect = new EffectDice(effect);
                newEffect.Value--;

                boost.Effects.Remove(effect);
                boost.Effects.Add(newEffect);

                if (newEffect.Value <= 0)
                    RemoveItem(boost);
                else
                    RefreshItem(boost);
            }
        }

        #endregion
    }
}