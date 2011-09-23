using Castle.ActiveRecord;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items.Templates;

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

        [BelongsTo("ItemId")]
        public ItemTemplate Item
        {
            get;
            set;
        }

        public abstract Item GetNetworkItem();
    }
}