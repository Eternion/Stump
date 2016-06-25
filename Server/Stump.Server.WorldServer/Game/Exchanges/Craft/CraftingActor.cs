using System.Linq;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft
{
    public abstract class CraftingActor : PlayerTrader
    {
        public CraftingActor(BaseCraftDialog trade, Character character)
            : base(character, trade)
        {
            CraftDialog = trade;
        }

        public BaseCraftDialog CraftDialog
        {
            get;
            private set;
        }
    
        

        public virtual bool CanMoveItem(BasePlayerItem item)
        {
            return true;
        }
    }
}