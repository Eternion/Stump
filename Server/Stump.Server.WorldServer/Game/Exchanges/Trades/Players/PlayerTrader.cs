using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
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

        public override bool MoveItem(int guid, int amount)
        {
            if (amount == 0)
                return false;

            return amount > 0 ? MoveItemToPanel(guid, (uint)amount) : MoveItemToInventory(guid, (uint)( -amount ));
        }

        public bool MoveItemToPanel(int guid, uint amount)
        {
            var playerItem = Character.Inventory[guid];
            var tradeItem = Items.SingleOrDefault(entry => entry.Guid == guid);

            ToggleReady(false);

            if (playerItem == null)
                return false;

            if (amount > playerItem.Stack || amount <= 0)
                return false;

            if ((playerItem.IsLinkedToAccount() || playerItem.IsLinkedToPlayer()) && Trade is PlayerTrade)
            {
                BasicHandler.SendTextInformationMessage(Character.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 345, playerItem.Template.Id, playerItem.Guid);
                return false;
            }

            if (tradeItem != null)
            {
                if (playerItem.Stack < tradeItem.Stack + amount)
                    return false;

                var currentStack = tradeItem.Stack;
                tradeItem.Stack += amount;

                if (tradeItem.Stack <= 0)
                    RemoveItem(tradeItem);

                OnItemMoved(tradeItem, true, (int) (tradeItem.Stack - currentStack));

                return true;
            }

            tradeItem = new PlayerTradeItem(playerItem, amount);
            AddItem(tradeItem);

            OnItemMoved(tradeItem, false, (int) amount);

            return true;
        }

        public bool MoveItemToInventory(int guid, uint amount)
        {
            var tradeItem = Items.SingleOrDefault(entry => entry.Guid == guid);

            if (amount == 0)
                return false;

            ToggleReady(false);

            if (tradeItem == null)
                return false;

            if (tradeItem.Stack <= amount)
            {
                RemoveItem(tradeItem);
                tradeItem.Stack = 0;
            }
            else
            {
                tradeItem.Stack -= amount;
            }

            OnItemMoved(tradeItem, tradeItem.Stack != 0, (int)-amount);
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