using System.Linq;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft
{
    public class Crafter : Trader
    {
        public Crafter(CraftDialog exchange, Character character)
            : base(exchange)
        {
            CraftDialog = exchange;
            Character = character;
        }

        public CraftDialog CraftDialog
        {
            get;
            private set;
        }

        public Character Character
        {
            get;
            private set;
        }

        public bool MoveItem(BasePlayerItem item, int quantity)
        {
            if (quantity < 0 || quantity > item.Stack)
                return false;

            var existingItem = Items.FirstOrDefault(x => x.Guid == item.Guid);
            if (existingItem != null)
            {
                if (quantity == 0)
                {
                    RemoveItem(existingItem);
                    InventoryHandler.SendExchangeObjectRemovedMessage(Character.Client, false, item.Guid);
                }
                else
                {
                    existingItem.Stack = (uint)quantity;
                    InventoryHandler.SendExchangeObjectModifiedMessage(Character.Client, false, existingItem);
                }

                return true;
            }

            var newItem = new PlayerTradeItem(item, (uint)quantity);
            AddItem(newItem);
            InventoryHandler.SendExchangeObjectAddedMessage(Character.Client, false, newItem);
            return false;
        }

        public override bool MoveItem(int id, int quantity)
        {
            var item = Character.Inventory[id];

            if (item == null)
                return false;

            return MoveItem(item, quantity);
        }

        public override int Id => Character.Id;

        public override bool SetKamas(int amount)
        {
            return false;
        }

        public override void ToggleReady(bool status)
        {
            CraftDialog.Craft();
        }
    }
}