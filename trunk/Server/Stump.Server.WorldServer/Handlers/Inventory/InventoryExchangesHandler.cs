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

namespace Stump.Server.WorldServer.Handlers.Inventory
{
    public partial class InventoryHandler
    {
        [WorldHandler(ExchangePlayerRequestMessage.Id)]
        public static void HandleExchangePlayerRequestMessage(WorldClient client, ExchangePlayerRequestMessage message)
        {
            switch ((ExchangeTypeEnum) message.exchangeType)
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
            
            client.Character.Trader.SetKamas((uint) message.quantity);
        }

        [WorldHandler(ExchangeObjectMoveMessage.Id)]
        public static void HandleExchangeObjectMoveMessage(WorldClient client, ExchangeObjectMoveMessage message)
        {
            if (!client.Character.IsTrading())
                return;

            client.Character.Trader.MoveItem(message.objectUID, message.quantity);
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
                client.Character.NpcShopDialog.BuyItem(message.objectToBuyId, (uint) message.quantity);
        }

        [WorldHandler(ExchangeSellMessage.Id)]
        public static void HandleExchangeSellMessage(WorldClient client, ExchangeSellMessage message)
        {
            if (client.Character.NpcShopDialog != null)
                client.Character.NpcShopDialog.SellItem(message.objectToSellId, (uint)message.quantity);
        }

        public static void SendExchangeRequestedTradeMessage(IPacketReceiver client, ExchangeTypeEnum type, Character source,
                                                             Character target)
        {
            client.Send(new ExchangeRequestedTradeMessage(
                            (sbyte) type,
                            source.Id,
                            target.Id));
        }

        public static void SendExchangeStartedWithPodsMessage(IPacketReceiver client, PlayerTrade playerTrade)
        {
            client.Send(new ExchangeStartedWithPodsMessage(
                            (sbyte) ExchangeTypeEnum.PLAYER_TRADE,
                            playerTrade.FirstTrader.Character.Id,
                            (int) playerTrade.FirstTrader.Character.Inventory.Weight,
                            (int) playerTrade.FirstTrader.Character.Inventory.WeightTotal,
                            playerTrade.SecondTrader.Character.Id,
                            (int) playerTrade.SecondTrader.Character.Inventory.Weight,
                            (int) playerTrade.SecondTrader.Character.Inventory.WeightTotal
                            ));
        }

        public static void SendExchangeStartOkNpcShopMessage(IPacketReceiver client, NpcShopDialog dialog)
        {
            client.Send(new ExchangeStartOkNpcShopMessage(dialog.Npc.Id, dialog.Token != null ? dialog.Token.Id : 0, dialog.Items.Select(entry => entry.GetNetworkItem() as ObjectItemToSellInNpcShop)));
        }

        public static void SendExchangeLeaveMessage(IPacketReceiver client, ExchangeTypeEnum exchangeType, bool success)
        {
            client.Send(new ExchangeLeaveMessage((sbyte)exchangeType, success));
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
            client.Send(new ExchangeErrorMessage((sbyte) errorEnum));
        }
    }
}