
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NLog;
using Stump.Database;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Data;
using Stump.Server.BaseServer.Initializing;
using Stump.Server.WorldServer.Data;
using Stump.Server.WorldServer.Effects;
using Stump.Server.WorldServer.Entities;
using ItemTypeEx = Stump.DofusProtocol.D2oClasses.ItemType;
using ItemEx = Stump.DofusProtocol.D2oClasses.Item;
using WeaponEx = Stump.DofusProtocol.D2oClasses.Weapon;

namespace Stump.Server.WorldServer.Items
{
    public static class ItemManager
    {
        #region Fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        ///   Array containing every Item template loaded from database.
        /// </summary>
        private static Dictionary<int, ItemTemplate> m_itemTemplates = new Dictionary<int, ItemTemplate>();

        private static readonly ConcurrentDictionary<long, Item> m_loadedItems = new ConcurrentDictionary<long, Item>();

        /// <summary>
        ///   Array containing every Item Types in the game
        /// </summary>
        private static ItemTypeEx[] m_itemTypes = new ItemTypeEx[(int) ItemTypeEnum.End];

        #endregion

        #region Creator

        public static Item Create(int id, LivingEntity owner, uint amount)
        {
            var newitem =
                new Item(m_itemTemplates[id],
                         CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, amount,
                         GenerateItemEffect(m_itemTemplates[id]));

            AddItem(newitem);

            if (!newitem.CanBeSave)
                throw new Exception("Item without assigned Guid cannot be used");

            return newitem;
        }

        public static Item Create(int id, LivingEntity owner, uint amount, List<EffectBase> effects)
        {
            var newitem =
                new Item(m_itemTemplates[id],
                         CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, amount, effects);

            AddItem(newitem);

            if (!newitem.CanBeSave)
                throw new Exception("Item without assigned Guid cannot be used");

            return newitem;
        }

        public static Item RegisterAnItemCopy(Item copy, LivingEntity owner, uint amount)
        {
            var newitem =
                new Item(copy.Template, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED,
                         amount, copy.Effects);

            AddItem(newitem);

            if (!newitem.CanBeSave)
                throw new Exception("Item without assigned Guid cannot be used");

            return newitem;
        }

        public static List<EffectBase> GenerateItemEffect(ItemTemplate template)
        {
            return template.Effects.Select(effect => effect.GenerateEffect(EffectGenerationContext.Item)).ToList();
        }

        #endregion

        #region Loading

        [StageStep(Stages.Two, "Loaded Items")]
        public static void LoadItemsInfo()
        {
            m_itemTypes = DataLoader.LoadDataById<ItemTypeEx>(entry => entry.id);
            m_itemTemplates =
                DataLoader.LoadDataByIdAsDictionary<int, ItemEx>(entry => entry.id).Select(
                    entry =>
                    entry.Value is WeaponEx ? new WeaponTemplate((WeaponEx) entry.Value) : new ItemTemplate(entry.Value))
                    .ToDictionary(entry => entry.Id);

            ItemLoader.LoadItemNames(ref m_itemTemplates);
            ItemLoader.LoadItemsStored(ref m_itemTemplates);
        }

        #endregion

        #region Getters

        public static ItemTemplate GetTemplate(int id)
        {
            return !m_itemTemplates.ContainsKey(id) ? null : m_itemTemplates[id];
        }

        public static ItemTemplate GetTemplate(string name, bool ignorecase)
        {
            return
                m_itemTemplates.Values.Where(
                    entry =>
                    entry.Name.Equals(name,
                                      ignorecase
                                          ? StringComparison.InvariantCultureIgnoreCase
                                          : StringComparison.InvariantCulture)).FirstOrDefault();
        }

        public static ItemTypeEx GetItemType(uint id)
        {
            return m_itemTypes.Count(entry => entry != null && entry.id == id) <= 0 ? null : m_itemTypes[id];
        }

        public static Item GetItem(long guid)
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

        /// <summary>
        /// Found an item template contains in a given list with a pattern
        /// </summary>
        /// <remarks>
        /// When @ precede the pattern the case is ignored
        /// * is a joker, it can be placed at the begin or at the end or both
        /// it means that characters are ignored
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
        // todo : enhanced the method to do like this '{Level>100;Name~coiffe...}'
        public static IEnumerable<ItemTemplate> GetItemsByPattern(string pattern, IEnumerable<ItemTemplate> list)
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

        public static IEnumerable<ItemTemplate> GetItemsByPattern(string pattern)
        {
            return GetItemsByPattern(pattern, m_itemTemplates.Values);
        }

        public static IEnumerable<Item> GetItemsByPattern(string pattern, IEnumerable<Item> list)
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
        public static void AddItem(Item item)
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

        public static void RemoveItem(long guid)
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
        /// <param name = "owner"></param>
        /// <param name = "record"></param>
        /// <returns></returns>
        public static Item LoadItem(LivingEntity owner, ItemRecord record)
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

        public static Item[] LoadItem(LivingEntity owner, ItemRecord[] records)
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
        public static void UnLoadItem(long guid)
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
        public static void UnLoadItem(long[] guids)
        {
            foreach (long key in guids)
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