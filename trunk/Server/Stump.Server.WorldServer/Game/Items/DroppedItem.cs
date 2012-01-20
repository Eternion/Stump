namespace Stump.Server.WorldServer.Game.Items
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
    }
}