using System;
using System.Linq;
using Castle.ActiveRecord;
using Stump.Core.Cache;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Npcs;

namespace Stump.Server.WorldServer.Database.Items.Shops
{
    [ActiveRecord(DiscriminatorValue = "Npc")]
    public class NpcItem : ItemToSell
    {
        public NpcItem()
        {
            m_objectItemToSellInNpcShop = new ObjectValidator<ObjectItemToSellInNpcShop>(BuildObjectItemToSellInNpcShop);
        }

        [BelongsTo("NpcId")]
        public NpcTemplate Npc
        {
            get;
            set;
        }

        public int Price
        {
            get
            {
                return CustomPrice.HasValue ? CustomPrice.Value : (int) Item.Price;
            }
        }

        private int? m_customPrice;

        [Property(NotNull = false)]
        public int? CustomPrice
        {
            get { return m_customPrice; }
            set
            {
                m_customPrice = value;
                m_objectItemToSellInNpcShop.Invalidate();
            }
        }

        private string m_buyCriterion;

        [Property]
        public string BuyCriterion
        {
            get { return m_buyCriterion; }
            set
            {
                m_buyCriterion = value;
                m_objectItemToSellInNpcShop.Invalidate();
            }
        }

        #region ObjectItemToSellInNpcShop

        private readonly ObjectValidator<ObjectItemToSellInNpcShop> m_objectItemToSellInNpcShop;

        private ObjectItemToSellInNpcShop BuildObjectItemToSellInNpcShop()
        {
            return new ObjectItemToSellInNpcShop(
                (short)Item.Id,
                0,
                false,
                Item.Effects.Select(entry => entry.GetObjectEffect()),
                CustomPrice.HasValue ? CustomPrice.Value : (int)Item.Price,
                BuyCriterion);
        }

        public override Item GetNetworkItem()
        {
            return m_objectItemToSellInNpcShop;
        }

        #endregion
    }
}