using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;
using Stump.Server.WorldServer.Game.Exchanges;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Database.Items.Templates;

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
            if (!client.Character.MerchantBag.HasItem(message.objectUID))
            {
                if (!client.Character.IsTrading())
                    return;

                client.Character.Trader.MoveItem(message.objectUID, message.quantity);
            }
            else
            {
                MerchantItem mItem = client.Character.MerchantBag.TryGetItem(message.objectUID);
                bool result = client.Character.MerchantBag.MoveToInventory(mItem);

                client.Send(new ExchangeShopStockMovementRemovedMessage(
                                message.objectUID));
            }
        }

        [WorldHandler(ExchangeReadyMessage.Id)]
        public static void HandleExchangeReadyMessage(WorldClient client, ExchangeReadyMessage message)
        {
            client.Character.Trader.ToggleReady(message.ready);
        }

        [WorldHandler(ExchangeBuyMessage.Id)]
        public static void HandleExchangeBuyMessage(WorldClient client, ExchangeBuyMessage message)
        {
            if (client.Character.NpcShopDialog != null)
                client.Character.NpcShopDialog.BuyItem(message.objectToBuyId, (uint)message.quantity);
        }

        [WorldHandler(ExchangeSellMessage.Id)]
        public static void HandleExchangeSellMessage(WorldClient client, ExchangeSellMessage message)
        {
            if (client.Character.NpcShopDialog != null)
                client.Character.NpcShopDialog.SellItem(message.objectToSellId, (uint)message.quantity);
        }

        [WorldHandler(ExchangeShowVendorTaxMessage.Id)]
        public static void HandleExchangeShowVendorTaxMessage(WorldClient client, ExchangeShowVendorTaxMessage message)
        {
            int objectValue = 0;
            int totalTax = client.Character.MerchantBag.CalcMerchantTax();

            if (totalTax <= 0)
                totalTax = 1;

            client.Send(new ExchangeReplyTaxVendorMessage(
                            objectValue,
                            totalTax));
        }

        [WorldHandler(ExchangeRequestOnShopStockMessage.Id)]
        public static void HandleExchangeRequestOnShopStockMessage(WorldClient client, ExchangeRequestOnShopStockMessage message)
        {
            client.Send(new ExchangeShopStockStartedMessage(
                            client.Character.MerchantBag.Select(x => new ObjectItemToSell((short)x.Template.Id, 0, false, x.Effects.Select(effect => effect.GetObjectEffect()), x.Guid, x.Stack, x.Price))
                            ));
        }

        [WorldHandler(ExchangeObjectMovePricedMessage.Id)]
        public static void HandleExchangeObjectMovePricedMessage(WorldClient client, ExchangeObjectMovePricedMessage message)
        {
            PlayerItem pItem = client.Character.Inventory.TryGetItem(message.objectUID);
            MerchantItem result = client.Character.Inventory.MoveToMerchantBag(pItem, message.quantity, message.price);

            if (result != null)
            {
                client.Send(new ExchangeShopStockMovementUpdatedMessage(
                                new ObjectItemToSell((short)result.Template.Id, 0, false, result.Template.Effects.Select(x => x.GetObjectEffect()), result.Guid, result.Stack, result.Price
                                )));

                InventoryHandler.SendObjectQuantityMessage(client, pItem);
                InventoryHandler.SendInventoryWeightMessage(client);
            }
        }

        [WorldHandler(ExchangeObjectModifyPricedMessage.Id)]
        public static void HandleExchangeObjectModifyPricedMessage(WorldClient client, ExchangeObjectModifyPricedMessage message)
        {
            MerchantItem mItem = client.Character.MerchantBag.TryGetItem(message.objectUID);
            mItem.setPrice(message.price);

            if (message.quantity != mItem.Stack && message.quantity != 0)
            {
                if (message.quantity < mItem.Stack)
                {
                    client.Character.MerchantBag.ModifyQuantity(mItem, message.quantity);
                }

                if (message.quantity > mItem.Stack)
                {
                    PlayerItem pItem = client.Character.Inventory.TryGetItem(mItem.Template);
                    client.Character.Inventory.MoveToMerchantBag(pItem, (message.quantity - mItem.Stack), message.price);

                    InventoryHandler.SendObjectQuantityMessage(client, pItem);
                }

                client.Send(new ExchangeShopStockMovementUpdatedMessage(
                                new ObjectItemToSell((short)mItem.Template.Id, 0, false, mItem.Template.Effects.Select(x => x.GetObjectEffect()), message.objectUID, message.quantity, message.price
                                )));

                InventoryHandler.SendInventoryWeightMessage(client);
            }
            else
            {
                client.Send(new ExchangeShopStockMovementUpdatedMessage(
                                new ObjectItemToSell((short)mItem.Template.Id, 0, false, mItem.Template.Effects.Select(x => x.GetObjectEffect()), message.objectUID, mItem.Stack, message.price
                                )));
            }
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
                            (int)playerTrade.FirstTrader.Character.Inventory.Weight,
                            (int)playerTrade.FirstTrader.Character.Inventory.WeightTotal,
                            playerTrade.SecondTrader.Character.Id,
                            (int)playerTrade.SecondTrader.Character.Inventory.Weight,
                            (int)playerTrade.SecondTrader.Character.Inventory.WeightTotal
                            ));
        }

        public static void SendExchangeStartOkNpcShopMessage(IPacketReceiver client, NpcShopDialog dialog)
        {
            client.Send(new ExchangeStartOkNpcShopMessage(dialog.Npc.Id, dialog.Token != null ? dialog.Token.Id : 0, dialog.Items.Select(entry => entry.GetNetworkItem() as ObjectItemToSellInNpcShop)));
        }

        public static void SendExchangeLeaveMessage(IPacketReceiver client, DialogTypeEnum dialogType, bool success)
        {
            client.Send(new ExchangeLeaveMessage((sbyte)dialogType, success));
        }

        public static void SendExchangeObjectAddedMessage(IPacketReceiver client, bool remote, PlayerItem item)
        {
            client.Send(new ExchangeObjectAddedMessage(remote, item.GetObjectItem()));
        }

        public static void SendExchangeObjectModifiedMessage(IPacketReceiver client, bool remote, PlayerItem item)
        {
            client.Send(new ExchangeObjectModifiedMessage(remote, item.GetObjectItem()));
        }

        public static void SendExchangeObjectRemovedMessage(IPacketReceiver client, bool remote, int guid)
        {
            client.Send(new ExchangeObjectRemovedMessage(remote, guid));
        }

        public static void SendExchangeIsReadyMessage(IPacketReceiver client, ITrader trader, bool ready)
        {
            client.Send(new ExchangeIsReadyMessage(trader.Actor.Id, ready));
        }

        public static void SendExchangeErrorMessage(IPacketReceiver client, ExchangeErrorEnum errorEnum)
        {
            client.Send(new ExchangeErrorMessage((sbyte)errorEnum));
        }

        public static void SendExchangeShopStockStartedMessage(WorldClient client)
        {
            client.Send(new ExchangeShopStockStartedMessage(
                            client.Character.MerchantBag.Select(x => new ObjectItemToSell((short)x.Template.Id, 0, false, x.Effects.Select(effect => effect.GetObjectEffect()), x.Guid, x.Stack, x.Price))
                            ));
        }
    }
}