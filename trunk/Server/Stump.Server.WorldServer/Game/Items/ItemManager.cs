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
using Stump.Server.WorldServer.Game.Items.Handlers;

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

        private Dictionary<ItemTypeEnum, ItemHandlerConstructor> m_itemTypesHandlers =
            new Dictionary<ItemTypeEnum, ItemHandlerConstructor>();

        private delegate BaseItemHandler ItemHandlerConstructor(PlayerItem item);


        #endregion

        #region Creators

        public PlayerItem CreatePlayerItem(Character owner, int id, uint amount, bool maxEffects = false)
        {
            if (!m_itemTemplates.ContainsKey(id))
                throw new Exception(string.Format("Template id '{0}' doesn't exist", id));

            return CreatePlayerItem(owner, m_itemTemplates[id], amount, maxEffects);
        }

        public PlayerItem CreatePlayerItem(Character owner, ItemTemplate template, uint amount, bool maxEffects = false)
        {
            var guid = PlayerItemRecord.PopNextId();

            var newitem =
                new PlayerItem(owner, guid, template,
                         CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, amount,
                         GenerateItemEffects(template, maxEffects));

            return newitem;
        }

        public PlayerItem CreatePlayerItem(Character owner, ItemTemplate template, uint amount, List<EffectBase> effects)
        {
            var guid = PlayerItemRecord.PopNextId();

            var newitem =
                new PlayerItem(owner, guid, template,
                         CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, amount, effects);

            return newitem;
        }

        public PlayerItem CreatePlayerItem(Character owner, IItem item)
        {
            return CreatePlayerItem(owner, item.Template, (uint)item.Stack, item.Effects.Clone());
        }

        public PlayerItem CreatePlayerItem(Character owner, IItem item, uint amount)
        {
            return CreatePlayerItem(owner, item.Template, amount, item.Effects.Clone());
        }

        public MerchantItem CreateMerchantItem(PlayerItem item, uint quantity, uint price)
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

            InitializeHandlers();
        }

        private void InitializeHandlers()
        {
            foreach (var type in
                    Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsSubclassOf(typeof (BaseItemHandler))))
            {
                var attr = type.GetCustomAttribute<ItemTypeAttribute>();

                if (attr == null)
                    continue;

                if (m_itemTypesHandlers.ContainsKey(attr.ItemType))
                {
                    logger.Error("ItemHandler with Type {0} defined twice or more !", attr.ItemType);
                    continue;
                }

                m_itemTypesHandlers.Add(attr.ItemType, type.GetConstructor(new [] {typeof(PlayerItem) }).CreateDelegate<ItemHandlerConstructor>());
            }
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
        public IEnumerable<PlayerItem> GetItemsByPattern(string pattern, IEnumerable<PlayerItem> list)
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

        public BaseItemHandler CreateItemHandler(PlayerItem item)
        {
            ItemHandlerConstructor handler;
            if (!m_itemTypesHandlers.TryGetValue((ItemTypeEnum) item.Template.Type.Id, out handler))
                return new BaseItemHandler(item);

            return handler(item);
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