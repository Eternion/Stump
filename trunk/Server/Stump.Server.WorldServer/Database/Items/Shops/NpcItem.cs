using System;
using System.Linq;
using Castle.ActiveRecord;
using Stump.Core.Cache;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Npcs.Actions;

namespace Stump.Server.WorldServer.Database.Items.Shops
{
    [ActiveRecord(DiscriminatorValue = "Npc")]
    public class NpcItem : ItemToSell
    {
        public NpcItem()
        {
            m_objectItemToSellInNpcShop = new ObjectValidator<ObjectItemToSellInNpcShop>(BuildObjectItemToSellInNpcShop);
        }

        [Property("Npc_NpcShopId")]
        public int NpcShopId
        {
            get;
            set;
        }

        public float Price
        {
            get
            {
                return CustomPrice.HasValue ? CustomPrice.Value : (float)Item.Price;
            }
        }

        private float? m_customPrice;

        [Property("Npc_CustomPrice", NotNull = false)]
        public float? CustomPrice
        {
            get { return m_customPrice; }
            set
            {
                m_customPrice = value;
                m_objectItemToSellInNpcShop.Invalidate();
            }
        }

        private string m_buyCriterion;

        [Property("Npc_BuyCriterion")]
        public string BuyCriterion
        {
            get { return m_buyCriterion; }
            set
            {
                m_buyCriterion = value ?? string.Empty;
                m_objectItemToSellInNpcShop.Invalidate();
            }
        }

        [Property("Npc_MaxStats")]
        public bool MaxStats
        {
            get;
            set;
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
                (int) (CustomPrice.HasValue ? CustomPrice.Value : Item.Price),
                BuyCriterion);
        }

        public override Item GetNetworkItem()
        {
            return m_objectItemToSellInNpcShop;
        }

        #endregion
    }
}