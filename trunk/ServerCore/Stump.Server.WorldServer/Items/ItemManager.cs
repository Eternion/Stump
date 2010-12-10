// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Stump.Database;
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
        ///   Used to get the next unused item guid
        /// </summary>
        private static readonly IdGenerator ItemGuidGenerator = new IdGenerator(typeof (CharacterItemRecord), "Guid");

        /// <summary>
        ///   Array containing every Item template loaded from database.
        /// </summary>
        private static Dictionary<int, ItemTemplate> m_itemTemplates = new Dictionary<int, ItemTemplate>();
        private static ConcurrentDictionary<long, Item> m_loadedItems = new ConcurrentDictionary<long, Item>();

        /// <summary>
        ///   Array containing every Item Types in the game
        /// </summary>
        private static ItemTypeEx[] m_itemTypes = new ItemTypeEx[(int) ItemTypeEnum.End];

        

        #endregion

        #region Creator

        public static Item Create(int id, Entity owner, uint amount)
        {
            long itemguid = ItemGuidGenerator.Next();

            var newitem =
                new Item(owner, m_itemTemplates[id], itemguid,
                         CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, amount,
                         GenerateItemEffect(m_itemTemplates[id]));

            AddItem(newitem);

            return newitem;
        }

        public static Item Create(int id, Entity owner, uint amount, List<EffectBase> effects)
        {
            long itemguid = ItemGuidGenerator.Next();

            var newitem =
                new Item(owner, m_itemTemplates[id], itemguid,
                         CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED, amount, effects);
            AddItem(newitem);

            return newitem;
        }

        public static Item RegisterAnItemCopy(Item copy, Entity owner, uint amount)
        {
            long itemguid = ItemGuidGenerator.Next();

            var newitem =
                new Item(owner, copy.Template, itemguid, CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED,
                         amount, copy.Effects);

            AddItem(newitem);

            return newitem;
        }

        public static List<EffectBase> GenerateItemEffect(ItemTemplate template)
        {
            return template.Effects.Select(effect => effect.GenerateEffect()).ToList();
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

        public static ItemTypeEx GetItemType(int id)
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

        #endregion

        #region Database Features

        /// <summary>
        ///   Create an item and add it into the database
        /// </summary>
        /// <param name = "item"></param>
        public static void AddItem(Item item)
        {
            if (m_loadedItems.ContainsKey(item.Guid))
                throw new Exception(string.Format(
                    "Cannot create a new item because the guid is already used <guid:{0}>",
                    item.Guid));

            if (!m_loadedItems.TryAdd(item.Guid, item))
            {
                // Item cannot be added
                logger.Error(
                    string.Format("Item <guid:{0}> cannot be added for an unknown reason " +
                                  (m_loadedItems.ContainsKey(item.Guid)
                                       ? "but the item has been added"
                                       : "and the item is not present"),
                                  item.Guid));
                return;
            }

            item.Create(); // write the item in the DB if no errors
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
        public static Item LoadItem(Entity owner, CharacterItemRecord record)
        {
            if (m_loadedItems.ContainsKey(record.Guid))
            {
                logger.Warn(string.Format("Trying to load an item <guid:{0}> twice", record.Guid));
                return m_loadedItems[record.Guid];
            }

            var item = new Item(owner, record);
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

        public static Item[] LoadItem(Entity owner, CharacterItemRecord[] records)
        {
            var result = new Item[records.Length];

            for (int i = 0; i < records.Length; i++)
            {
                if (m_loadedItems.ContainsKey(records[i].Guid))
                {
                    logger.Warn(string.Format("Trying to load an item <guid:{0}> twice", records[i].Guid));
                }

                result[i] = new Item(owner, records[i]);
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