using Stump.DofusProtocol.Types;
using Stump.ORM;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Database
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