using System.Collections.Generic;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft.Runes
{
    public class DecraftResult
    {
        public DecraftResult(PlayerTradeItem item)
        {
            Item = item;
            Runes = new Dictionary<ItemTemplate, int>();
        }

        public PlayerTradeItem Item
        {
            get;
        } 

        public Dictionary<ItemTemplate, int> Runes
        {
            get;
        }

        public double? MinCoeff
        {
            get;
            set;
        }

        public double? MaxCoeff
        {
            get;
            set;
        }
    }
}