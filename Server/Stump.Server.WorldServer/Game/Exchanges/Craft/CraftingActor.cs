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
    
        // move item to panel but the owner is not the trader
        public bool MoveItemFromBag(BasePlayerItem item, Trader owner, int quantity)
        {
            if (quantity > 0)
            {
                if (quantity > item.Stack)
                    return false;

                var existingItem = Items.FirstOrDefault(x => x.Guid == item.Guid);
                if (existingItem != null)
                {
                    if (existingItem.Stack + quantity > item.Stack)
                        quantity = (int)(item.Stack - existingItem.Stack);

                    existingItem.Stack += (uint)quantity;

                    OnItemMoved(existingItem, true, quantity);

                    return true;
                }


                var newItem = new PlayerTradeItem(owner, item, (uint)quantity);
                AddItem(newItem);
                OnItemMoved(newItem, false, quantity);

                return true;
            }

            var panelItem = Items.FirstOrDefault(x => x.Guid == item.Guid);

            if (item == null)
                return false;

            if (-quantity >= item.Stack || quantity == 0)
            {
                if (RemoveItem(panelItem))
                    OnItemMoved(panelItem, true, quantity);
            }
            else
            {
                item.Stack += (uint)quantity;
                OnItemMoved(panelItem, true, quantity);
            }
            return false;
        }

        public virtual bool CanMoveItem(BasePlayerItem item)
        {
            return true;
        }
    }
}