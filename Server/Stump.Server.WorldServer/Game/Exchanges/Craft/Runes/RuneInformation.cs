using Stump.Server.WorldServer.Database.Items.Templates;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft.Runes
{
    public class RuneInformation
    {
        public RuneInformation(ItemTemplate item, int amount)
        {
            Item = item;
            Amount = amount;
        }

        public ItemTemplate Item
        {
            get;
        }

        public int Amount
        {
            get;
        }

        public int Level => (int) Item.Level;
    }
}