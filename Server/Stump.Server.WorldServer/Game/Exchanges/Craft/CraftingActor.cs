using System.Linq;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft
{
    public abstract class CraftingActor : Trader
    {
        public CraftingActor(CraftDialog trade, Character character)
            : base(trade)
        {
            Character = character;
            CraftDialog = trade;
        }

        public Character Character
        {
            get;
            private set;
        }

        public CraftDialog CraftDialog
        {
            get;
            private set;
        }

        public override int Id => Character.Id;

        public override bool MoveItem(int id, int quantity)
        {
            if (quantity > 0)
            {
                var item = Character.Inventory[id];

                if (item == null)
                    return false;

                return MoveItemToPanel(item, quantity);
            }
            else
            {
                var item = Items.FirstOrDefault(x => x.Guid == id);

                if (item == null)
                    return false;

                return MoveItemToInventory(item, -quantity);
            }
        }

        public bool MoveItemToInventory(TradeItem item, int quantity)
        {
            if (quantity >= item.Stack || quantity == 0)
            {
                if (RemoveItem(item))
                    OnItemMoved(item, true, quantity);
            }
            else
            {
                item.Stack -= (uint)quantity;
                OnItemMoved(item, true, quantity);
            }

            return true;
        }

        public bool MoveItemToPanel(BasePlayerItem item, int quantity)
        {
            if (quantity <= 0 || quantity > item.Stack)
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


            var newItem = new PlayerTradeItem(item, (uint)quantity);
            AddItem(newItem);
            OnItemMoved(newItem, false, quantity);

            return true;
        }
    }
}