using System.Collections.Generic;
using System.Linq;
using Stump.Core.Collections;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Items.BidHouse
{
    public class BidHouseCategory
    {
        public BidHouseCategory(int id, BidHouseItem item)
        {
            Id = id;
            ItemType = (ItemTypeEnum)item.Template.TypeId;
            TemplateId = item.Template.Id;
            Effects = item.Effects;
            ItemLevel = (int)item.Template.Level;
            Items = new ConcurrentList<BidHouseItem>();
        }

        public int Id
        {
            get;
            private set;
        }

        public int TemplateId
        {
            get;
            private set;
        }

        public ItemTypeEnum ItemType
        {
            get;
            private set;
        }

        public int ItemLevel
        {
            get;
            private set;
        }

        public List<EffectBase> Effects
        {
            get;
            private set;
        }

        public ConcurrentList<BidHouseItem> Items
        {
            get;
            private set;
        }

        #region Functions

        public List<BidHouseItem> GetItems()
        {
            var items = new List<BidHouseItem>();

            foreach (var quantity in BidHouseManager.Quantities)
            {
                var item = Items.OrderBy(x => x.Price).FirstOrDefault(x => x.Stack == quantity);
                if (item == null)
                    continue;

                items.Add(item);
            }

            return items;
        }

        public IEnumerable<int> GetPrices()
        {
            var prices = new List<int>();

            foreach (var item in BidHouseManager.Quantities.Select(quantity => Items.ToArray().Where(x => x.Stack == quantity)
                .OrderBy(x => x.Price).FirstOrDefault()))
            {
                if (item != null)
                    prices.Add((int)item.Price);
                else
                    prices.Add(0);
            }

            return prices;
        }

        public BidHouseItem GetItem(int quantity, int price)
        {
            return Items.FirstOrDefault(x => x.Stack == quantity && x.Price == price && !x.Sold);
        }

        public bool IsValidForThisCategory(BidHouseItem item)
        {
            return item.Template.Id == TemplateId && Effects.CompareEnumerable(item.Effects);
        }

        public bool IsEmpty()
        {
            return !Items.Any();
        }

        #endregion

        #region Network

        public BidExchangerObjectInfo GetBidExchangerObjectInfo()
        {
            return new BidExchangerObjectInfo(Id, 0, false, Effects.Select(x => x.GetObjectEffect()), GetPrices());
        }

        #endregion
    }
}
