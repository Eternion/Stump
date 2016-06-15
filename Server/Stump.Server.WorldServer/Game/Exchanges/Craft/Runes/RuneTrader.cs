using System.Linq;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft.Runes
{
    public class RuneTrader : PlayerTrader
    {
        public const int MAX_ITEMS_COUNT = 50;

        public RuneTrader(Character character, RuneTrade trade)
            : base(character, trade)
        {
        }

        public override bool MoveItemToPanel(BasePlayerItem item, int amount)
        {
            var tradeItem = Items.SingleOrDefault(entry => entry.Guid == item.Guid);

            if (Items.Count >= MAX_ITEMS_COUNT && tradeItem == null)
                return false;

            return base.MoveItemToPanel(item, amount);
        }

        public override bool SetKamas(int amount)
        {
            return false;
        }
    }
}