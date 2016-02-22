using System.Linq;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft
{
    public class Crafter : CraftingActor
    {
        public Crafter(CraftDialog exchange, Character character)
            :base(exchange, character)
        {
        }

        protected override void OnItemMoved(TradeItem item, bool modified, int difference)
        {
            base.OnItemMoved(item, modified, difference);
        }

        public override int Id => Character.Id;
        public override bool SetKamas(int amount)
        {
            return false;
        }
    }
}