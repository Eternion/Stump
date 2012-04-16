using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges
{
    public class PlayerTradeRequest : IRequestBox
    {
        public PlayerTradeRequest(Character source, Character target)
        {
            Source = source;
            Target = target;
        }

        #region IRequestBox Members

        public Character Source
        {
            get;
            private set;
        }

        public Character Target
        {
            get;
            private set;
        }

        public void Open()
        {
            InventoryHandler.SendExchangeRequestedTradeMessage(Source.Client, ExchangeTypeEnum.PLAYER_TRADE,
                                                               Source, Target);
            InventoryHandler.SendExchangeRequestedTradeMessage(Target.Client, ExchangeTypeEnum.PLAYER_TRADE,
                                                               Source, Target);
        }

        public void Accept()
        {
            var trade = TradeManager.Instance.Create();

            var firstTrader = new PlayerTrader(Source, trade);
            Source.SetDialoger(firstTrader);

            var secondTrader = new PlayerTrader(Target, trade);
            Target.SetDialoger(secondTrader);

            trade.FirstTrader = firstTrader;
            trade.SecondTrader = secondTrader;

            trade.Open();

            Close();
        }

        public void Deny()
        {
            InventoryHandler.SendExchangeLeaveMessage(Source.Client, ExchangeTypeEnum.PLAYER_TRADE, false);
            InventoryHandler.SendExchangeLeaveMessage(Target.Client, ExchangeTypeEnum.PLAYER_TRADE, false);

            Close();
        }

        public void Cancel()
        {
            Deny();
        }

        private void Close()
        {
            Source.ResetRequestBox();
            Target.ResetRequestBox();
        }

        #endregion
    }
}