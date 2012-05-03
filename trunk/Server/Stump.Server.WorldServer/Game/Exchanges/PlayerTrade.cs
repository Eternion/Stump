using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges
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

        public ExchangeTypeEnum Type
        {
            get
            {
                return ExchangeTypeEnum.PLAYER_TRADE;
            }
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

            InventoryHandler.SendExchangeLeaveMessage(FirstTrader.Character.Client, Type,
                                                      FirstTrader.ReadyToApply && SecondTrader.ReadyToApply);
            InventoryHandler.SendExchangeLeaveMessage(SecondTrader.Character.Client, Type,
                                                      FirstTrader.ReadyToApply && SecondTrader.ReadyToApply);

            FirstTrader.Character.ResetDialog();
            SecondTrader.Character.ResetDialog();
        }

        #endregion

        private void Apply()
        {
            FirstTrader.Character.Inventory.SetKamas(
                (int) (FirstTrader.Character.Inventory.Kamas + (SecondTrader.Kamas - FirstTrader.Kamas)));
            SecondTrader.Character.Inventory.SetKamas(
                (int) (SecondTrader.Character.Inventory.Kamas + (FirstTrader.Kamas - SecondTrader.Kamas)));

            // trade items
            foreach (PlayerItem item in FirstTrader.Items)
            {
                FirstTrader.Character.Inventory.ChangeItemOwner(
                    SecondTrader.Character, item, (uint) item.Stack);
            }

            foreach (PlayerItem item in SecondTrader.Items)
            {
                SecondTrader.Character.Inventory.ChangeItemOwner(
                    FirstTrader.Character, item, (uint) item.Stack);
            }

            InventoryHandler.SendInventoryWeightMessage(FirstTrader.Character.Client);
            InventoryHandler.SendInventoryWeightMessage(SecondTrader.Character.Client);
        }

        private void OnTraderItemMoved(ITrader trader, PlayerItem item, bool modified, int difference)
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
            InventoryHandler.SendExchangeKamaModifiedMessage(FirstTrader.Character.Client, trader != FirstTrader,
                                                             (int) amount);
            InventoryHandler.SendExchangeKamaModifiedMessage(SecondTrader.Character.Client, trader != SecondTrader,
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