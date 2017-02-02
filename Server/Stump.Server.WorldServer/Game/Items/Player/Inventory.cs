using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
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
        const int MaxInventoryKamas = 150000000;

        [Variable(true)]
        const int MaxPresets = 8;

        [Variable]
        public static readonly bool ActiveTokens = true;

        [Variable]
        public static readonly int TokenTemplateId = (int)ItemIdEnum.JETON_DU_MAITRE_DE_JEU_7919;
        public static ItemTemplate TokenTemplate;

        [Variable(true, DefinableRunning = true)]
        public static bool WeightEnabled = true;

        [Initialization(typeof(ItemManager), Silent=true)]
        static void InitializeTokenTemplate()
        {
            if (ActiveTokens)
                TokenTemplate = ItemManager.Instance.TryGetTemplate(TokenTemplateId);
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Events

        #region Delegates

        public delegate void ItemMovedEventHandler(Inventory sender, BasePlayerItem item, CharacterInventoryPositionEnum lastPosition);

        #endregion

        public event ItemMovedEventHandler ItemMoved;

        public void NotifyItemMoved(BasePlayerItem item, CharacterInventoryPositionEnum lastPosition)
        {
            OnItemMoved(item, lastPosition);

            ItemMoved?.Invoke(this, item, lastPosition);
        }


        #endregion

        readonly Dictionary<CharacterInventoryPositionEnum, List<BasePlayerItem>> m_itemsByPosition
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
                      {CharacterInventoryPositionEnum.ACCESSORY_POSITION_RIDE_HARNESS, new List<BasePlayerItem>()},
                      {CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, new List<BasePlayerItem>()},
                  };

        readonly Dictionary<ItemSuperTypeEnum, CharacterInventoryPositionEnum[]> m_itemsPositioningRules
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
                {ItemSuperTypeEnum.SUPERTYPE_MOUNTRELATED, new [] {CharacterInventoryPositionEnum.ACCESSORY_POSITION_RIDE_HARNESS, CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS,  } }, 
            };

        public Inventory(Character owner)
        {
            Owner = owner;
            InitializeEvents();
        }

        public Character Owner
        {
            get;
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

        public BasePlayerItem this[int guid] => TryGetItem(guid);

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

        public uint WeightTotal => 1000 + (uint)(5 * Owner.Stats.Strength.Total) + (uint)Owner.Stats[PlayerFields.Weight].Total;

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

        Queue<PlayerPresetRecord> PresetsToDelete
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

            if (TokenTemplate == null || !ActiveTokens || Owner.WorldAccount.Tokens <= 0)
                return;

            CreateTokenItem(Owner.WorldAccount.Tokens);
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

        void UnLoadInventory()
        {
            // we must keep then in case it's a fight disconnection
            /*Items.Clear();
            foreach (var item in m_itemsByPosition)
            {
                m_itemsByPosition[item.Key].Clear();
            }*/
        }

        public override void Save(ORM.Database database)
        {
            Save(database, true);
        }

        public void Save(ORM.Database database, bool updateAccount)
        {
            lock (Locker)
            {
                foreach (var item in Items.Where(item => Tokens == null || item.Value != Tokens).Where(item => !item.Value.IsTemporarily))
                {
                    if (item.Value.Record.IsNew)
                    {
                        database.Insert(item.Value.Record);
                        item.Value.OnPersistantItemAdded();
                        item.Value.Record.IsNew = false;
                    }
                    else if (item.Value.Record.IsDirty)
                    {
                        database.Update(item.Value.Record);
                        item.Value.OnPersistantItemUpdated();
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
                    item.OnPersistantItemDeleted();
                }

                while (PresetsToDelete.Count > 0)
                {
                    var preset = PresetsToDelete.Dequeue();

                    database.Delete(preset);
                }

                Owner.WorldAccount.Tokens = Tokens == null ? 0 : (int)Tokens.Stack;
                if (updateAccount)
                    database.Update(Owner.WorldAccount);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            UnLoadInventory();
            TeardownEvents();
        }

        #endregion

        public override int SetKamas(int amount)
        {
            if (amount >= MaxInventoryKamas)
            {
                amount = MaxInventoryKamas;            
                //Vous avez atteint le seuil maximum de kamas dans votre inventaire.
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 344);
            }

            return base.SetKamas(amount);
        }

        public BasePlayerItem AddItem(ItemTemplate template, List<EffectBase> effects, int amount = 1)
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

                return !item.OnAddItem() ? null : AddItem(item);
            }

            return item;
        }

        public BasePlayerItem AddItem(ItemTemplate template, int amount = 1)
        {
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");

            var item = TryGetItem(template);

            if (item != null && !item.IsEquiped() && IsStackable(item, out item))
            {
                if (!item.OnAddItem())
                    return null;

                StackItem(item, amount);
            }
            else
            {
                item = ItemManager.Instance.CreatePlayerItem(Owner, template, amount);

                return !item.OnAddItem() ? null : AddItem(item);
            }

            return item;
        }

        public override bool RemoveItem(BasePlayerItem item, bool delete = true, bool sendMessage = true)
        {
            if (item.IsEquiped())
                item = (MoveItem(item, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, true) ?? item);

            return item.OnRemoveItem() && base.RemoveItem(item, delete, sendMessage);
        }

        public override int RemoveItem(BasePlayerItem item, int amount, bool delete = true, bool sendMessage = true)
        {
            if (item.IsEquiped())
                item = (MoveItem(item, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, true) ?? item);

            return base.RemoveItem(item, amount, delete, sendMessage);
        }

        public void CreateTokenItem(int amount)
        {
            Tokens = ItemManager.Instance.CreatePlayerItem(Owner, TokenTemplate, amount);
            Items.Add(Tokens.Guid, Tokens); // cannot stack
        }

        public PlayerPresetRecord GetPreset(int presetId) => Presets.FirstOrDefault(x => x.PresetId == presetId);

        public bool IsPresetExist(int presetId) => Presets.Any(x => x.PresetId == presetId);

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

        public PresetSaveResultEnum AddPreset(int presetId, int symbolId, IEnumerable<int> itemsUids)
        {
            if (presetId < 0 || presetId > 8)
                return PresetSaveResultEnum.PRESET_SAVE_ERR_UNKNOWN;

            if (Presets.Count > MaxPresets)
                return PresetSaveResultEnum.PRESET_SAVE_ERR_TOO_MANY;

            if (!IsPresetExist(presetId))
                return PresetSaveResultEnum.PRESET_SAVE_ERR_UNKNOWN;

            var preset = GetPreset(presetId);
            preset.SymbolId = symbolId;
            preset.Objects = new List<PresetItem>();
            preset.IsDirty = true;

            foreach (var itemUid in itemsUids)
            {
                if (!HasItem(itemUid))
                    continue;

                var item = TryGetItem(itemUid);
                preset.AddObject(new PresetItem((byte)item.Position, (short)item.Template.Id, item.Guid));
            }

            InventoryHandler.SendInventoryPresetUpdateMessage(Owner.Client, preset.GetNetworkPreset());

            return PresetSaveResultEnum.PRESET_SAVE_OK;
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
                    preset.AddObject(new PresetItem((byte)item.Position, (short)item.Template.Id, item.Guid));

                Owner.Shortcuts.AddPresetShortcut(Owner.Shortcuts.GetNextFreeSlot(ShortcutBarEnum.GENERAL_SHORTCUT_BAR), presetId);
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

        public void EquipPreset(int presetId)
        {
            var unlinkedPosition = new List<byte>();

            var preset = GetPreset(presetId);

            if (preset == null)
            {
                InventoryHandler.SendInventoryPresetUseResultMessage(Owner.Client, (sbyte)presetId, PresetUseResultEnum.PRESET_USE_ERR_BAD_PRESET_ID, unlinkedPosition);
                return;
            }
                

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

                if (preset.Objects.Exists(x => x.objUid == item.Guid))
                    continue;

                unlinkedPosition.Add((byte)item.Position);

                MoveItem(item, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
            }

            foreach (var presetItem in preset.Objects.OrderByDescending(x => x.position))
            {
                var item = TryGetItem(presetItem.objUid);

                if (item == null)
                {
                    Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 228, presetItem.objGid, presetId);
                    partial = true;
                    continue;
                }

                if (!CanEquip(item, (CharacterInventoryPositionEnum)presetItem.position))
                {
                    InventoryHandler.SendInventoryPresetUseResultMessage(Owner.Client, (sbyte)presetId, PresetUseResultEnum.PRESET_USE_ERR_CRITERION, unlinkedPosition);
                    return;
                }

                itemsToMove.Add(new Pair<BasePlayerItem, CharacterInventoryPositionEnum>(item, (CharacterInventoryPositionEnum)presetItem.position));
            }

            InventoryHandler.SendInventoryPresetUseResultMessage(Owner.Client, (sbyte)presetId, partial ? PresetUseResultEnum.PRESET_USE_OK_PARTIAL : PresetUseResultEnum.PRESET_USE_OK, unlinkedPosition);

            foreach (var item in itemsToMove)
                MoveItem(item.First, item.Second);
        }

        public PlayerPresetRecord[] GetPresetsByItemGuid(int itemGuid) => Presets.Where(x => x.Objects.Exists(y => y.objUid == itemGuid)).ToArray();

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
            if (!item.CanEquip())
                return false;

            if (Owner.IsInFight() && Owner.Fight.State != FightState.Placement)
                return false;

            if (Owner.IsInExchange())
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

            if (!item.AreConditionFilled(Owner))
            {
                if (send)
                    BasicHandler.SendTextInformationMessage(Owner.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 19);

                return false;
            }

            var weapon = TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON);
            if (item.Template.Type.ItemType == ItemTypeEnum.BOUCLIER && weapon != null && weapon.Template.TwoHanded)
            {
                //Vous avez dû lâcher votre arme à deux mains pour équiper un bouclier.
                if (send)
                    BasicHandler.SendTextInformationMessage(Owner.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 78);

                MoveItem(weapon, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
                return true;
            }

            var shield = TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_SHIELD);
            if (shield != null && (item.Template is WeaponTemplate && item.Template.TwoHanded))
            {
                //Vous avez dû lâcher votre bouclier pour équiper une arme à deux mains.
                if (send)
                    BasicHandler.SendTextInformationMessage(Owner.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 79);

                MoveItem(shield, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
                return true;
            }

            return true;
        }

        public CharacterInventoryPositionEnum[] GetItemPossiblePositions(BasePlayerItem item) => !m_itemsPositioningRules.ContainsKey(item.Template.Type.SuperType) ? new[] { CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED } : m_itemsPositioningRules[item.Template.Type.SuperType];

        public BasePlayerItem MoveItem(BasePlayerItem item, CharacterInventoryPositionEnum position, bool forceCanEquip = false)
        {
            if (!HasItem(item))
                return null;

            if (position == item.Position)
                return null;

            var oldPosition = item.Position;

            BasePlayerItem equipedItem;
            if (position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED &&
                // check if an item is already on the desired position
                ((equipedItem = TryGetItem(position)) != null))
            {
                if (item.CanDrop(equipedItem) && item.Drop(equipedItem))
                {
                    UnStackItem(item, 1);
                    return item;
                }

                if (equipedItem.CanFeed(item) && equipedItem.Feed(item))
                {
                    UnStackItem(item, 1);
                    return item;
                }

                // if there is one we move it to the inventory
                if (CanEquip(item, position, false))
                    MoveItem(equipedItem, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
            }

            if (!CanEquip(item, position) && !forceCanEquip)
                return null;

            // second check
            if (!HasItem(item))
                return null;

            if (position != CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED)
                UnEquipedDouble(item);

            if (item.Stack > 1) // if the item to move is stack we cut it
            {
                var newItem = CutItem(item, 1);
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

            if (position == CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED &&
                IsStackable(item, out var stacktoitem) && stacktoitem != null)
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
                StackItem(stacktoitem, (int)item.Stack); // in all cases Stack = 1 else there is an error
                RemoveItem(item, true);

                item = stacktoitem;
            }
            else // else we just move the item
            {
                NotifyItemMoved(item, oldPosition);
            }

            return item;
        }

        void UnEquipedDouble(IItem itemToEquip)
        {
            if (itemToEquip.Template.Type.ItemType == ItemTypeEnum.DOFUS || itemToEquip.Template.Type.ItemType == ItemTypeEnum.TROPHÉE)
            {
                var item = GetEquipedItems().FirstOrDefault(entry => entry.Guid != itemToEquip.Guid && entry.Template.Id == itemToEquip.Template.Id);

                if (item != null)
                {
                    MoveItem(item, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);

                    return;
                }
            }

            if (itemToEquip.Template.Type.ItemType != ItemTypeEnum.ANNEAU)
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
                RemoveItem(item, true);
            }
            else
            {
                UnStackItem(item, amount);
            }

            DeleteItemFromPresets(item);

            var copy = ItemManager.Instance.CreatePlayerItem(newOwner, item, amount);
            newOwner.Inventory.AddItem(copy);
        }

        public void CheckItemsCriterias()
        {
            foreach (var equipedItem in GetEquipedItems().Where(equipedItem => !equipedItem.AreConditionFilled(Owner)).ToArray())
            {
                MoveItem(equipedItem, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED);
            }
        }

        public bool CanUseItem(BasePlayerItem item, bool send = true)
        {
            if (!HasItem(item.Guid) || !item.IsUsable())
                return false;

            if (Owner.IsInExchange() || (Owner.IsInFight() && Owner.Fight.State != FightState.Placement))
                return false;

            if (Owner.IsGhost())
                return false;

            if (!item.AreConditionFilled(Owner))
            {
                if (send)
                    BasicHandler.SendTextInformationMessage(Owner.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 19);
                return false;
            }

            return true;
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

            if ((target != null && !target.Inventory.CanUseItem(item)) || !CanUseItem(item))
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
        public BasePlayerItem CutItem(BasePlayerItem item, int amount)
        {
            if (item.Stack <= amount)
                return item;

            UnStackItem(item, amount);

            var newitem = ItemManager.Instance.CreatePlayerItem(Owner, item, amount);

            Items.Add(newitem.Guid, newitem);

            NotifyItemAdded(newitem);

            return newitem;
        }

        public void ApplyItemEffects(BasePlayerItem item, bool send = true, ItemEffectHandler.HandlerOperation? force = null, double? efficiency = null)
        {
            bool? applied = null;
            foreach (var handler in item.Effects.Select(effect => EffectManager.Instance.GetItemEffectHandler(effect, Owner, item)))
            {
                if (force != null)
                    handler.Operation = force.Value;
                
                handler.Efficiency = efficiency ?? 1+item.CurrentSubAreaBonus/100d;

                handler.Apply();

                if (!applied.HasValue && handler.Applied)
                    applied = handler.Operation == ItemEffectHandler.HandlerOperation.APPLY;
            }
            if (applied != null)
                item.OnEffectApplied(applied.Value);

            if (send)
                Owner.RefreshStats();
        }
        

        void ApplyItemSetEffects(ItemSetTemplate itemSet, int count, bool apply, bool send = true)
        {
            var effects = itemSet.GetEffects(count);

            foreach (var handler in effects.Select(effect => EffectManager.Instance.GetItemEffectHandler(effect.GenerateEffect(EffectGenerationContext.Item), Owner, itemSet, apply)))
            {
                handler.Apply();
            }

            if (send)
                Owner.RefreshStats();
        }

        protected override void DeleteItem(BasePlayerItem item, bool sendMessage = true)
        {
            if (item == Tokens)
                return;

            base.DeleteItem(item, sendMessage);
        }

        protected override void OnItemAdded(BasePlayerItem item, bool sendMessage = true)
        {
            m_itemsByPosition[item.Position].Add(item);

            if (item.IsEquiped())
                ApplyItemEffects(item);

            if (sendMessage)
            {
                InventoryHandler.SendObjectAddedMessage(Owner.Client, item);
                InventoryHandler.SendInventoryWeightMessage(Owner.Client);
            }

            if (IsFull())
                Owner.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 12);

            base.OnItemAdded(item, sendMessage);
        }

        protected override void OnItemRemoved(BasePlayerItem item, bool sendMessage = true)
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

            if (sendMessage)
            {
                InventoryHandler.SendObjectDeletedMessage(Owner.Client, item.Guid);
                InventoryHandler.SendInventoryWeightMessage(Owner.Client);
            }

            DeleteItemFromPresets(item);

            if (wasEquiped)
                CheckItemsCriterias();

            if (wasEquiped && item.AppearanceId != 0)
                Owner.UpdateLook();

            base.OnItemRemoved(item, sendMessage);
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

            if (isEquiped || wasEquiped)
            {
                CheckItemsCriterias();
            }
            
            Owner.UpdateLook(item);
            Owner.RefreshStats();
        }

        protected override void OnItemStackChanged(BasePlayerItem item, int difference)
        {
            InventoryHandler.SendObjectQuantityMessage(Owner.Client, item);
            InventoryHandler.SendInventoryWeightMessage(Owner.Client);

            base.OnItemStackChanged(item, difference);
        }

        protected override void OnKamasAmountChanged(int amount)
        {
            InventoryHandler.SendKamasUpdateMessage(Owner.Client, Kamas);

            base.OnKamasAmountChanged(amount);
        }

        public void RefreshItem(BasePlayerItem item)
        {
            if (!item.IsDeleted)
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

        public BasePlayerItem TryGetItem(CharacterInventoryPositionEnum position) => Items.Values.FirstOrDefault(entry => entry.Position == position);

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

        public BasePlayerItem[] GetItems() => Items.Values.ToArray();

        public BasePlayerItem[] GetItems(Predicate<BasePlayerItem> predicate) => Items.Values.Where(entry => predicate(entry)).ToArray();

        public BasePlayerItem[] GetEquipedItems()
        {
            return (from entry in Items
                   where entry.Value.IsEquiped()
                   select entry.Value).ToArray();
        }

        public int CountItemSetEquiped(ItemSetTemplate itemSet) => GetEquipedItems().Count(entry => itemSet.Items.Contains(entry.Template));

        public BasePlayerItem[] GetItemSetEquipped(ItemSetTemplate itemSet)
        {
            return GetEquipedItems().Where(entry => itemSet.Items.Contains(entry.Template)).ToArray();
        }

        public EffectBase[] GetItemSetEffects(ItemSetTemplate itemSet)
        {
            return itemSet.GetEffects(CountItemSetEquiped(itemSet));
        }

        public short[] GetItemsSkins() => GetEquipedItems().Where(entry => entry.Position != CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS && entry.AppearanceId != 0)
            .Select(entry => (short)entry.AppearanceId).ToArray();

        public Tuple<short?, bool> GetPetSkin()
        {
            var pet = TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_PETS);

            if (pet == null || pet.AppearanceId == 0)
                return null;

            return Tuple.Create((short?)pet.AppearanceId, pet.Template.TypeId == (int)ItemTypeEnum.FAMILIER);
        }

        public bool IsFull()
        {
            return Weight > WeightTotal && WeightEnabled;
        }

        public bool IsFull(ItemTemplate item, int count)
        {
            return (Weight + (item.RealWeight * count)) > WeightTotal && WeightEnabled;
        }

        #region Events

        private void InitializeEvents()
        {
        }

        private void TeardownEvents()
        {
        }

        #endregion
    }
}
