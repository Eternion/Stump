using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NLog;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Shops;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Items.Player.Custom;
using Stump.Server.WorldServer.Game.Items.TaxCollector;

namespace Stump.Server.WorldServer.Game.Items
{
    public class ItemManager : DataManager<ItemManager>
    {
        #region Fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private Dictionary<int, ItemTemplate> m_itemTemplates = new Dictionary<int, ItemTemplate>();
        private Dictionary<int, LivingObjectRecord> m_livingObjects = new Dictionary<int, LivingObjectRecord>();
        private Dictionary<uint, ItemSetTemplate> m_itemsSets = new Dictionary<uint, ItemSetTemplate>();
        private Dictionary<int, ItemTypeRecord> m_itemTypes = new Dictionary<int, ItemTypeRecord>();
        private Dictionary<int, NpcItem> m_npcShopItems = new Dictionary<int, NpcItem>();

        private readonly Dictionary<ItemIdEnum, PlayerItemConstructor> m_itemCtorById =
            new Dictionary<ItemIdEnum, PlayerItemConstructor>();

        private readonly Dictionary<ItemTypeEnum, PlayerItemConstructor> m_itemCtorByTypes =
            new Dictionary<ItemTypeEnum, PlayerItemConstructor>();
        
        private readonly Dictionary<EffectsEnum, PlayerItemConstructor> m_itemCtorByEffects =
            new Dictionary<EffectsEnum, PlayerItemConstructor>();

        private delegate BasePlayerItem PlayerItemConstructor(Character owner, PlayerItemRecord record);


        #endregion

        #region Creators

        public BasePlayerItem CreatePlayerItem(Character owner, int id, int amount, bool maxEffects = false)
        {            
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");

            if (!m_itemTemplates.ContainsKey(id))
                throw new Exception(string.Format("Template id '{0}' doesn't exist", id));

            return CreatePlayerItem(owner, m_itemTemplates[id], amount, maxEffects);
        }

        public BasePlayerItem CreatePlayerItem(Character owner, ItemTemplate template, int amount, bool maxEffects = false)
        {            
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");

            return CreatePlayerItem(owner, template, amount, GenerateItemEffects(template, maxEffects));
        }

        public BasePlayerItem CreatePlayerItem(Character owner, IItem item)
        {
            return CreatePlayerItem(owner, item.Template, (int)item.Stack, item.Effects.Clone());
        }

        public BasePlayerItem CreatePlayerItem(Character owner, IItem item, int amount)
        {            
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");

            return CreatePlayerItem(owner, item.Template, amount, item.Effects.Clone());
        }

        public BasePlayerItem CreatePlayerItem(Character owner, ItemTemplate template, int amount, List<EffectBase> effects)
        {            
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");

            var guid = PlayerItemRecord.PopNextId();
            var record = new PlayerItemRecord // create the associated record
                        {
                            Id = guid,
                            OwnerId = owner.Id,
                            Template = template,
                            Stack = (uint)amount,
                            Position = CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED,
                            Effects = effects,
                            IsNew = true,
                        };

            return CreateItemInstance(owner, record);
        }

        public BasePlayerItem RecreateItemInstance(BasePlayerItem item)
        {
            return CreateItemInstance(item.Owner, item.Record);
        }

        public BasePlayerItem LoadPlayerItem(Character owner, PlayerItemRecord record)
        {
            return CreateItemInstance(owner, record);
        }

        private BasePlayerItem CreateItemInstance(Character character, PlayerItemRecord record)
        {
            PlayerItemConstructor ctor = null;
            if (record.Effects.Any(effect => m_itemCtorByEffects.TryGetValue(effect.EffectId, out ctor)))
            {
                return ctor(character, record);
            }

            if (m_itemCtorById.TryGetValue((ItemIdEnum) record.ItemId, out ctor))
            {
                return ctor(character, record);
            }

            return m_itemCtorByTypes.TryGetValue((ItemTypeEnum) record.Template.Type.Id, out ctor) ? ctor(character, record) : new DefaultItem(character, record);
        }

        public MerchantItem CreateMerchantItem(Character character, BasePlayerItem item, int amount, uint price)
        {
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");


            var guid = PlayerMerchantItemRecord.PopNextId();
            var record = new PlayerMerchantItemRecord // create the associated record
            {
                Id = guid,
                OwnerId = character.Id,
                Price = price,
                Template = item.Template,
                Stack = (uint)amount,
                Effects = new List<EffectBase>(item.Effects),
                IsNew = true
            };

            return new MerchantItem(record);
        }

        public TaxCollectorItem CreateTaxCollectorItem(TaxCollectorNpc owner, ItemTemplate template, int amount)
        {
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");
            var guid = TaxCollectorItemRecord.PopNextId();
            var record = new TaxCollectorItemRecord // create the associated record
                        {
                            Id = guid,
                            OwnerId = owner.GlobalId,
                            Template = template,
                            Stack = (uint)amount,
                            Effects = GenerateItemEffects(template),
                            IsNew = true,
                        };

            return new TaxCollectorItem(record);
        }
        
        public TaxCollectorItem CreateTaxCollectorItem(TaxCollectorNpc owner, int id, int amount)
        {
             if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");


            if (!m_itemTemplates.ContainsKey(id))
                throw new Exception(string.Format("Template id '{0}' doesn't exist", id));

            return CreateTaxCollectorItem(owner, m_itemTemplates[id], amount);

        }

        public BankItem CreateBankItem(Character character, int id, int amount, bool maxStats = false)
        {
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");

            if (!m_itemTemplates.ContainsKey(id))
                throw new Exception(string.Format("Template id '{0}' doesn't exist", id));

            return CreateBankItem(character, m_itemTemplates[id], amount, maxStats);
        }

        public BankItem CreateBankItem(Character character, ItemTemplate template, int amount, bool maxStats = false)
        {
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");


            var guid = BankItemRecord.PopNextId();
            var record = new BankItemRecord // create the associated record
            {
                Id = guid,
                OwnerAccountId = character.Account.Id,
                Template = template,
                Stack = (uint)amount,
                Effects = GenerateItemEffects(template, maxStats),
                IsNew = true
            };

            return new BankItem(character, record);
        }

        public BankItem CreateBankItem(Character character, BasePlayerItem item, int amount)
        {
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");


            var guid = BankItemRecord.PopNextId();
            var record = new BankItemRecord // create the associated record
                        {
                            Id = guid,
                            OwnerAccountId = character.Account.Id,
                            Template = item.Template,
                            Stack = (uint)amount,
                            Effects = new List<EffectBase>(item.Effects),
                            IsNew = true
                        };

            return new BankItem(character, record);
        }

        public bool HasToBeGenerated(ItemTemplate template)
        {
            return template.Effects.Any(x => !EffectManager.Instance.IsUnRandomableWeaponEffect(x.EffectId) && (!(x is EffectDice) || (((EffectDice) x).DiceFace != 0 && ((EffectDice) x).DiceNum != 0)));
        }

        public List<EffectBase> GenerateItemEffects(ItemTemplate template, bool max = false)
        {
            var effects = template.Effects.Select(effect => EffectManager.Instance.IsUnRandomableWeaponEffect(effect.EffectId) ? (EffectBase)effect.Clone() : effect.GenerateEffect(EffectGenerationContext.Item, max ? EffectGenerationType.MaxEffects : EffectGenerationType.Normal)).ToList();

            return effects.ToList();
        }

        #endregion

        #region Loading

        [Initialization(InitializationPass.Fourth)]
        public override void Initialize()
        {
            m_itemTypes = Database.Query<ItemTypeRecord>(ItemTypeRecordRelator.FetchQuery).ToDictionary(entry => entry.Id);
            m_itemTemplates = Database.Query<ItemTemplate>(ItemTemplateRelator.FetchQuery).ToDictionary(entry => entry.Id);
            foreach (var weapon in Database.Query<WeaponTemplate>(WeaponTemplateRelator.FetchQuery))
            {
                m_itemTemplates.Add(weapon.Id, weapon);
            }
            m_itemsSets = Database.Query<ItemSetTemplate>(ItemSetTemplateRelator.FetchQuery).ToDictionary(entry => entry.Id);
            m_npcShopItems = Database.Query<NpcItem>(NpcItemRelator.FetchQuery).ToDictionary(entry => entry.Id);
            m_livingObjects = Database.Query<LivingObjectRecord>(LivingObjectRelator.FetchQuery).ToDictionary(entry => entry.Id);

            InitializeItemCtors();
        }

        private void InitializeItemCtors()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(x => typeof (BasePlayerItem).IsAssignableFrom(x)))
            {
                var idAttrs = type.GetCustomAttributes<ItemIdAttribute>();

                if (idAttrs != null)
                {
                    foreach (var idAttr in idAttrs)
                    {
                        if (m_itemCtorById.ContainsKey(idAttr.ItemId))
                        {
                            logger.Error("Item Constructor with ID {0} defined twice or more !", idAttr.ItemId);
                            continue;
                        }

                        m_itemCtorById.Add(idAttr.ItemId,
                            type.GetConstructor(new[] { typeof(Character), typeof(PlayerItemRecord) })
                                .CreateDelegate<PlayerItemConstructor>());
                    }
                }

                var typeAttrs = type.GetCustomAttributes<ItemTypeAttribute>();

                if (typeAttrs != null)
                {
                    foreach (var typeAttr in typeAttrs)
                    {
                        if (m_itemCtorByTypes.ContainsKey(typeAttr.ItemType))
                        {
                            logger.Error("Item Constructor with Type {0} defined twice or more !", typeAttr.ItemType);
                            continue;
                        }

                        m_itemCtorByTypes.Add(typeAttr.ItemType,
                            type.GetConstructor(new[] { typeof(Character), typeof(PlayerItemRecord) })
                                .CreateDelegate<PlayerItemConstructor>());
                    }
                }

                var effectAttr = type.GetCustomAttribute<ItemHasEffectAttribute>();

                if (effectAttr == null)
                    continue;

                if (m_itemCtorByEffects.ContainsKey(effectAttr.Effect))
                {
                    logger.Error("Item Constructor with Effect {0} defined twice or more !", effectAttr.Effect);
                    continue;
                }

                m_itemCtorByEffects.Add(effectAttr.Effect,
                    type.GetConstructor(new[] {typeof (Character), typeof (PlayerItemRecord)})
                        .CreateDelegate<PlayerItemConstructor>());
            }
        }

        public void AddItemTypeConstructor(Type type)
        {
            var attr = type.GetCustomAttribute<ItemTypeAttribute>();

            if (attr == null)
            {                
                logger.Error("Item Constructor {0} has no attribute !", type);
                return;
            }

            if (m_itemCtorByTypes.ContainsKey(attr.ItemType))
            {
                logger.Error("Item Constructor with Type {0} defined twice or more !", attr.ItemType);
                return;
            }

            m_itemCtorByTypes.Add(attr.ItemType, type.GetConstructor(new [] {typeof(Character), typeof(PlayerItemRecord) }).CreateDelegate<PlayerItemConstructor>());

        }

        public void AddItemIdConstructor(Type type)
        {
            var attr = type.GetCustomAttribute<ItemIdAttribute>();

            if (attr == null)
            {
                logger.Error("Item Constructor {0} has no attribute !", type);
                return;
            }

            if (m_itemCtorById.ContainsKey(attr.ItemId))
            {
                logger.Error("Item Constructor with Type {0} defined twice or more !", attr.ItemId);
                return;
            }

            AddItemIdConstructor(type, attr.ItemId);
        }

        public void AddItemIdConstructor(Type type, ItemIdEnum itemId)
        {
            m_itemCtorById.Add(itemId, type.GetConstructor(new[] { typeof(Character), typeof(PlayerItemRecord) }).CreateDelegate<PlayerItemConstructor>());
        }

        #endregion

        #region Getters

        public IEnumerable<ItemTemplate> GetTemplates()
        {
            return m_itemTemplates.Values;
        }

        public ItemTemplate TryGetTemplate(int id)
        {
            return !m_itemTemplates.ContainsKey(id) ? null : m_itemTemplates[id];
        }

        public ItemTemplate TryGetTemplate(ItemIdEnum id)
        {
            return !m_itemTemplates.ContainsKey((int)id) ? null : m_itemTemplates[(int)id];
        }

        public ItemTemplate TryGetTemplate(string name, bool ignorecase)
        {
            return
                m_itemTemplates.Values.FirstOrDefault(entry => entry.Name.Equals(name,
                                                                                 ignorecase
                                                                                     ? StringComparison.InvariantCultureIgnoreCase
                                                                                     : StringComparison.InvariantCulture));
        }

        public ItemSetTemplate TryGetItemSetTemplate(uint id)
        {
            return !m_itemsSets.ContainsKey(id) ? null : m_itemsSets[id];
        }

        public ItemSetTemplate TryGetItemSetTemplate(string name, bool ignorecase)
        {
            return
                m_itemsSets.Values.FirstOrDefault(entry => entry.Name.Equals(name,
                                                                             ignorecase
                                                                                 ? StringComparison.InvariantCultureIgnoreCase
                                                                                 : StringComparison.InvariantCulture));
        }

        public List<NpcItem> GetNpcShopItems(uint id)
        {
            return m_npcShopItems.Values.Where(entry => entry.NpcShopId == id).ToList();
        }

        public ItemTypeRecord TryGetItemType(int id)
        {
            return !m_itemTypes.ContainsKey(id) ? null : m_itemTypes[id];
        }

        public List<PlayerItemRecord> FindPlayerItems(int ownerId)
        {
            return Database.Fetch<PlayerItemRecord>(string.Format(PlayerItemRelator.FetchByOwner, ownerId));
        }

        public List<PlayerPresetRecord> FindPlayerPresets(int ownerId)
        {
            return Database.Fetch<PlayerPresetRecord>(string.Format(PlayerPresetRelator.FetchByOwner, ownerId));
        }

        public List<PlayerMerchantItemRecord> FindPlayerMerchantItems(int ownerId)
        {
            return Database.Fetch<PlayerMerchantItemRecord>(string.Format(PlayerMerchantItemRelator.FetchByOwner, ownerId));
        }

        public List<TaxCollectorItemRecord> FindTaxCollectorItems(int ownerId)
        {
            return Database.Fetch<TaxCollectorItemRecord>(string.Format(TaxCollectorItemRelator.FetchByOwner, ownerId));
        }

        /// <summary>
        /// Find an item template contains in a given list with a pattern
        /// </summary>
        /// <remarks>
        /// When @ precede the pattern, then the case is ignored
        /// * is a joker, it can be placed at the begin or at the end or both
        /// it means that characters are ignored (include letters, numbers, spaces and underscores)
        /// 
        /// Note : We use RegExp for the pattern. '*' are remplaced by '[\w\d_]*'
        /// </remarks>
        /// <example>
        /// pattern :   @Ab*
        /// list :  abc
        ///         Abd
        ///         ace
        /// 
        /// returns : abc and Abd
        /// </example>
        public IEnumerable<ItemTemplate> GetItemsByPattern(string pattern, IEnumerable<ItemTemplate> list)
        {
            if (pattern == "*")
                return list;

            var ignorecase = pattern[0] == '@';

            if (ignorecase)
                pattern = pattern.Remove(0, 1);

            int outvalue;
            if (!ignorecase && int.TryParse(pattern, out outvalue)) // the pattern is an id
            {
                return list.Where(entry => entry.Id == outvalue);
            }

            pattern = pattern.Replace("*", @"[\w\d\s_]*");

            return list.Where(entry => Regex.Match(entry.Name, pattern, ignorecase ? RegexOptions.IgnoreCase : RegexOptions.None).Success);
        }

        /// <summary>
        /// Find an item template by a pattern
        /// </summary>
        /// <remarks>
        /// When @ precede the pattern, then the case is ignored
        /// * is a joker, it can be placed at the begin or at the end or both
        /// it means that characters are ignored (include letters, numbers, spaces and underscores)
        /// 
        /// Note : We use RegExp for the pattern. '*' are remplaced by '[\w\d_]*'
        /// </remarks>
        /// <example>
        /// pattern :   @Ab*
        /// list :  abc
        ///         Abd
        ///         ace
        /// 
        /// returns : abc and Abd
        /// </example>
        public IEnumerable<ItemTemplate> GetItemsByPattern(string pattern)
        {
            return GetItemsByPattern(pattern, m_itemTemplates.Values);
        }

        /// <summary>
        /// Find an item instancce contains in a given list with a pattern
        /// </summary>
        /// <remarks>
        /// When @ precede the pattern, then the case is ignored
        /// * is a joker, it can be placed at the begin or at the end or both
        /// it means that characters are ignored (include letters, numbers, spaces and underscores)
        /// 
        /// Note : We use RegExp for the pattern. '*' are remplaced by '[\w\d_]*'
        /// </remarks>
        /// <example>
        /// pattern :   @Ab*
        /// list :  abc
        ///         Abd
        ///         ace
        /// 
        /// returns : abc and Abd
        /// </example>
        public IEnumerable<BasePlayerItem> GetItemsByPattern(string pattern, IEnumerable<BasePlayerItem> list)
        {
            if (pattern == "*")
                return list;

            var ignorecase = pattern[0] == '@';

            if (ignorecase)
                pattern = pattern.Remove(0, 1);

            int outvalue;
            if (!ignorecase && int.TryParse(pattern, out outvalue)) // the pattern is an id
            {
                return list.Where(entry => entry.Template.Id == outvalue);
            }

            pattern = pattern.Replace("*", @"[\w\d\s_]*");

            return list.Where(entry => Regex.Match(entry.Template.Name, pattern, ignorecase ? RegexOptions.IgnoreCase : RegexOptions.None).Success);
        }


        public void AddItemTemplate(ItemTemplate template)
        {
            m_itemTemplates.Add(template.Id, template);
            Database.Insert(template);
        }

        public LivingObjectRecord TryGetLivingObjectRecord(int id)
        {
            return !m_livingObjects.TryGetValue(id, out var livingObjectRecord) ? null : livingObjectRecord;
        }

        #endregion
    }

    public class EffectsListComparer : IEqualityComparer<List<EffectBase>>
    {
        public bool Equals(List<EffectBase> x, List<EffectBase> y)
        {
            return x.Count == y.Count && x.All(y.Contains);
        }

        public int GetHashCode(List<EffectBase> obj)
        {
            unchecked // Overflow is fine, just wrap
            {
                return obj.Aggregate((int) 2166136261, (current, effectBase) => current*16777619 ^ effectBase.GetHashCode());
            }
        }
    }
}