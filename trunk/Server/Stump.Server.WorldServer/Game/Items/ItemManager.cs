using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NLog;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items.Shops;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Items
{
    public class ItemManager : Singleton<ItemManager>
    {
        #region Fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private Dictionary<int, ItemTemplate> m_itemTemplates = new Dictionary<int, ItemTemplate>();
        private Dictionary<int, ItemTypeRecord> m_itemTypes = new Dictionary<int, ItemTypeRecord>();
        private Dictionary<int, ItemToSell> m_itemsToSell = new Dictionary<int, ItemToSell>();

        #endregion

        #region Creators

        public Item Create(int id, uint amount)
        {
            if (!m_itemTemplates.ContainsKey(id))
                throw new Exception(string.Format("Template id '{0}' doesn't exists", id));

            var newitem =
                new Item(m_itemTemplates[id],
                         CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, (int) amount,
                         GenerateItemEffects(m_itemTemplates[id]));

            return newitem;
        }

        public Item Create(ItemTemplate template, uint amount)
        {
            var newitem =
                new Item(template,
                         CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, (int) amount,
                         GenerateItemEffects(template));

            return newitem;
        }

        public Item Create(int id, uint amount, List<EffectBase> effects)
        {
            if (!m_itemTemplates.ContainsKey(id))
                throw new Exception(string.Format("Template id '{0}' doesn't exists", id));


            var newitem =
                new Item(m_itemTemplates[id],
                         CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, (int) amount, effects);

            return newitem;
        }

        public Item Create(ItemTemplate template, uint amount, List<EffectBase> effects)
        {
            var newitem =
                new Item(template,
                         CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, (int) amount, effects);

            return newitem;
        }

        public Item RegisterAnItemCopy(Item copy, uint amount)
        {
            var newitem =
                new Item(copy.Template, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED,
                         (int) amount, copy.Effects);

            return newitem;
        }

        public List<EffectBase> GenerateItemEffects(ItemTemplate template)
        {
            return template.Effects.Select(effect => effect.GenerateEffect(EffectGenerationContext.Item)).ToList();
        }

        #endregion

        #region Loading

        [Initialization(InitializationPass.Fourth)]
        public void Initialize()
        {
            m_itemTypes = ItemTypeRecord.FindAll().ToDictionary(entry => entry.Id);
            m_itemTemplates = ItemTemplate.FindAll().ToDictionary(entry => entry.Id);
            m_itemsToSell = ItemToSell.FindAll().ToDictionary(entry => entry.Id);
        }

        #endregion

        #region Getters

        public ItemTemplate GetTemplate(int id)
        {
            return !m_itemTemplates.ContainsKey(id) ? null : m_itemTemplates[id];
        }

        public ItemTemplate GetTemplate(string name, bool ignorecase)
        {
            return
                m_itemTemplates.Values.Where(
                    entry =>
                    entry.Name.Equals(name,
                                      ignorecase
                                          ? StringComparison.InvariantCultureIgnoreCase
                                          : StringComparison.InvariantCulture)).FirstOrDefault();
        }

        public List<NpcItem> GetNpcShopItems(uint id)
        {
            return m_itemsToSell.Values.OfType<NpcItem>().Where(entry => entry.NpcShopId == id).ToList();
        }

        public ItemTypeRecord GetItemType(int id)
        {
            return !m_itemTypes.ContainsKey(id) ? null : m_itemTypes[id];
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
        public IEnumerable<Item> GetItemsByPattern(string pattern, IEnumerable<Item> list)
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

        #endregion
    }
}