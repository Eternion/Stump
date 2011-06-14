
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

            if (kamas > trader.Character.Inventory.Kamas)
                kamas = (uint) trader.Character.Inventory.Kamas;

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
            SourceTrader.Character.Inventory.SetKamas(
                (int) (SourceTrader.Character.Inventory.Kamas + (TargetTrader.Kamas - SourceTrader.Kamas)));
            TargetTrader.Character.Inventory.SetKamas(
                (int) (TargetTrader.Character.Inventory.Kamas + (SourceTrader.Kamas - TargetTrader.Kamas)));

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