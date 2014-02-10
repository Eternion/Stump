using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Exchanges.Items;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges
{
    public class TaxCollectorTrade : Trade<PlayerTrader, TaxCollectorTrader>
    {
        public TaxCollectorTrade() : base(0)
        {
        }

        public override ExchangeTypeEnum ExchangeType
        {
            get { return ExchangeTypeEnum.TAXCOLLECTOR; }
        }


        public override void Open(PlayerTrader firstTrader, TaxCollectorTrader secondTrader)
        {
            base.Open(firstTrader, secondTrader);

            //InventoryHandler.SendExchangeStartOkTaxCollectorMessage(FirstTrader.Character.Client, this);
        }

        public override void Close()
        {
            base.Close();

            InventoryHandler.SendExchangeLeaveMessage(FirstTrader.Character.Client, DialogTypeEnum.DIALOG_EXCHANGE,
                                                      FirstTrader.ReadyToApply);

            FirstTrader.Character.CloseDialog(this);
        }

        protected override void Apply()
        {
        }

        protected override void OnTraderItemMoved(Trader trader, TradeItem item, bool modified, int difference)
        {
            base.OnTraderItemMoved(trader, item, modified, difference);

            if (item.Stack == 0)
            {
                InventoryHandler.SendExchangeObjectRemovedMessage(FirstTrader.Character.Client, trader != FirstTrader, item.Guid);
            }
            else if (modified)
            {
                InventoryHandler.SendExchangeObjectModifiedMessage(FirstTrader.Character.Client, trader != FirstTrader, item);
            }
            else
            {
                InventoryHandler.SendExchangeObjectAddedMessage(FirstTrader.Character.Client, trader != FirstTrader, item);
            }
        }
    }
}
