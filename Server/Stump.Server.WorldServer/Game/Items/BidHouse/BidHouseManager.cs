using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Items.BidHouse;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Items.BidHouse
{
    public class BidHouseManager : DataManager<BidHouseManager>, ISaveable
    {
        #region Fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private List<BidHouseItem> m_bidHouseItems = new List<BidHouseItem>();

        private readonly object m_lock = new object();

        [Variable]
        public static int UnsoldDelay = 672;

        [Variable]
        public static float TaxPercent = 2;

        [Variable]
        public static IEnumerable<int> Quantities = new[] { 1, 10, 100 };

        #endregion

        #region Creators

        public BidHouseItem CreateBidHouseItem(Character character, BasePlayerItem item, int amount, uint price)
        {
            if (amount < 0)
                throw new ArgumentException("amount < 0", "amount");


            var guid = BidHouseItemRecord.PopNextId();
            var record = new BidHouseItemRecord // create the associated record
            {
                Id = guid,
                OwnerId = character.Account.Id,
                Price = price,
                SellDate = DateTime.Now,
                Template = item.Template,
                Stack = (uint)amount,
                Effects = new List<EffectBase>(item.Effects),
                IsNew = true
            };

            return new BidHouseItem(record);
        }

        #endregion

        #region Loading

        [Initialization(typeof(ItemManager))]
        public override void Initialize()
        {
            m_bidHouseItems = Database.Query<BidHouseItemRecord>(BidHouseItemRelator.FetchQuery).Select(x => new BidHouseItem(x)).ToList();

            World.Instance.RegisterSaveableInstance(this);
        }

        #endregion

        #region Getters

        public List<BidHouseItem> GetBidHouseItems(ItemTypeEnum type, int maxItemLevel)
        {
            return m_bidHouseItems.Where(x => x.Template.TypeId == (int)type && x.Template.Level >= maxItemLevel)
                .GroupBy(x => x.Template.Id).Select(x => x.First()).ToList();
        }

        public List<BidHouseItem> GetBidHouseItems(int ownerId)
        {
            return m_bidHouseItems.Where(x => x.Record.OwnerId == ownerId).ToList();
        }

        public List<BidHouseItem> GetBidsForItem(int itemId)
        {
            return m_bidHouseItems.Where(x => x.Template.Id == itemId).Distinct(new BidHouseItemComparer()).ToList();
        }

        public IEnumerable<int> GetBidsPriceForItem(int itemId)
        {
            var prices = new List<int>();

            foreach (var item in Quantities.Select(quantity => m_bidHouseItems.Where(x => x.Template.Id == itemId && x.Stack == quantity)
                .OrderBy(x => x.Price).FirstOrDefault()))
            {
                if (item != null)
                    prices.Add((int) item.Price);
                else
                    prices.Add(0);
            }

            return prices;
        }

        public BidHouseItem GetBidHouseItem(int guid)
        {
            return m_bidHouseItems.FirstOrDefault(x => x.Guid == guid);
        }

        public BidHouseItem GetBidHouseItem(int itemId, int quantity, int price)
        {
            return m_bidHouseItems.FirstOrDefault(x => x.Template.Id == itemId && x.Stack == quantity && x.Price == price);
        }

        public int GetAveragePriceForItem(int itemId)
        {
            var items = m_bidHouseItems.Where(x => x.Template.Id == itemId).Select(x => (int)(x.Price / x.Stack)).ToArray();

            if (!items.Any())
                return 0;

            return (int)Math.Round(items.Average());
        }

        #endregion

        #region Functions

        public event Action<BidHouseItem> ItemAdded;

        public void AddBidHouseItem(BidHouseItem item)
        {
            lock(m_lock)
            {
                m_bidHouseItems.Add(item);
            }

            var handler = ItemAdded;

            if (handler != null)
                handler(item);
        }

        public event Action<BidHouseItem> ItemRemoved;

        public void RemoveBidHouseItem(BidHouseItem item)
        {
            WorldServer.Instance.IOTaskPool.AddMessage(
                () => Database.Delete(item.Record));

            lock (m_lock)
            {
                m_bidHouseItems.Remove(item);
            }

            var handler = ItemRemoved;

            if (handler != null)
                handler(item);
        }

        #endregion

        public void Save()
        {
            lock (m_lock)
            {
                foreach (var item in m_bidHouseItems.Where(item => item.Record.IsDirty))
                {
                    item.Save(Database);
                }
            }
        }
    }

    public class BidHouseItemComparer : IEqualityComparer<BidHouseItem>
    {
        public bool Equals(BidHouseItem x, BidHouseItem y)
        {
            return x.Effects.CompareEnumerable(y.Effects);
        }

        public int GetHashCode(BidHouseItem obj)
        {
            return 0;
        }
    }
}
