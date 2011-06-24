
using System.Collections.Generic;
using System.Xml.Serialization;
using Stump.DofusProtocol.Classes;

namespace Stump.Server.WorldServer.Items
{
    public class ItemToSellInNpcShop
    {
        [XmlIgnore]
        private ItemTemplate m_template;

        private ItemToSellInNpcShop()
        {
            
        }

        public ItemToSellInNpcShop(ItemTemplate template, int price = 0, string buyCriterion = "")
        {
            Template = template;
            Price = price;
            BuyCriterion = buyCriterion;
        }

        public ItemToSellInNpcShop(ObjectItemToSellInNpcShop item)
        {
            ItemId = (int) item.objectGID;
            Price = (int) item.objectPrice;
            BuyCriterion = item.buyCriterion;
        }

        public int Price
        {
            get;
            set;
        }

        public string BuyCriterion
        {
            get;
            set;
        }

        public int ItemId
        {
            get;
            set;
        }

        [XmlIgnore]
        public ItemTemplate Template
        {
            get { return m_template ?? (m_template = ItemManager.GetTemplate(ItemId)); }
            set
            {
                m_template = value;
                ItemId = value.Id;
            }
        }

        public ObjectItemToSellInNpcShop ToNetworkObjectItem()
        {
            return new ObjectItemToSellInNpcShop(
                (uint) Template.Id,
                0,
                false,
                new List<ObjectEffect>(),
                (uint) Price,
                BuyCriterion);
        }
    }
}