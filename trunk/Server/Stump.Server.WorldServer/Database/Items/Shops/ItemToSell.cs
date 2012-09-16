using System.Data.Entity.ModelConfiguration;
using Stump.Server.WorldServer.Game.Items;
using Item = Stump.DofusProtocol.Types.Item;

namespace Stump.Server.WorldServer.Database
{
    public class ItemToSellConfiguration : EntityTypeConfiguration<ItemToSell>
    {
        public ItemToSellConfiguration()
        {
            ToTable("items_tosell");
        }
    }

    public abstract class ItemToSell
    {
        public int Id
        {
            get;
            set;
        }

        public int ItemId
        {
            get;
            set;
        }

        private ItemTemplate m_item;
        public ItemTemplate Item
        {
            get
            {
                return m_item ?? ( m_item = ItemManager.Instance.TryGetTemplate(ItemId) );
            }
            set
            {
                m_item = value;
                ItemId = value.Id;
            }
        }

        public abstract Item GetNetworkItem();
    }
}