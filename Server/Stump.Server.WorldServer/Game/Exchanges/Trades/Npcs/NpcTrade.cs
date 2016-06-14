#region License GNU GPL
// NpcTrade.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using System.Globalization;
using System.Linq;
using MongoDB.Bson;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Logging;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges.Trades.Npcs
{
    public class NpcTrade : Trade<PlayerTrader, NpcTrader>
    {
        public NpcTrade(Character character, Npc npc)
        {
            FirstTrader = new PlayerTrader(character, this);
            SecondTrader = new NpcTrader(npc, this);
        }

        public override ExchangeTypeEnum ExchangeType
        {
            get { return ExchangeTypeEnum.NPC_TRADE; }
        }

        public override void Open()
        {
            base.Open();
            FirstTrader.Character.SetDialoger(FirstTrader);

            InventoryHandler.SendExchangeStartOkNpcTradeMessage(FirstTrader.Character.Client, this);
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
            // check all items are still there
            if (!FirstTrader.Items.All(x =>
                {
                    var item = FirstTrader.Character.Inventory.TryGetItem(x.Guid);
                    
                    return item != null && item.Stack >= x.Stack;
                }))
            {
                return;
            }

            //Check if kamas still here
            if (FirstTrader.Character.Inventory.Kamas < FirstTrader.Kamas)
                return;

            FirstTrader.Character.Inventory.SetKamas((int)(FirstTrader.Character.Inventory.Kamas + (SecondTrader.Kamas - FirstTrader.Kamas)));


            // trade items
            foreach (var tradeItem in FirstTrader.Items)
            {
                var item = FirstTrader.Character.Inventory.TryGetItem(tradeItem.Guid);
                    FirstTrader.Character.Inventory.RemoveItem(item, (int)tradeItem.Stack);
            }

            foreach (var tradeItem in SecondTrader.Items)
            {
                FirstTrader.Character.Inventory.AddItem(tradeItem.Template, (int)tradeItem.Stack);
            }

            InventoryHandler.SendInventoryWeightMessage(FirstTrader.Character.Client);

            var document = new BsonDocument
                    {
                        { "NpcId", SecondTrader.Npc.TemplateId },
                        { "PlayerId", FirstTrader.Character.Id },
                        { "PlayerName", FirstTrader.Character.Name },
                        { "NpcKamas", SecondTrader.Kamas },
                        { "PlayerItems", FirstTrader.ItemsString },
                        { "NpcItems", SecondTrader.ItemsString },
                        { "Date", DateTime.Now.ToString(CultureInfo.InvariantCulture) }
                    };

            MongoLogger.Instance.Insert("NpcTrade", document);
        }

        protected override void OnTraderReadyStatusChanged(Trader trader, bool status)
        {
            base.OnTraderReadyStatusChanged(trader, status);

            InventoryHandler.SendExchangeIsReadyMessage(FirstTrader.Character.Client,
                                                        trader, status);

            if (trader is PlayerTrader && status)
            {
                SecondTrader.ToggleReady(true);
            }
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

        protected override void OnTraderKamasChanged(Trader trader, uint amount)
        {
            base.OnTraderKamasChanged(trader, amount);

            InventoryHandler.SendExchangeKamaModifiedMessage(FirstTrader.Character.Client, trader != FirstTrader,
                                                             (int)amount);
        }
    }
}