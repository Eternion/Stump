
using Stump.Server.WorldServer.Dialog;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Handlers;

namespace Stump.Server.WorldServer.Exchange
{
    public class TradeRequest : IDialogRequest
    {
        public TradeRequest(Character source, Character target)
        {
            Source = source;
            Target = target;
        }

        #region IDialogRequest Members

        public Character Source
        {
            get;
            set;
        }

        public Character Target
        {
            get;
            set;
        }

        public void AcceptDialog()
        {
            try
            {
                var trade = new PlayerTrade(Source, Target);

                Source.Dialoger = trade.SourceTrader;
                Target.Dialoger = trade.TargetTrader;

                TradeManager.CreateTrade(trade);

                InventoryHandler.SendExchangeStartedWithPodsMessage(Source.Client, trade);
                InventoryHandler.SendExchangeStartedWithPodsMessage(Target.Client, trade);
            }
            finally
            {
                Source.DialogRequest = null;
                Target.DialogRequest = null;
            }
        }

        public void DeniedDialog()
        {
            try
            {
                InventoryHandler.SendExchangeLeaveMessage(Source.Client, false);
                InventoryHandler.SendExchangeLeaveMessage(Target.Client, false);
            }
            finally
            {
                Source.DialogRequest = null;
                Target.DialogRequest = null;
            }
        }

        #endregion
    }
}