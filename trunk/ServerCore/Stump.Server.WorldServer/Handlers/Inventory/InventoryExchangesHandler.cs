// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Exchange;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Items;

namespace Stump.Server.WorldServer.Handlers
{
    public partial class InventoryHandler : WorldHandlerContainer
    {
        [WorldHandler(typeof(ExchangePlayerRequestMessage))]
        public static void HandleExchangePlayerRequestMessage(WorldClient client, ExchangePlayerRequestMessage message)
        {
            switch ((ExchangeTypeEnum) message.exchangeType)
            {
                case ExchangeTypeEnum.PLAYER_TRADE:
                    Character target = World.Instance.GetCharacter(message.target);

                    if (target == null)
                    {
                        SendExchangeErrorMessage(client, ExchangeErrorEnum.BID_SEARCH_ERROR);
                        return;
                    }

                    if (target.Map.Id != client.ActiveCharacter.Map.Id)
                    {
                        SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_CHARACTER_TOOL_TOO_FAR);
                        return;
                    }

                    if (target.IsInDialog || target.IsDialogRequested)
                    {
                        SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_CHARACTER_OCCUPIED);
                        return;
                    }

                    var dialogRequest = new TradeRequest(client.ActiveCharacter, target);
                    client.ActiveCharacter.DialogRequest = dialogRequest;
                    target.DialogRequest = dialogRequest;


                    SendExchangeRequestedTradeMessage(client, (ExchangeTypeEnum)message.exchangeType, client.ActiveCharacter, target);
                    SendExchangeRequestedTradeMessage(target.Client, (ExchangeTypeEnum)message.exchangeType, client.ActiveCharacter, target);

                    break;
                default:
                    SendExchangeErrorMessage(client, ExchangeErrorEnum.REQUEST_IMPOSSIBLE);
                    break;
            }
        }

        [WorldHandler(typeof(ExchangeAcceptMessage))]
        public static void HandleExchangeAcceptMessage(WorldClient client, ExchangeAcceptMessage message)
        {
            if (client.ActiveCharacter.IsDialogRequested &&
               client.ActiveCharacter.DialogRequest is TradeRequest &&
               client.ActiveCharacter.DialogRequest.Target == client.ActiveCharacter)
            {
                client.ActiveCharacter.DialogRequest.AcceptDialog();
            }
        }

        [WorldHandler(typeof(ExchangeObjectMoveKamaMessage))]
        public static void HandleExchangeObjectMoveKamaMessage(WorldClient client, ExchangeObjectMoveKamaMessage message)
        {
           ( (PlayerTrade) client.ActiveCharacter.Dialog ).SetKamas((Trader)client.ActiveCharacter.Dialoger, (uint) message.quantity);
        }

        [WorldHandler(typeof(ExchangeObjectMoveMessage))]
        public static void HandleExchangeObjectMoveMessage(WorldClient client, ExchangeObjectMoveMessage message)
        {
                var guid = (long) message.objectUID;

                if (message.quantity > 0)
                    ( (PlayerTrade) client.ActiveCharacter.Dialog ).AddItem((Trader)client.ActiveCharacter.Dialoger, guid, (uint)message.quantity);
                else if (message.quantity < 0)
                    ( (PlayerTrade) client.ActiveCharacter.Dialog ).RemoveItem((Trader)client.ActiveCharacter.Dialoger, guid, (uint)( -message.quantity ));
        }

        [WorldHandler(typeof(ExchangeReadyMessage))]
        public static void HandleExchangeReadyMessage(WorldClient client, ExchangeReadyMessage message)
        {
                ( (PlayerTrade) client.ActiveCharacter.Dialog ).ToggleReady((Trader)client.ActiveCharacter.Dialoger, message.ready);
        }

        public static void SendExchangeRequestedTradeMessage(WorldClient client, ExchangeTypeEnum type, Character source,
                                                             Character target)
        {
            client.Send(new ExchangeRequestedTradeMessage(
                            (byte) type,
                            (uint) source.Id,
                            (uint) target.Id));
        }

        public static void SendExchangeStartedWithPodsMessage(WorldClient client, PlayerTrade playerTrade)
        {
            client.Send(new ExchangeStartedWithPodsMessage(
                            (byte) ExchangeTypeEnum.PLAYER_TRADE,
                            (int) playerTrade.SourceTrader.Character.Id,
                            playerTrade.SourceTrader.Character.Inventory.Weight,
                            playerTrade.SourceTrader.Character.Inventory.WeightTotal,
                            (int)playerTrade.SourceTrader.Character.Id,
                            playerTrade.TargetTrader.Character.Inventory.Weight,
                            playerTrade.TargetTrader.Character.Inventory.WeightTotal
                            ));
        }

        public static void SendExchangeLeaveMessage(WorldClient client, bool success)
        {
            client.Send(new ExchangeLeaveMessage(success));
        }

        public static void SendExchangeObjectAddedMessage(WorldClient client, bool remote, Item item)
        {
            client.Send(new ExchangeObjectAddedMessage(remote, item.ToNetworkItem()));
        }

        public static void SendExchangeObjectModifiedMessage(WorldClient client, bool remote, Item item)
        {
            client.Send(new ExchangeObjectModifiedMessage(remote, item.ToNetworkItem()));
        }

        public static void SendExchangeObjectRemovedMessage(WorldClient client, bool remote, long guid)
        {
            client.Send(new ExchangeObjectRemovedMessage(remote, (uint) guid));
        }

        public static void SendExchangeIsReadyMessage(WorldClient client, Trader trader, bool ready)
        {
            client.Send(new ExchangeIsReadyMessage((uint)trader.Character.Id, ready));
        }

        public static void SendExchangeErrorMessage(WorldClient client, ExchangeErrorEnum errorEnum)
        {
            client.Send(new ExchangeErrorMessage((byte) errorEnum));
        }
    }
}