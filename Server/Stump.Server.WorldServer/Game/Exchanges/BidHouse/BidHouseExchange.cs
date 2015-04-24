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

        public BidHouseExchange(Character character, Npc npc, IEnumerable<int> types, bool buy)
        {
            Character = character;
            Npc = npc;
            Types = types;
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

        public bool Buy
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
            Character.SetDialoger(m_exchanger);

            if (Buy)
                InventoryHandler.SendExchangeStartedBidBuyerMessage(Character.Client, this);
            else
            {
                var items = BidHouseManager.Instance.GetBidHouseItems(Character.Id);
                InventoryHandler.SendExchangeStartedBidSellerMessage(Character.Client, this, items.Select(x => x.GetObjectItemToSellInBid()));
            }       
        }

        public void Close()
        {
            InventoryHandler.SendExchangeLeaveMessage(Character.Client, DialogType, false);
            Character.CloseDialog(this);
        }

        #endregion

        #region Network

        public SellerBuyerDescriptor GetBuyerDescriptor()
        {
            return new SellerBuyerDescriptor(BidHouseManager.Quantities, Types, BidHouseManager.TaxPercent, 60, 7, Npc.Id, (short)BidHouseManager.UnsoldDelay);
        }

        #endregion
    }
}
