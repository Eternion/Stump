using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NLog;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Worlds.Effects.Instances;

namespace Stump.Server.WorldServer.Worlds.Items
{
    public class ItemManager : Singleton<ItemManager>
    {
        #region Fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        ///   Array containing every Item template loaded from database.
        /// </summary>
        private Dictionary<int, ItemTemplate> m_itemTemplates = new Dictionary<int, ItemTemplate>();
        private Dictionary<int, ItemTypeRecord> m_itemTypes =  new Dictionary<int, ItemTypeRecord>();

        private readonly ConcurrentDictionary<int, Item> m_loadedItems = new ConcurrentDictionary<int, Item>();

        #endregion

        #region Creator

        public Item Create(int id, uint amount)
        {
            if (!m_itemTemplates.ContainsKey(id))
                throw new Exception(string.Format("Template id '{0}' doesn't exists", id));

            var newitem =
                new Item(m_itemTemplates[id],
                         CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, (int)amount,
                         GenerateItemEffects(m_itemTemplates[id]));

            AddItem(newitem);

            if (!newitem.CanBeSave)
                throw new Exception("Item without assigned Guid cannot be used");

            return newitem;
        }

        public Item Create(ItemTemplate template, uint amount)
        {
            var newitem =
                new Item(template,
                         CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, (int)amount,
                         GenerateItemEffects(template));

            AddItem(newitem);

            if (!newitem.CanBeSave)
                throw new Exception("Item without assigned Guid cannot be used");

            return newitem;
        }

        public Item Create(int id, uint amount, List<EffectBase> effects)
        {
            if (!m_itemTemplates.ContainsKey(id))
                throw new Exception(string.Format("Template id '{0}' doesn't exists", id));


            var newitem =
                new Item(m_itemTemplates[id],
                         CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, (int)amount, effects);

            AddItem(newitem);

            if (!newitem.CanBeSave)
                throw new Exception("Item without assigned Guid cannot be used");

            return newitem;
        }

        public Item RegisterAnItemCopy(Item copy, uint amount)
        {
            var newitem =
                new Item(copy.Template, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED,
                         (int)amount, copy.Effects);

            AddItem(newitem);

            if (!newitem.CanBeSave)
                throw new Exception("Item without assigned Guid cannot be used");

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

        public ItemTypeRecord GetItemType(int id)
        {
            return !m_itemTypes.ContainsKey(id) ? null : m_itemTypes[id];
        }

        public Item GetItem(int guid)
        {
            if (!m_loadedItems.ContainsKey(guid))
                return null;

            Item item;
            if (!m_loadedItems.TryGetValue(guid, out item) || item == null)
            {
                logger.Error(
                    string.Format("Cannot obtains item <guid:{0}> for an unknown reason",
                                  item.Guid));
                return null;
            }

            return item;
        }

        public bool IsLoaded(int guid)
        {
            return guid != -1 && m_loadedItems.ContainsKey(guid);
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

        #region Database Features

        /// <summary>
        ///   Create an item and add it into the database
        /// </summary>
        /// <param name = "item"></param>
        public void AddItem(Item item)
        {
            if (!item.CanBeSave)
            {
                item.Create(); // get the guid

                if (!m_loadedItems.TryAdd(item.Guid, item))
                {
                    // Item cannot be added
                     throw new Exception(
                        string.Format("Item <guid:{0}> cannot be added for an unknown reason " +
                                      ( m_loadedItems.ContainsKey(item.Guid)
                                           ? "but the item has been added"
                                           : "and the item is not present" ),
                                      item.Guid));
                }
            }
            else
            {
                if (m_loadedItems.ContainsKey(item.Guid))
                    throw new Exception(string.Format(
                        "Cannot create a new item because the guid is already used <guid:{0}>",
                        item.Guid));

                if (!m_loadedItems.TryAdd(item.Guid, item))
                {
                    // Item cannot be added
                    throw new Exception(
                        string.Format("Item <guid:{0}> cannot be added for an unknown reason " +
                                      (m_loadedItems.ContainsKey(item.Guid)
                                           ? "but the item has been added"
                                           : "and the item is not present"),
                                      item.Guid));
                }

                item.Create(); // write the item in the DB if no errors
            }
        }

        public void RemoveItem(int guid)
        {
            if (!m_loadedItems.ContainsKey(guid))
            {
                logger.Warn(string.Format("Trying to remove an unexisting item <guid:{0}>", guid));
                return;
            }

            GetItem(guid).Delete();

            Item removedItem;
            if (!m_loadedItems.TryRemove(guid, out removedItem) ||
                removedItem == null)
            {
                // Item cannot be removed
                logger.Error(
                    string.Format("Item <guid:{0}> cannot be removed for an unknown reason " +
                                  (m_loadedItems.ContainsKey(guid)
                                       ? "and the item has not been deleted"
                                       : "but the item is no more present"),
                                  guid));
            }
        }

        /// <summary>
        ///   Load an item and register it into the cache
        /// </summary>
        /// <param name = "record"></param>
        /// <returns></returns>
        public Item LoadItem(ItemRecord record)
        {
            if (m_loadedItems.ContainsKey(record.Guid))
            {
                logger.Warn(string.Format("Trying to load an item <guid:{0}> twice", record.Guid));
                return m_loadedItems[record.Guid];
            }

            var item = new Item(record);
            if (!m_loadedItems.TryAdd(record.Guid, item))
            {
                // Item cannot be added
                throw new Exception(string.Format("Item <guid:{0}> cannot be added for an unknown reason " +
                                                  (m_loadedItems.ContainsKey(item.Guid)
                                                       ? "but the item has been added"
                                                       : "and the item is not present"),
                                                  item.Guid));
            }

            return item;
        }

        public Item[] LoadItems(ItemRecord[] records)
        {
            var result = new Item[records.Length];

            for (int i = 0; i < records.Length; i++)
            {
                if (m_loadedItems.ContainsKey(records[i].Guid))
                {
                    logger.Warn(string.Format("Trying to load an item <guid:{0}> twice", records[i].Guid));
                }

                result[i] = new Item(records[i]);
                if (!m_loadedItems.TryAdd(records[i].Guid, result[i]))
                {
                    // Item cannot be added
                    throw new Exception(string.Format("Item <guid:{0}> cannot be added for an unknown reason " +
                                                      (m_loadedItems.ContainsKey(records[i].Guid)
                                                           ? "but the item has been added"
                                                           : "and the item is not present"),
                                                      records[i].Guid));
                }
            }

            return result;
        }

        /// <summary>
        ///   Unregister an item from the cache
        /// </summary>
        /// <param name = "guid"></param>
        public void UnLoadItem(int guid)
        {
            if (!m_loadedItems.ContainsKey(guid))
            {
                logger.Warn(string.Format("Trying to unload an item <guid:{0}> that is not in the cache", guid));
            }

            Item removedItem;
            if (!m_loadedItems.TryRemove(guid, out removedItem) ||
                removedItem == null)
            {
                // Item cannot be removed
                logger.Error(
                    string.Format("Item <guid:{0}> cannot be removed for an unknown reason " +
                                  (m_loadedItems.ContainsKey(guid)
                                       ? "and the item has not been deleted"
                                       : "but the item is no more present"),
                                  guid));
            }
        }

        /// <summary>
        ///   Unregister items from the cache
        /// </summary>
        public void UnLoadItems(int[] guids)
        {
            foreach (var key in guids)
            {
                if (!m_loadedItems.ContainsKey(key))
                {
                    logger.Warn(string.Format("Trying to unload an item <guid:{0}> that is not in cache", key));
                }

                Item removedItem;
                if (!m_loadedItems.TryRemove(key, out removedItem) ||
                    removedItem == null)
                {
                    // Item cannot be removed
                    logger.Error(
                        string.Format("Item <guid:{0}> cannot be removed for an unknown reason " +
                                      (m_loadedItems.ContainsKey(key)
                                           ? "and item has not been deleted"
                                           : "but item is no more present"),
                                      key));
                }
            }
        }

        #endregion
    }
}