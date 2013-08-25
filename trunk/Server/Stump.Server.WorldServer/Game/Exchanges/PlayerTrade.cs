using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Exchanges.Items;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges
{
    public class PlayerTrade : Trade<PlayerTrader, PlayerTrader>
    {
        public PlayerTrade(int id)
            : base(id)
        {
        }

        public override ExchangeTypeEnum ExchangeType
        {
            get
            {
                return ExchangeTypeEnum.PLAYER_TRADE;
            }
        }

        public override void Open(PlayerTrader firstTrader, PlayerTrader secondTrader)
        {
            base.Open(firstTrader, secondTrader);

            InventoryHandler.SendExchangeStartedWithPodsMessage(FirstTrader.Character.Client, this);
            InventoryHandler.SendExchangeStartedWithPodsMessage(SecondTrader.Character.Client, this);
        }

        public override void Close()
        {
            base.Close();

            InventoryHandler.SendExchangeLeaveMessage(FirstTrader.Character.Client, DialogTypeEnum.DIALOG_EXCHANGE, 
                                                      FirstTrader.ReadyToApply && SecondTrader.ReadyToApply);
            InventoryHandler.SendExchangeLeaveMessage(SecondTrader.Character.Client, DialogTypeEnum.DIALOG_EXCHANGE,
                                                      FirstTrader.ReadyToApply && SecondTrader.ReadyToApply);

            FirstTrader.Character.ResetDialog();
            SecondTrader.Character.ResetDialog();
        }

        protected override void Apply()
        {
            FirstTrader.Character.Inventory.SetKamas(
                (int) (FirstTrader.Character.Inventory.Kamas + (SecondTrader.Kamas - FirstTrader.Kamas)));
            SecondTrader.Character.Inventory.SetKamas(
                (int) (SecondTrader.Character.Inventory.Kamas + (FirstTrader.Kamas - SecondTrader.Kamas)));

            // trade items
            foreach (var tradeItem in FirstTrader.Items)
            {
                var item = FirstTrader.Character.Inventory.TryGetItem(tradeItem.Guid);

                FirstTrader.Character.Inventory.ChangeItemOwner(
                    SecondTrader.Character, item, (uint)tradeItem.Stack);
            }

            foreach (var tradeItem in SecondTrader.Items)
            {
                var item = SecondTrader.Character.Inventory.TryGetItem(tradeItem.Guid);

                SecondTrader.Character.Inventory.ChangeItemOwner(
                    FirstTrader.Character, item, (uint)tradeItem.Stack);
            }

            InventoryHandler.SendInventoryWeightMessage(FirstTrader.Character.Client);
            InventoryHandler.SendInventoryWeightMessage(SecondTrader.Character.Client);
        }

        protected override void OnTraderItemMoved(Trader trader, TradeItem item, bool modified, int difference)
        {
            base.OnTraderItemMoved(trader, item, modified, difference);

            if (item.Stack == 0)
            {
                InventoryHandler.SendExchangeObjectRemovedMessage(FirstTrader.Character.Client, trader != FirstTrader, item.Guid);
                InventoryHandler.SendExchangeObjectRemovedMessage(SecondTrader.Character.Client, trader != SecondTrader, item.Guid);
            }
            else if (modified)
            {
                InventoryHandler.SendExchangeObjectModifiedMessage(FirstTrader.Character.Client, trader != FirstTrader, item);
                InventoryHandler.SendExchangeObjectModifiedMessage(SecondTrader.Character.Client, trader != SecondTrader, item);
            }
            else
            {
                InventoryHandler.SendExchangeObjectAddedMessage(FirstTrader.Character.Client, trader != FirstTrader, item);
                InventoryHandler.SendExchangeObjectAddedMessage(SecondTrader.Character.Client, trader != SecondTrader, item);
            }
        }

        protected override void OnTraderKamasChanged(Trader trader, uint amount)
        {
            base.OnTraderKamasChanged(trader, amount);

            InventoryHandler.SendExchangeKamaModifiedMessage(FirstTrader.Character.Client, trader != FirstTrader,
                                                             (int) amount);
            InventoryHandler.SendExchangeKamaModifiedMessage(SecondTrader.Character.Client, trader != SecondTrader,
                                                             (int) amount);
        }

        protected override void OnTraderReadyStatusChanged(Trader trader, bool status)
        {
            base.OnTraderReadyStatusChanged(trader, status);

            InventoryHandler.SendExchangeIsReadyMessage(FirstTrader.Character.Client,
                                                        trader, status);
            InventoryHandler.SendExchangeIsReadyMessage(SecondTrader.Character.Client,
                                                        trader, status);
        }
    }
}