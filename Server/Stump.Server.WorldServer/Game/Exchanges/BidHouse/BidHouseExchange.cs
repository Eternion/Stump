using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Inventory;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Items.BidHouse;

namespace Stump.Server.WorldServer.Game.Exchanges.BidHouse
{
    public class BidHouseExchange : IExchange
    {
        private readonly BidHouseExchanger m_exchanger;

        public BidHouseExchange(Character character, Npc npc, IEnumerable<int> types, int maxItemLevel, bool buy)
        {
            Character = character;
            Npc = npc;
            Types = types;
            MaxItemLevel = maxItemLevel;
            Buy = buy;

            m_exchanger = new BidHouseExchanger(character, this);
        }

        public Character Character
        {
            get;
            private set;
        }

        public Npc Npc
        {
            get;
            protected set;
        }

        public IEnumerable<int> Types
        {
            get;
            protected set;
        }

        public int MaxItemLevel
        {
            get;
            protected set;
        }

        public bool Buy
        {
            get;
            protected set;
        }

        public int CurrentViewedItem
        {
            get;
            protected set;
        }

        public ExchangeTypeEnum ExchangeType
        {
            get { return Buy ? ExchangeTypeEnum.BIDHOUSE_BUY : ExchangeTypeEnum.BIDHOUSE_SELL; }
        }

        public DialogTypeEnum DialogType
        {
            get { return DialogTypeEnum.DIALOG_EXCHANGE; }
        }

        #region IDialog Members

        public void Open()
        {
            if (Character.Dialoger != null)
                Character.Dialoger.Dialog.Close();

            Character.SetDialoger(m_exchanger);

            if (Buy)
            {
                InventoryHandler.SendExchangeStartedBidBuyerMessage(Character.Client, this);
            } 
            else
            {
                var items = BidHouseManager.Instance.GetBidHouseItems(Character.Account.Id, Types);
                InventoryHandler.SendExchangeStartedBidSellerMessage(Character.Client, this, items.Select(x => x.GetObjectItemToSellInBid()));
            }

            BidHouseManager.Instance.ItemAdded += OnBidHouseItemAdded;
            BidHouseManager.Instance.ItemRemoved += OnBidHouseItemRemoved;
        }

        public void Close()
        {
            InventoryHandler.SendExchangeLeaveMessage(Character.Client, DialogType, false);
            Character.CloseDialog(this);

            BidHouseManager.Instance.ItemAdded -= OnBidHouseItemAdded;
            BidHouseManager.Instance.ItemRemoved -= OnBidHouseItemRemoved;
        }

        #region Functions

        public void UpdateCurrentViewedItem(int itemId)
        {
            CurrentViewedItem = itemId;

            var categories = BidHouseManager.Instance.GetBidHouseCategories(itemId, MaxItemLevel).Select(x => x.GetBidExchangerObjectInfo());

            InventoryHandler.SendExchangeTypesItemsExchangerDescriptionForUserMessage(Character.Client, categories);
        }

        #endregion

        #region Events

        private void OnBidHouseItemAdded(BidHouseItem item, BidHouseCategory category, bool newCategory)
        {
            if (!Types.Contains((int)item.Template.TypeId))
                return;

            if (!BidHouseManager.Instance.GetBidHouseCategories(item.Template.Id, MaxItemLevel).Any())
                InventoryHandler.SendExchangeBidHouseGenericItemAddedMessage(Character.Client, item);

            if (CurrentViewedItem != item.Template.Id)
                return;

            if (newCategory)
                InventoryHandler.SendExchangeBidHouseInListAddedMessage(Character.Client, category);
            else
                InventoryHandler.SendExchangeBidHouseInListUpdatedMessage(Character.Client, category);
        }

        private void OnBidHouseItemRemoved(BidHouseItem item, BidHouseCategory category, bool categoryDeleted)
        {
            if (!Types.Contains((int)item.Template.TypeId))
                return;

            if (!BidHouseManager.Instance.GetBidHouseCategories(item.Template.Id, MaxItemLevel).Any())
            {
                CurrentViewedItem = 0;
                InventoryHandler.SendExchangeBidHouseGenericItemRemovedMessage(Character.Client, item);
                UpdateCurrentViewedItem(item.Template.Id);

                return;
            }  

            if (CurrentViewedItem != item.Template.Id)
                return;

            if (categoryDeleted)
                InventoryHandler.SendExchangeBidHouseInListRemovedMessage(Character.Client, category);
            else
                InventoryHandler.SendExchangeBidHouseInListUpdatedMessage(Character.Client, category);
        }

        #endregion

        #endregion

        #region Network

        public SellerBuyerDescriptor GetBuyerDescriptor()
        {
            return new SellerBuyerDescriptor(BidHouseManager.Quantities, Types, BidHouseManager.TaxPercent, MaxItemLevel, Character.Level, Npc.Id, (short)BidHouseManager.UnsoldDelay);
        }

        #endregion
    }
}
