using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Merchants;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Dialogs;
using Stump.Server.WorldServer.Game.Dialogs.Merchants;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;
using Stump.Server.WorldServer.Game.Dialogs.TaxCollector;
using Stump.Server.WorldServer.Game.Exchanges;
using Stump.Server.WorldServer.Game.Exchanges.Items;
using Stump.Server.WorldServer.Game.Guilds;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Handlers.Inventory
{
    public partial class InventoryHandler
    {
        [WorldHandler(ExchangePlayerRequestMessage.Id)]
        public static void HandleExchangePlayerRequestMessage(WorldClient client, ExchangePlayerRequestMessage message)
        {
            switch ((ExchangeTypeEnum)message.exchangeType)
            {
                case ExchangeTypeEnum.PLAYER_TRADE:
                    var target = World.Instance.GetCharacter(message.target);

                    if (target == null)
                    {
                        SendExchangeErrorMessage(client, ExchangeErrorEnum.BID_SEARCH_ERROR);
                        return;
                    }

                    if (target.Map.Id != client.Character.Map.Id)
                    {
                        SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_CHARACTER_TOOL_TOO_FAR);
                        return;
                    }

                    if (target.IsInRequest() || target.IsTrading())
                    {
                        SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_CHARACTER_OCCUPIED);
                        return;
                    }

                    if (target.IsAway)
                    {
                        SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_CHARACTER_OCCUPIED);
                        return;
                    }

                    var request = new PlayerTradeRequest(client.Character, target);
                    client.Character.OpenRequestBox(request);
                    target.OpenRequestBox(request);

                    request.Open();

                    break;
                default:
                    SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_IMPOSSIBLE);
                    break;
            }
        }

        [WorldHandler(ExchangeAcceptMessage.Id)]
        public static void HandleExchangeAcceptMessage(WorldClient client, ExchangeAcceptMessage message)
        {
            if (client.Character.IsInRequest() &&
                client.Character.RequestBox is PlayerTradeRequest)
            {
                client.Character.AcceptRequest();
            }
        }

        [WorldHandler(ExchangeObjectMoveKamaMessage.Id)]
        public static void HandleExchangeObjectMoveKamaMessage(WorldClient client, ExchangeObjectMoveKamaMessage message)
        {
            if (!client.Character.IsTrading())
                return;

            client.Character.Trader.SetKamas((uint)message.quantity);
        }

        [WorldHandler(ExchangeObjectMoveMessage.Id)]
        public static void HandleExchangeObjectMoveMessage(WorldClient client, ExchangeObjectMoveMessage message)
        {
            if (client.Character.IsTrading())
                client.Character.Trader.MoveItem(message.objectUID, message.quantity);
            else if (client.Character.IsInTaxCollectorDialog())
            {
                if (message.quantity <= 0)
                {
                    var taxCollector = (client.Character.Dialog as TaxCollectorExchangeDialog).TaxCollector;
                    var taxCollectorItem = taxCollector.Bag.TryGetItem(message.objectUID);
                    if (taxCollectorItem == null)
                        return;
                }
                else
                    client.Character.SendSystemMessage(7, false);
            }
            else if (client.Character.IsInMerchantDialog() && message.quantity <= 0)
            {
                // he is modifying his merchant bag and remove an item
                var merchantItem = client.Character.MerchantBag.TryGetItem(message.objectUID);
                if (merchantItem == null)
                    return;

                if (client.Character.MerchantBag.MoveToInventory(merchantItem))
                    client.Send(new ExchangeShopStockMovementRemovedMessage(message.objectUID));
            }
        }

        [WorldHandler(ExchangeReadyMessage.Id)]
        public static void HandleExchangeReadyMessage(WorldClient client, ExchangeReadyMessage message)
        {
            if (message != null && client != null)
                client.Character.Trader.ToggleReady(message.ready);
        }

        [WorldHandler(ExchangeBuyMessage.Id)]
        public static void HandleExchangeBuyMessage(WorldClient client, ExchangeBuyMessage message)
        {
            var dialog = client.Character.Dialog as IShopDialog;
            if (dialog != null)
                dialog.BuyItem(message.objectToBuyId, (uint)message.quantity);
        }

        [WorldHandler(ExchangeSellMessage.Id)]
        public static void HandleExchangeSellMessage(WorldClient client, ExchangeSellMessage message)
        {
            var dialog = client.Character.Dialog as IShopDialog;
            if (dialog != null) 
                dialog.SellItem(message.objectToSellId, (uint)message.quantity);
        }

        [WorldHandler(ExchangeShowVendorTaxMessage.Id)]
        public static void HandleExchangeShowVendorTaxMessage(WorldClient client, ExchangeShowVendorTaxMessage message)
        {
            const int objectValue = 0;
            var totalTax = client.Character.MerchantBag.GetMerchantTax();

            if (totalTax <= 0)
                totalTax = 1;

            client.Send(new ExchangeReplyTaxVendorMessage(
                            objectValue,
                            totalTax));
        }

        [WorldHandler(ExchangeRequestOnShopStockMessage.Id)]
        public static void HandleExchangeRequestOnShopStockMessage(WorldClient client, ExchangeRequestOnShopStockMessage message)
        {
            SendExchangeShopStockStartedMessage(client, client.Character.MerchantBag);
        }

        [WorldHandler(ExchangeObjectMovePricedMessage.Id)]
        public static void HandleExchangeObjectMovePricedMessage(WorldClient client, ExchangeObjectMovePricedMessage message)
        {
            if (message.quantity <= 0 || message.price <= 0)
                return;

            var item = client.Character.Inventory.TryGetItem(message.objectUID);

            if (item == null) 
                return;

            if (client.Character.IsBusy())
                return;

            client.Character.Inventory.MoveToMerchantBag(item, (uint) message.quantity, (uint) message.price);
        }

        [WorldHandler(ExchangeObjectModifyPricedMessage.Id)]
        public static void HandleExchangeObjectModifyPricedMessage(WorldClient client, ExchangeObjectModifyPricedMessage message)
        {
            if (client.Character.IsBusy())
                return;

            if (message.price <= 0)
                return;

            var merchantItem = client.Character.MerchantBag.TryGetItem(message.objectUID);

            if (merchantItem == null)
                return;

            merchantItem.Price = (uint)message.price;

            if (message.quantity == merchantItem.Stack || message.quantity == 0)
            {
                SendExchangeShopStockMovementUpdatedMessage(client, merchantItem);
                return;
            }

            if (message.quantity < merchantItem.Stack)
                client.Character.MerchantBag.MoveToInventory(merchantItem,
                                                             (uint) (merchantItem.Stack - message.quantity));
            else
            {
                var playerItem = client.Character.Inventory.TryGetItem(merchantItem.Template);
                if (playerItem != null)
                    client.Character.Inventory.MoveToMerchantBag(playerItem,
                                                                 (uint) (message.quantity - merchantItem.Stack),
                                                                 (uint) message.price);
            }
        }

        [WorldHandler(ExchangeStartAsVendorMessage.Id)]
        public static void HandleExchangeStartAsVendorMessage(WorldClient client, ExchangeStartAsVendorMessage message)
        {
            client.Character.EnableMerchantMode();
        }

        [WorldHandler(ExchangeOnHumanVendorRequestMessage.Id)]
        public static void HandleExchangeOnHumanVendorRequestMessage(WorldClient client, ExchangeOnHumanVendorRequestMessage message)
        {
            var merchant = client.Character.Map.GetActor<Merchant>(message.humanVendorId);

            if (merchant == null || merchant.Cell.Id != message.humanVendorCell)
                return;

            var shop = new MerchantShopDialog(merchant, client.Character);
            shop.Open();
        }

        [WorldHandler(ExchangeRequestOnTaxCollectorMessage.Id)]
        public static void HandleExchangeRequestOnTaxCollectorMessage(WorldClient client, ExchangeRequestOnTaxCollectorMessage message)
        {
            if (client.Character.Guild == null)
                return;

            var taxCollectorNpc = GuildManager.Instance.FindTaxCollectorNpc(message.taxCollectorId);
            if (taxCollectorNpc == null)
                return;

            if (taxCollectorNpc.Guild.Id != client.Character.Guild.Id)
                return;
                //TODO: Send error Message

            var exchange = new TaxCollectorExchangeDialog(taxCollectorNpc, client.Character);
            exchange.Open();
        }

        public static void SendExchangeRequestedTradeMessage(IPacketReceiver client, ExchangeTypeEnum type, Character source,
                                                             Character target)
        {
            client.Send(new ExchangeRequestedTradeMessage(
                            (sbyte)type,
                            source.Id,
                            target.Id));
        }

        public static void SendExchangeStartedWithPodsMessage(IPacketReceiver client, PlayerTrade playerTrade)
        {
            client.Send(new ExchangeStartedWithPodsMessage(
                            (sbyte)ExchangeTypeEnum.PLAYER_TRADE,
                            playerTrade.FirstTrader.Character.Id,
                            playerTrade.FirstTrader.Character.Inventory.Weight,
                            (int)playerTrade.FirstTrader.Character.Inventory.WeightTotal,
                            playerTrade.SecondTrader.Character.Id,
                            playerTrade.SecondTrader.Character.Inventory.Weight,
                            (int)playerTrade.SecondTrader.Character.Inventory.WeightTotal
                            ));
        }

        public static void SendExchangeStartOkTaxCollectorMessage(IPacketReceiver client, TaxCollectorNpc taxCollector)
        {
            client.Send(new ExchangeStartOkTaxCollectorMessage(taxCollector.Id, taxCollector.Bag.Select(x => x.GetObjectItem()), 0));
        }

        public static void SendExchangeStartOkHumanVendorMessage(IPacketReceiver client, Merchant merchant)
        {
            client.Send(new ExchangeStartOkHumanVendorMessage(merchant.Id, merchant.Bag.Select(x => x.GetObjectItemToSellInHumanVendorShop())));
        }

        public static void SendExchangeStartOkNpcTradeMessage(IPacketReceiver client, NpcTrade trade)
        {
            client.Send(new ExchangeStartOkNpcTradeMessage(trade.SecondTrader.Npc.Id));
        }

        public static void SendExchangeStartOkNpcShopMessage(IPacketReceiver client, NpcShopDialog dialog)
        {
            client.Send(new ExchangeStartOkNpcShopMessage(dialog.Npc.Id, dialog.Token != null ? dialog.Token.Id : 0, dialog.Items.Select(entry => entry.GetNetworkItem() as ObjectItemToSellInNpcShop)));
        }

        public static void SendExchangeLeaveMessage(IPacketReceiver client, DialogTypeEnum dialogType, bool success)
        {
            client.Send(new ExchangeLeaveMessage((sbyte)dialogType, success));
        }

        public static void SendExchangeObjectAddedMessage(IPacketReceiver client, bool remote, TradeItem item)
        {
            client.Send(new ExchangeObjectAddedMessage(remote, item.GetObjectItem()));
        }

        public static void SendExchangeObjectModifiedMessage(IPacketReceiver client, bool remote, TradeItem item)
        {
            client.Send(new ExchangeObjectModifiedMessage(remote, item.GetObjectItem()));
        }

        public static void SendExchangeObjectRemovedMessage(IPacketReceiver client, bool remote, int guid)
        {
            client.Send(new ExchangeObjectRemovedMessage(remote, guid));
        }

        public static void SendExchangeIsReadyMessage(IPacketReceiver client, Trader trader, bool ready)
        {
            client.Send(new ExchangeIsReadyMessage(trader.Id, ready));
        }

        public static void SendExchangeErrorMessage(IPacketReceiver client, ExchangeErrorEnum errorEnum)
        {
            client.Send(new ExchangeErrorMessage((sbyte)errorEnum));
        }

        public static void SendExchangeShopStockStartedMessage(IPacketReceiver client, CharacterMerchantBag merchantBag)
        {
            client.Send(new ExchangeShopStockStartedMessage(
                            merchantBag.Select(x => x.GetObjectItemToSell())));
        }
    }
}