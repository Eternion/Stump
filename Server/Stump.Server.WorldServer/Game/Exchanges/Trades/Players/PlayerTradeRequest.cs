using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges.Trades.Players
{
    public class PlayerTradeRequest : RequestBox
    {
        public PlayerTradeRequest(Character source, Character target)
            : base(source, target)
        {
            Source = source;
            Target = target;
        }
        public override bool IsExchangeRequest => true;

        protected override void OnOpen()
        {
            InventoryHandler.SendExchangeRequestedTradeMessage(Source.Client, ExchangeTypeEnum.PLAYER_TRADE,
                                                               Source, Target);
            InventoryHandler.SendExchangeRequestedTradeMessage(Target.Client, ExchangeTypeEnum.PLAYER_TRADE,
                                                               Source, Target);
        }

        protected override void OnAccept()
        {
            var trade = new PlayerTrade(Source, Target);
            trade.Open();
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