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
using System.Collections.Generic;
using System.Linq;
using Stump.Server.BaseServer.Manager;
using Stump.Server.WorldServer.Dialog;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.Items;

namespace Stump.Server.WorldServer.Exchange
{
    public class PlayerTrade : IDialog, IInstance
    {
        public PlayerTrade(Character sourceTrader, Character targetTrader)
        {
            SourceTrader = new Trader(sourceTrader, this);
            TargetTrader = new Trader(targetTrader, this);
        }

        public Trader SourceTrader
        {
            get;
            private set;
        }

        public Trader TargetTrader
        {
            get;
            private set;
        }

        #region IDialog Members

        public void EndDialog()
        {
            try
            {
                if (SourceTrader.Ready && TargetTrader.Ready)
                {
                    ApplyExchange();
                }
                else // cancel trade
                {
                    InventoryHandler.SendExchangeLeaveMessage(SourceTrader.Character.Client, false);
                    InventoryHandler.SendExchangeLeaveMessage(TargetTrader.Character.Client, false);
                }
            }
            finally
            {
                // reset proprieties
                SourceTrader.Character.Dialoger = null;
                TargetTrader.Character.Dialoger = null;
            }
        }

        #endregion

        #region IInstance Members

        public int Id
        {
            get;
            set;
        }

        #endregion

        public void AddItem(Trader trader, long guid, uint amount)
        {
            if (trader != SourceTrader &&
                trader != TargetTrader)
                return;

            ToggleReady(SourceTrader, false);
            ToggleReady(TargetTrader, false);

            if (!trader.Character.Inventory.HasItem(guid))
                return;

            IEnumerable<Item> presentsItems = trader.Items.Where(entry => entry.Guid == guid);

            if (presentsItems.Count() > 0)
            {
                if (trader.Character.Inventory.GetItem(guid).Stack >=
                    presentsItems.First().Stack + amount)
                {
                    presentsItems.First().Stack += amount;

                    InventoryHandler.SendExchangeObjectModifiedMessage(SourceTrader.Character.Client,
                                                                       false,
                                                                       presentsItems.First());
                    InventoryHandler.SendExchangeObjectModifiedMessage(TargetTrader.Character.Client,
                                                                       false,
                                                                       presentsItems.First());
                }
            }
            else
            {
                var itemcopy = new Item(trader.Character.Inventory.GetItem(guid), amount);
                trader.Items.Add(itemcopy);

                InventoryHandler.SendExchangeObjectAddedMessage(SourceTrader.Character.Client, false,
                                                                itemcopy);
                InventoryHandler.SendExchangeObjectAddedMessage(TargetTrader.Character.Client, false,
                                                                itemcopy);
            }
        }

        public void RemoveItem(Trader trader, long guid, uint amount)
        {
            if (trader != SourceTrader &&
                trader != TargetTrader)
                return;

            ToggleReady(SourceTrader, false);
            ToggleReady(TargetTrader, false);
            IEnumerable<Item> items = trader.Items.Where(entry => entry.Guid == guid);

            if (items.Count() != 1)
                return;

            Item item = items.First();

            if (item.Stack - amount > 0) // unstack
            {
                item.Stack -= amount;

                InventoryHandler.SendExchangeObjectModifiedMessage(SourceTrader.Character.Client, false,
                                                                   item);
                InventoryHandler.SendExchangeObjectModifiedMessage(TargetTrader.Character.Client, false,
                                                                   item);
            }
            else // delete
            {
                trader.Items.Remove(item);

                InventoryHandler.SendExchangeObjectRemovedMessage(SourceTrader.Character.Client, false,
                                                                  (int) item.Guid);
                InventoryHandler.SendExchangeObjectRemovedMessage(TargetTrader.Character.Client, false,
                                                                  (int) item.Guid);
            }
        }

        public void SetKamas(Trader trader, uint kamas)
        {
            if (trader != SourceTrader &&
                trader != TargetTrader)
                return;

            ToggleReady(SourceTrader, false);
            ToggleReady(TargetTrader, false);

            if (kamas > trader.Character.Kamas)
                kamas = (uint) trader.Character.Kamas;

            trader.Kamas = kamas;

            InventoryHandler.SendExchangeKamaModifiedMessage(SourceTrader.Character.Client, false,
                                                             kamas);
            InventoryHandler.SendExchangeKamaModifiedMessage(TargetTrader.Character.Client, false,
                                                             kamas);
        }

        public void ToggleReady(Trader trader)
        {
            ToggleReady(trader, !trader.Ready);
        }

        public void ToggleReady(Trader trader, bool state)
        {
            if (trader != SourceTrader &&
                trader != TargetTrader)
                return;

            if (trader.Ready != state)
            {
                trader.Ready = state;
                InventoryHandler.SendExchangeIsReadyMessage(SourceTrader.Character.Client,
                                                            trader, state);
                InventoryHandler.SendExchangeIsReadyMessage(TargetTrader.Character.Client,
                                                            trader, state);
            }

            if (SourceTrader.Ready && TargetTrader.Ready)
                EndDialog();
        }

        public void ApplyExchange()
        {
            SourceTrader.Character.SetKamas(
                (int) (SourceTrader.Character.Kamas + (TargetTrader.Kamas - SourceTrader.Kamas)));
            TargetTrader.Character.SetKamas(
                (int) (TargetTrader.Character.Kamas + (SourceTrader.Kamas - TargetTrader.Kamas)));

            // trade items
            foreach (Item item in SourceTrader.Items)
            {
                SourceTrader.Character.Inventory.ChangeItemOwner(
                    TargetTrader.Character, item.Guid, item.Stack);
            }

            foreach (Item item in TargetTrader.Items)
            {
                TargetTrader.Character.Inventory.ChangeItemOwner(
                    SourceTrader.Character, item.Guid, item.Stack);
            }

            InventoryHandler.SendInventoryWeightMessage(SourceTrader.Character.Client);
            InventoryHandler.SendInventoryWeightMessage(TargetTrader.Character.Client);

            // Exchange ended
            InventoryHandler.SendExchangeLeaveMessage(SourceTrader.Character.Client, true);
            InventoryHandler.SendExchangeLeaveMessage(TargetTrader.Character.Client, true);
        }
    }
}