using System;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using Castle.ActiveRecord;
using Stump.Core.Cache;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Database
{
    public class NpcItemConfiguration : EntityTypeConfiguration<NpcItem>
    {
        public NpcItemConfiguration()
        {
            Map(x => x.Requires("Discriminator").HasValue("Npc"));
            Property(x => x.NpcShopId).HasColumnName("Npc_NpcShopId");
            Property(x => x.CustomPrice).HasColumnName("Npc_CustomPrice");
            Property(x => x.BuyCriterion).HasColumnName("Npc_BuyCriterion");
            Property(x => x.MaxStats).HasColumnName("Npc_MaxStats");
        }
    }

    public class NpcItem : ItemToSell
    {
        public NpcItem()
        {
            m_objectItemToSellInNpcShop = new ObjectValidator<ObjectItemToSellInNpcShop>(BuildObjectItemToSellInNpcShop);
        }

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

        public string BuyCriterion
        {
            get { return m_buyCriterion; }
            set
            {
                m_buyCriterion = value ?? string.Empty;
                m_objectItemToSellInNpcShop.Invalidate();
            }
        }

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