using System.Linq;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft.Runes
{
    public class RuneCrafter : Crafter
    {
        public RuneCrafter(BaseCraftDialog exchange, Character character)
            : base(exchange, character)
        {
        }

        public RuneMagicCraftDialog RuneCraftDialog => Trade as RuneMagicCraftDialog;

        // move item to panel but the owner is not the trader
        public bool MoveItemFromBag(BasePlayerItem item, Trader owner, int quantity)
        {
            if (quantity > 0)
            {
                if (quantity > item.Stack)
                    return false;

                if (RuneCraftDialog.IsItemEditable(item) && quantity > 1)
                    quantity = 1;

                var existingItem = Items.OfType<PlayerTradeItem>().FirstOrDefault(x => x.Guid == item.Guid);
                if (existingItem != null)
                {
                    if (RuneCraftDialog.IsItemEditable(existingItem)) // cannot stack item to edit
                        MoveItemFromBag(existingItem.PlayerItem, owner, (int) -existingItem.Stack);
                    else
                    {
                        if (existingItem.Stack + quantity > item.Stack)
                            quantity = (int) (item.Stack - existingItem.Stack);

                        existingItem.Stack += (uint) quantity;

                        OnItemMoved(existingItem, true, quantity);

                        return true;
                    }
                }

                if (RuneCraftDialog.IsItemEditable(item) && item.Stack > 1)
                {
                    item = item.Owner.Inventory.CutItem(item, 1);
                }

                var newItem = new PlayerTradeItem(owner, item, (uint)quantity);
                AddItem(newItem);
                OnItemMoved(newItem, false, quantity);

                return true;
            }

            var panelItem = Items.FirstOrDefault(x => x.Guid == item.Guid);

            if (item == null)
                return false;

            BasePlayerItem stackableWith;
            if (item.Owner.Inventory.IsStackable(item, out stackableWith))
            {
                if (quantity >= item.Stack || quantity == 0)
                {
                    item.Owner.Inventory.StackItem(stackableWith, (int) item.Stack);
                    item.Owner.Inventory.RemoveItem(item);
                }
                else
                {
                    item.Owner.Inventory.StackItem(stackableWith, quantity);
                    item.Owner.Inventory.RemoveItem(item, quantity);
                }
            }

            if (-quantity >= item.Stack || quantity == 0)
            {
                if (RemoveItem(panelItem))
                    OnItemMoved(panelItem, true, quantity);
            }
            else
            {
                item.Stack += (uint) quantity;
                OnItemMoved(panelItem, true, quantity);
            }

            owner.MoveItem(item.Guid, quantity != 0 ? -quantity : (int)item.Stack);

            return false;
            
        }

        public override bool MoveItemToPanel(BasePlayerItem item, int quantity)
        {
            if (RuneCraftDialog.IsItemEditable(item) && quantity > 1)
                quantity = 1;

            if (RuneCraftDialog.IsItemEditable(item) && item.Stack > 1)
            {
                var newItem = item.Owner.Inventory.CutItem(item, 1);

                return base.MoveItemToPanel(newItem, 1);
            }

            return base.MoveItemToPanel(item, quantity);
        }

        public override bool MoveItemToInventory(TradeItem item, int quantity)
        {
            var playerItem = (item as PlayerTradeItem).PlayerItem;

            BasePlayerItem stackableWith;
            if (playerItem == null || !Character.Inventory.IsStackable(playerItem, out stackableWith))
                return base.MoveItemToInventory(item, quantity);

            if (quantity >= item.Stack || quantity == 0)
            {
                Character.Inventory.StackItem(stackableWith, (int)playerItem.Stack);
                Character.Inventory.RemoveItem(playerItem);
            }
            else
            {
                Character.Inventory.StackItem(stackableWith, quantity);
                Character.Inventory.RemoveItem(playerItem, quantity);
            }

            return base.MoveItemToInventory(item, quantity);
        }
    }
}