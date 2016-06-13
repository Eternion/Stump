using System.Collections.Generic;
using System.Linq;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft
{
    public class RuneTrader : PlayerTrader
    {
        public const int MAX_ITEMS_COUNT = 50;

        public RuneTrader(Character character, RuneTrade trade)
            : base(character, trade)
        {
        }

        public override bool MoveItemToPanel(int guid, uint amount)
        {
            var playerItem = Character.Inventory[guid];
            var tradeItem = Items.SingleOrDefault(entry => entry.Guid == guid);
            
            if (playerItem == null)
                return false;

            if (Items.Count >= MAX_ITEMS_COUNT && tradeItem == null)
                return false;

            return base.MoveItemToPanel(guid, amount);
        }

        public override bool SetKamas(int amount)
        {
            return false;
        }
    }
}