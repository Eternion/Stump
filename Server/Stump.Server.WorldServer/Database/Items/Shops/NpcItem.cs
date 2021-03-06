using System.Linq;
using Stump.Core.Cache;
using Stump.DofusProtocol.Types;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace Stump.Server.WorldServer.Database.Items.Shops
{
    public class NpcItemRelator
    {
        public static string FetchQuery = "SELECT * FROM npcs_items";

        /// <summary>
        /// Use string.Format
        /// </summary>
        public static string FetchByNpcShop = "SELECT * FROM npcs_items WHERE NpcShopId = {0}";
    }

    [TableName("npcs_items")]
    public class NpcItem : ItemToSell, IAutoGeneratedRecord
    {
        private string m_buyCriterion;
        private double? m_customPrice;

        public NpcItem()
        {
            m_objectItemToSellInNpcShop = new ObjectValidator<ObjectItemToSellInNpcShop>(BuildObjectItemToSellInNpcShop);
        }

        public int NpcShopId
        {
            get;
            set;
        }

        public double Price
        {
            get { return CustomPrice.HasValue ? CustomPrice.Value : Item.Price; }
        }

        public double? CustomPrice
        {
            get { return m_customPrice; }
            set
            {
                m_customPrice = value;
                m_objectItemToSellInNpcShop.Invalidate();
            }
        }

        [NullString]
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
                (short) Item.Id,
                Item.Effects.Select(entry => entry.GetObjectEffect()),
                (int) (CustomPrice.HasValue ? CustomPrice.Value : Item.Price),
                BuyCriterion ?? string.Empty);
        }

        public override Item GetNetworkItem()
        {
            return m_objectItemToSellInNpcShop;
        }

        #endregion
    }
}