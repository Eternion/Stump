using System;
using System.Globalization;
using System.Linq;
using MongoDB.Bson;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Logging;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges.Trades.Players
{
    public class PlayerTrade : Trade<PlayerTrader, PlayerTrader>
    {
        public PlayerTrade(Character first, Character second)
        {
            FirstTrader = new PlayerTrader(first, this);
            SecondTrader = new PlayerTrader(second, this);
        }

        public override ExchangeTypeEnum ExchangeType
        {
            get
            {
                return ExchangeTypeEnum.PLAYER_TRADE;
            }
        }

        public override void Open()
        {
            base.Open();
            FirstTrader.Character.SetDialoger(FirstTrader);
            SecondTrader.Character.SetDialoger(SecondTrader);

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

            FirstTrader.Character.CloseDialog(this);
            SecondTrader.Character.CloseDialog(this);
        }

        protected override void Apply()
        {
            // check all items are still there
            if (!FirstTrader.Items.All(x =>
            {
                var item = FirstTrader.Character.Inventory.TryGetItem(x.Guid);

                return item != null && item.Stack >= x.Stack && !item.IsEquiped();
            }))
            {
                return;
            }

            if (!SecondTrader.Items.All(x =>
            {
                var item = SecondTrader.Character.Inventory.TryGetItem(x.Guid);

                return item != null && item.Stack >= x.Stack && !item.IsEquiped();
            }))
            {
                return;
            }

            //Check if kamas still here
            if (FirstTrader.Character.Inventory.Kamas < FirstTrader.Kamas ||
                SecondTrader.Character.Inventory.Kamas < SecondTrader.Kamas)
                return;

            FirstTrader.Character.Inventory.SetKamas(
                (int) (FirstTrader.Character.Inventory.Kamas + (SecondTrader.Kamas - FirstTrader.Kamas)));
            SecondTrader.Character.Inventory.SetKamas(
                (int) (SecondTrader.Character.Inventory.Kamas + (FirstTrader.Kamas - SecondTrader.Kamas)));

            // trade items
            foreach (var tradeItem in FirstTrader.Items)
            {
                var item = FirstTrader.Character.Inventory.TryGetItem(tradeItem.Guid);

                FirstTrader.Character.Inventory.ChangeItemOwner(
                    SecondTrader.Character, item, (int)tradeItem.Stack);
            }

            foreach (var tradeItem in SecondTrader.Items)
            {
                var item = SecondTrader.Character.Inventory.TryGetItem(tradeItem.Guid);

                SecondTrader.Character.Inventory.ChangeItemOwner(
                    FirstTrader.Character, item, (int)tradeItem.Stack);
            }

            InventoryHandler.SendInventoryWeightMessage(FirstTrader.Character.Client);
            InventoryHandler.SendInventoryWeightMessage(SecondTrader.Character.Client);

            var document = new BsonDocument
                    {
                        { "FirstTraderId", FirstTrader.Character.Id },
                        { "FirstTraderName", FirstTrader.Character.Name },
                        { "SecondTraderId", SecondTrader.Character.Id },
                        { "SecondTraderName", SecondTrader.Character.Name },
                        { "FirstTraderKamas", FirstTrader.Kamas },
                        { "SecondTraderKamas", SecondTrader.Kamas },
                        { "FirstTraderItems", FirstTrader.ItemsString },
                        { "SecondTraderItems", SecondTrader.ItemsString },
                        { "Date", DateTime.Now.ToString(CultureInfo.InvariantCulture) }
                    };

            MongoLogger.Instance.Insert("PlayerTrade", document);
        }

        protected override void OnTraderItemMoved(Trader trader, TradeItem item, bool modified, int difference)
        {
            base.OnTraderItemMoved(trader, item, modified, difference);

            FirstTrader.ToggleReady(false);
            SecondTrader.ToggleReady(false);

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