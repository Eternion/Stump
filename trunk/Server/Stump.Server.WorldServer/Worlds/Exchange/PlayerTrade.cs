using Stump.Server.WorldServer.Handlers.Inventory;
using Stump.Server.WorldServer.Worlds.Items;

namespace Stump.Server.WorldServer.Worlds.Exchange
{
    public class PlayerTrade : ITrade
    {
        public PlayerTrade(int id)
        {
            Id = id;
        }

        public PlayerTrader FirstTrader
        {
            get;
            internal set;
        }

        public PlayerTrader SecondTrader
        {
            get;
            internal set;
        }

        #region ITrade Members

        public int Id
        {
            get;
            private set;
        }

        public void Open()
        {
            FirstTrader.ItemMoved += OnTraderItemMoved;
            FirstTrader.KamasChanged += OnTraderKamasChanged;
            FirstTrader.ReadyStatusChanged += OnTraderReadyStatusChanged;

            SecondTrader.ItemMoved += OnTraderItemMoved;
            SecondTrader.KamasChanged += OnTraderKamasChanged;
            SecondTrader.ReadyStatusChanged += OnTraderReadyStatusChanged;

            InventoryHandler.SendExchangeStartedWithPodsMessage(FirstTrader.Character.Client, this);
            InventoryHandler.SendExchangeStartedWithPodsMessage(SecondTrader.Character.Client, this);
        }

        public void Close()
        {
            if (FirstTrader.ReadyToApply && SecondTrader.ReadyToApply)
                Apply();

            InventoryHandler.SendExchangeLeaveMessage(FirstTrader.Character.Client,
                                                      FirstTrader.ReadyToApply && SecondTrader.ReadyToApply);
            InventoryHandler.SendExchangeLeaveMessage(SecondTrader.Character.Client,
                                                      FirstTrader.ReadyToApply && SecondTrader.ReadyToApply);

            FirstTrader.Character.ResetDialoger();
            SecondTrader.Character.ResetDialoger();
        }

        #endregion

        private void Apply()
        {
            FirstTrader.Character.Inventory.SetKamas(
                (int) (FirstTrader.Character.Inventory.Kamas + (SecondTrader.Kamas - FirstTrader.Kamas)));
            SecondTrader.Character.Inventory.SetKamas(
                (int) (SecondTrader.Character.Inventory.Kamas + (FirstTrader.Kamas - SecondTrader.Kamas)));

            // trade items
            foreach (Item item in FirstTrader.Items)
            {
                FirstTrader.Character.Inventory.ChangeItemOwner(
                    SecondTrader.Character, item.Guid, (uint) item.Stack);
            }

            foreach (Item item in SecondTrader.Items)
            {
                SecondTrader.Character.Inventory.ChangeItemOwner(
                    FirstTrader.Character, item.Guid, (uint) item.Stack);
            }

            InventoryHandler.SendInventoryWeightMessage(FirstTrader.Character.Client);
            InventoryHandler.SendInventoryWeightMessage(SecondTrader.Character.Client);
        }

        private void OnTraderItemMoved(ITrader trader, Item item, bool modified, int difference)
        {
            if (item.Stack == 0)
            {
                InventoryHandler.SendExchangeObjectRemovedMessage(FirstTrader.Character.Client, false, item.Guid);
                InventoryHandler.SendExchangeObjectRemovedMessage(SecondTrader.Character.Client, false, item.Guid);
            }

            if (modified)
            {
                InventoryHandler.SendExchangeObjectModifiedMessage(FirstTrader.Character.Client, false, item);
                InventoryHandler.SendExchangeObjectModifiedMessage(SecondTrader.Character.Client, false, item);
            }
            else
            {
                InventoryHandler.SendExchangeObjectAddedMessage(FirstTrader.Character.Client, false, item);
                InventoryHandler.SendExchangeObjectAddedMessage(SecondTrader.Character.Client, false, item);
            }
        }

        private void OnTraderKamasChanged(ITrader trader, uint amount)
        {
            InventoryHandler.SendExchangeKamaModifiedMessage(FirstTrader.Character.Client, false,
                                                             (int) amount);
            InventoryHandler.SendExchangeKamaModifiedMessage(SecondTrader.Character.Client, false,
                                                             (int) amount);
        }

        private void OnTraderReadyStatusChanged(ITrader trader, bool status)
        {
            if (FirstTrader.ReadyToApply && SecondTrader.ReadyToApply)
                Close();

            InventoryHandler.SendExchangeIsReadyMessage(FirstTrader.Character.Client,
                                                        trader, status);
            InventoryHandler.SendExchangeIsReadyMessage(SecondTrader.Character.Client,
                                                        trader, status);
        }
    }
}