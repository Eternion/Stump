using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Handlers.Dialogs;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Dialogs.Npcs
{
    public class NpcBidDialog : IDialog
    {
        public NpcBidDialog(Character character, Npc npc, IEnumerable<int> types, bool buy)
        {
            Character = character;
            Npc = npc;
            Types = types;
            Buy = buy;
        }

        public DialogTypeEnum DialogType
        {
            get { return DialogTypeEnum.DIALOG_EXCHANGE; }
        }

        public Character Character
        {
            get;
            protected set;
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

        #region IDialog Members
        
        public void Open()
        {
            Character.SetDialog(this);

            if (Buy)
                InventoryHandler.SendExchangeStartedBidBuyerMessage(Character.Client, this);
            else
                InventoryHandler.SendExchangeStartedBidSellerMessage(Character.Client, this);
        }

        public void Close()
        {
            DialogHandler.SendLeaveDialogMessage(Character.Client, DialogType);
            Character.CloseDialog(this);
        }

        #endregion

        #region Methods

        public IEnumerable<ItemTemplate> GetItemsForType(int typeId)
        {
            return ItemManager.Instance.GetTemplates().Where(x => x.TypeId == typeId);
        }

        public IEnumerable<BidExchangerObjectInfo> GetBidsForItem(int itemId)
        {
            return new BidExchangerObjectInfo[0];
        }

        #endregion

        #region Network

        public SellerBuyerDescriptor GetBuyerDescriptor()
        {
            return new SellerBuyerDescriptor(new[] {1, 10, 100}, Types, 2, 0, 63, Npc.Id, 7);
        }

        #endregion
    }
}
