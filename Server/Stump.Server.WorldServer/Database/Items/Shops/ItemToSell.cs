using Stump.DofusProtocol.Types;
using Stump.ORM;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Items;
using Item = Stump.DofusProtocol.Types.Item;

namespace Stump.Server.WorldServer.Database.Items.Shops
{
    public abstract class ItemToSell
    {
        private ItemTemplate m_item;

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

        [Ignore]
        public ItemTemplate Item
        {
            get { return m_item ?? (m_item = ItemManager.Instance.TryGetTemplate(ItemId)); }
            set
            {
                m_item = value;
                ItemId = value.Id;
            }
        }

        public abstract Item GetNetworkItem();
    }
}