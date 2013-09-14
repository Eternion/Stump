using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges
{
    public class PlayerTradeRequest : RequestBox
    {
        public PlayerTradeRequest(Character source, Character target)
            : base(source, target)
        {
            Source = source;
            Target = target;
        }

        protected override void OnOpen()
        {
            InventoryHandler.SendExchangeRequestedTradeMessage(Source.Client, ExchangeTypeEnum.PLAYER_TRADE,
                                                               Source, Target);
            InventoryHandler.SendExchangeRequestedTradeMessage(Target.Client, ExchangeTypeEnum.PLAYER_TRADE,
                                                               Source, Target);
        }

        protected override void OnAccept()
        {
            var trade = TradeManager.Instance.Create();

            var firstTrader = new PlayerTrader(Source, trade);
            Source.SetDialoger(firstTrader);

            var secondTrader = new PlayerTrader(Target, trade);
            Target.SetDialoger(secondTrader);

            trade.Open(firstTrader, secondTrader);
        }

        protected override void OnDeny()
        {
            InventoryHandler.SendExchangeLeaveMessage(Source.Client, DialogTypeEnum.DIALOG_EXCHANGE, false);
            InventoryHandler.SendExchangeLeaveMessage(Target.Client, DialogTypeEnum.DIALOG_EXCHANGE, false);
        }

        protected override void OnCancel()
        {
            Deny();
        }
    }
}