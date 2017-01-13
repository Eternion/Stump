using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Handlers.Basic;

namespace Stump.Server.WorldServer.Game.Exchanges.Trades.Players
{
    public class PlayerTrader : Trader
    {
        public PlayerTrader(Character character, ITrade trade)
             : base(trade)
        {
            Character = character;
        }

        public Character Character
        {
            get;
            private set;
        }

        public override int Id
        {
            get { return Character.Id; }
        }


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

        public virtual bool MoveItemToInventory(TradeItem item, int quantity)
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

        public virtual bool MoveItemToPanel(BasePlayerItem item, int quantity)
        {
            if (quantity <= 0 || quantity > item.Stack || item.IsEquiped())
                return false;

            if ((item.IsLinkedToAccount() || item.IsLinkedToPlayer()) && Trade is PlayerTrade)
            {
                BasicHandler.SendTextInformationMessage(Character.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 345, item.Template.Id, item.Guid);
                return false;
            }

            var existingItem = Items.FirstOrDefault(x => x.Guid == item.Guid);
            if (existingItem != null)
            {
                if (existingItem.Stack + quantity > item.Stack)
                    quantity = (int)(item.Stack - existingItem.Stack);

                existingItem.Stack += (uint)quantity;

                OnItemMoved(existingItem, true, quantity);

                return true;
            }

            var newItem = new PlayerTradeItem(this, item, (uint)quantity);
            AddItem(newItem);
            OnItemMoved(newItem, false, quantity);

            return true;
        }

        public override bool SetKamas(int amount)
        {
            if (amount < 0)
                return false;

            return amount <= Character.Inventory.Kamas && base.SetKamas(amount);
        }
    }
}