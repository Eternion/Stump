//// /*************************************************************************
////  *
////  *  Copyright (C) 2010 - 2011 Stump Team
////  *
////  *  This program is free software: you can redistribute it and/or modify
////  *  it under the terms of the GNU General Public License as published by
////  *  the Free Software Foundation, either version 3 of the License, or
////  *  (at your option) any later version.
////  *
////  *  This program is distributed in the hope that it will be useful,
////  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
////  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
////  *  GNU General Public License for more details.
////  *
////  *  You should have received a copy of the GNU General Public License
////  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
////  *
////  *************************************************************************/
//using System.Collections.Generic;
//using System.Xml.Serialization;
//using Stump.DofusProtocol.Classes;

//namespace Stump.Server.WorldServer.Items
//{
//    public class ItemToSellInNpcShop
//    {
//        [XmlIgnore]
//        private ItemTemplate m_template;

//        private ItemToSellInNpcShop()
//        {
            
//        }

//        public ItemToSellInNpcShop(ItemTemplate template, int price = 0, string buyCriterion = "")
//        {
//            Template = template;
//            Price = price;
//            BuyCriterion = buyCriterion;
//        }

//        public ItemToSellInNpcShop(ObjectItemToSellInNpcShop item)
//        {
//            ItemId = (int) item.objectGID;
//            Price = (int) item.objectPrice;
//            BuyCriterion = item.buyCriterion;
//        }

//        public int Price
//        {
//            get;
//            set;
//        }

//        public string BuyCriterion
//        {
//            get;
//            set;
//        }

//        public int ItemId
//        {
//            get;
//            set;
//        }

//        [XmlIgnore]
//        public ItemTemplate Template
//        {
//            get { return m_template ?? (m_template = ItemManager.GetTemplate(ItemId)); }
//            set
//            {
//                m_template = value;
//                ItemId = value.Id;
//            }
//        }

//        public ObjectItemToSellInNpcShop ToNetworkObjectItem()
//        {
//            return new ObjectItemToSellInNpcShop(
//                (uint) Template.Id,
//                0,
//                false,
//                new List<ObjectEffect>(),
//                (uint) Price,
//                BuyCriterion);
//        }
//    }
//}