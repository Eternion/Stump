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
using Stump.Server.WorldServer.Items;

namespace Stump.Server.WorldServer.Exchange
{
    ///<summary>
    ///</summary>
    ///<remarks>
    ///  TODO : Manage trade between npc and player
    ///</remarks>
    public class Trade : IDialog, IInstance
    {
        public Trade(Trader sourceTrader, Trader targetTrader)
        {
            SourceTrader = sourceTrader;
            TargetTrader = targetTrader;

            SourceTrader.Trade = this;
            TargetTrader.Trade = this;
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
                    ExchangeHandler.SendExchangeLeaveMessage(((Character) SourceTrader.Entity).Client, false);
                    ExchangeHandler.SendExchangeLeaveMessage(((Character) TargetTrader.Entity).Client, false);
                }
            }
            catch
            {
            }
            finally
            {
                // reset proprieties

                ((Character) SourceTrader.Entity).Dialog = null;
                ((Character) SourceTrader.Entity).Dialoger = null;

                ((Character) TargetTrader.Entity).Dialog = null;
                ((Character) TargetTrader.Entity).Dialoger = null;
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

            if (trader.Entity is Character)
            {
                if (!(trader.Entity as Character).Inventory.HasItem(guid))
                    return;

                IEnumerable<Item> presentsItems = trader.Items.Where(entry => entry.Guid == guid);

                if (presentsItems.Count() > 0)
                {
                    if ((trader.Entity as Character).Inventory.GetItem(guid).Stack >=
                        presentsItems.First().Stack + amount)
                    {
                        presentsItems.First().Stack += amount;

                        ExchangeHandler.SendExchangeObjectModifiedMessage(((Character) SourceTrader.Entity).Client,
                                                                          presentsItems.First());
                        ExchangeHandler.SendExchangeObjectModifiedMessage(((Character) TargetTrader.Entity).Client,
                                                                          presentsItems.First());
                    }
                }
                else
                {
                    var itemcopy = new Item(trader.Entity, (trader.Entity as Character).Inventory.GetItem(guid), amount);
                    trader.Items.Add(itemcopy);

                    ExchangeHandler.SendExchangeObjectAddedMessage(((Character) SourceTrader.Entity).Client,
                                                                   itemcopy);
                    ExchangeHandler.SendExchangeObjectAddedMessage(((Character) TargetTrader.Entity).Client,
                                                                   itemcopy);
                }
            }
        }

        public void RemoveItem(Trader trader, long guid, uint amount)
        {
            if (trader != SourceTrader &&
                trader != TargetTrader)
                return;

            ToggleReady(SourceTrader, false);
            ToggleReady(TargetTrader, false);


            if (trader.Entity is Character)
            {
                IEnumerable<Item> items = trader.Items.Where(entry => entry.Guid == guid);

                if (items.Count() != 1)
                    return;

                Item item = items.First();

                if (item.Stack - amount > 0) // unstack
                {
                    item.Stack -= amount;

                    ExchangeHandler.SendExchangeObjectModifiedMessage(((Character) SourceTrader.Entity).Client,
                                                                      item);
                    ExchangeHandler.SendExchangeObjectModifiedMessage(((Character) TargetTrader.Entity).Client,
                                                                      item);
                }
                else // delete
                {
                    trader.Items.Remove(item);

                    ExchangeHandler.SendExchangeObjectRemovedMessage(((Character) SourceTrader.Entity).Client,
                                                                     (int) item.Guid);
                    ExchangeHandler.SendExchangeObjectRemovedMessage(((Character) TargetTrader.Entity).Client,
                                                                     (int) item.Guid);
                }
            }
        }

        public void SetKamas(Trader trader, uint kamas)
        {
            if (trader != SourceTrader &&
                trader != TargetTrader)
                return;

            ToggleReady(SourceTrader, false);
            ToggleReady(TargetTrader, false);

            if (trader.Entity is Character)
            {
                if (kamas > (trader.Entity as Character).Kamas)
                    kamas = (uint) (trader.Entity as Character).Kamas;

                trader.Kamas = kamas;

                ExchangeHandler.SendExchangeKamaModifiedMessage(((Character) SourceTrader.Entity).Client,
                                                                kamas);
                ExchangeHandler.SendExchangeKamaModifiedMessage(((Character) TargetTrader.Entity).Client,
                                                                kamas);
            }
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
                ExchangeHandler.SendExchangeIsReadyMessage(((Character) SourceTrader.Entity).Client,
                                                           (int) trader.Entity.Id, state);
                ExchangeHandler.SendExchangeIsReadyMessage(((Character) TargetTrader.Entity).Client,
                                                           (int) trader.Entity.Id, state);
            }

            if (SourceTrader.Ready && TargetTrader.Ready)
                EndDialog();
        }

        public void ApplyExchange()
        {
            // set kamas
            if (SourceTrader.Entity is Character)
            {
                ((Character) SourceTrader.Entity).SetKamas(
                    (int) (((Character) SourceTrader.Entity).Kamas + (TargetTrader.Kamas - SourceTrader.Kamas)));
            }
            if (TargetTrader.Entity is Character)
            {
                ((Character) TargetTrader.Entity).SetKamas(
                    (int) (((Character) TargetTrader.Entity).Kamas + (SourceTrader.Kamas - TargetTrader.Kamas)));
            }

            // trade items
            if (TargetTrader.Entity is Character)
                foreach (Item item in SourceTrader.Items)
                {
                    if (SourceTrader.Entity is Character) // different if trader is an npc
                    {
                        ((Character) SourceTrader.Entity).Inventory.ChangeItemOwner(
                            ((Character) TargetTrader.Entity), item.Guid, item.Stack);
                    }
                }

            if (SourceTrader.Entity is Character)
                foreach (Item item in TargetTrader.Items)
                {
                    if (TargetTrader.Entity is Character) // different if trader is an npc
                    {
                        ((Character) TargetTrader.Entity).Inventory.ChangeItemOwner(
                            ((Character) SourceTrader.Entity), item.Guid, item.Stack);
                    }
                }

            InventoryHandler.SendInventoryWeightMessage(((Character) SourceTrader.Entity).Client);
            InventoryHandler.SendInventoryWeightMessage(((Character) TargetTrader.Entity).Client);

            // Exchange ended
            ExchangeHandler.SendExchangeLeaveMessage(((Character) SourceTrader.Entity).Client, true);
            ExchangeHandler.SendExchangeLeaveMessage(((Character) TargetTrader.Entity).Client, true);
        }
    }
}