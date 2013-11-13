using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NLog;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Shops;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Items.Player.Custom;

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

        private Dictionary<ItemTypeEnum, PlayerItemConstructor> m_itemCtorByTypes =
            new Dictionary<ItemTypeEnum, PlayerItemConstructor>();
        
        private Dictionary<EffectsEnum, PlayerItemConstructor> m_itemCtorByEffects =
            new Dictionary<EffectsEnum, PlayerItemConstructor>();

        private delegate BasePlayerItem PlayerItemConstructor(Character owner, PlayerItemRecord record);


        #endregion

        #region Creators

        public BasePlayerItem CreatePlayerItem(Character owner, int id, uint amount, bool maxEffects = false)
        {
            if (!m_itemTemplates.ContainsKey(id))
                throw new Exception(string.Format("Template id '{0}' doesn't exist", id));

            return CreatePlayerItem(owner, m_itemTemplates[id], amount, maxEffects);
        }

        public BasePlayerItem CreatePlayerItem(Character owner, ItemTemplate template, uint amount, bool maxEffects = false)
        {
            return CreatePlayerItem(owner, template, amount, GenerateItemEffects(template, maxEffects));
        }

        public BasePlayerItem CreatePlayerItem(Character owner, IItem item)
        {
            return CreatePlayerItem(owner, item.Template, (uint)item.Stack, item.Effects.Clone());
        }

        public BasePlayerItem CreatePlayerItem(Character owner, IItem item, uint amount)
        {
            return CreatePlayerItem(owner, item.Template, amount, item.Effects.Clone());
        }

        public BasePlayerItem CreatePlayerItem(Character owner, ItemTemplate template, uint amount, List<EffectBase> effects)
        {
            var guid = PlayerItemRecord.PopNextId();
            var record = new PlayerItemRecord // create the associated record
                        {
                            Id = guid,
                            OwnerId = owner.Id,
                            Template = template,
                            Stack = amount,
                            Position = CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED,
                            Effects = effects,
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

            if (m_itemCtorByTypes.TryGetValue((ItemTypeEnum) record.Template.Type.Id, out ctor))
                return ctor(character, record);

            return new DefaultItem(character, record);
        }

        public MerchantItem CreateMerchantItem(BasePlayerItem item, uint quantity, uint price)
        {
            var guid = PlayerItemRecord.PopNextId();

            var newitem =
                new MerchantItem(item.Owner, guid, item.Template, item.Effects, quantity, price);

            return newitem;
        }

        public List<EffectBase> GenerateItemEffects(ItemTemplate template, bool max = false)
        {
            var effects = new List<EffectBase>();

            foreach (var effect in template.Effects)
            {
                if (EffectManager.Instance.IsUnRandomableWeaponEffect(effect.EffectId))
                    effects.Add(effect);
                else
                    effects.Add(effect.GenerateEffect(EffectGenerationContext.Item, max ? EffectGenerationType.MaxEffects : EffectGenerationType.Normal));
            }

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
            m_livingObjects =
                Database.Query<LivingObjectRecord>(LivingObjectRelator.FetchQuery).ToDictionary(entry => entry.Id);

            InitializeItemCtors();
        }

        private void InitializeItemCtors()
        {
            foreach (var type in
                    Assembly.GetExecutingAssembly().GetTypes().Where(x => typeof (BasePlayerItem).IsAssignableFrom(x)))
            {
                var typeAttr = type.GetCustomAttribute<ItemTypeAttribute>();

                if (typeAttr != null)
                {
                    if (m_itemCtorByTypes.ContainsKey(typeAttr.ItemType))
                    {
                        logger.Error("Item Constructor with Type {0} defined twice or more !", typeAttr.ItemType);
                        continue;
                    }

                    m_itemCtorByTypes.Add(typeAttr.ItemType,
                        type.GetConstructor(new[] {typeof (Character), typeof (PlayerItemRecord)})
                            .CreateDelegate<PlayerItemConstructor>());
                }

                var effectAttr = type.GetCustomAttribute<ItemHasEffectAttribute>();

                if (effectAttr != null)
                {
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
        }

        public void AddItemConstructor(Type type)
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

        public List<PlayerMerchantItemRecord> FindPlayerMerchantItems(int ownerId)
        {
            return Database.Fetch<PlayerMerchantItemRecord>(string.Format(PlayerMerchantItemRelator.FetchByOwner, ownerId));
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

            bool ignorecase = pattern[0] == '@';

            if (ignorecase)
                pattern = pattern.Remove(0, 1);

            int outvalue;
            if (!ignorecase &&
                int.TryParse(pattern, out outvalue)) // the pattern is an id
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

            bool ignorecase = pattern[0] == '@';

            if (ignorecase)
                pattern = pattern.Remove(0, 1);

            int outvalue;
            if (!ignorecase &&
                int.TryParse(pattern, out outvalue)) // the pattern is an id
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
            LivingObjectRecord livingObjectRecord;
            if (!m_livingObjects.TryGetValue(id, out livingObjectRecord))
                return null;
            return livingObjectRecord;        
        }

        #endregion
    }
}