namespace Stump.Server.WorldServer.Worlds.Items
{
    public class DroppedItem
    {
        public DroppedItem(short itemId, uint amount)
        {
            ItemId = itemId;
            Amount = amount;
        }

        public short ItemId
        {
            get;
            set;
        }

        public uint Amount
        {
            get;
            set;
        }

        public Item GenerateItem()
        {
            return ItemManager.Instance.Create(ItemId, Amount);
        }
    }
}