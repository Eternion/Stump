using Castle.ActiveRecord;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Items;
using Item = Stump.DofusProtocol.Types.Item;

namespace Stump.Server.WorldServer.Database.Items.Shops
{
    [ActiveRecord("items_selled", DiscriminatorColumn = "RecognizerType", DiscriminatorType = "String", DiscriminatorValue = "Base")]
    public abstract class ItemToSell : WorldBaseRecord<ItemToSell>
    {
        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get;
            set;
        }

        [Property("ItemId")]
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