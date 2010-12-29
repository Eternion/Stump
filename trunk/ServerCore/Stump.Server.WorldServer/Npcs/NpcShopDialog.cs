using System;
using System.Collections.Generic;
using Stump.Server.WorldServer.Dialog;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.Items;

namespace Stump.Server.WorldServer.Npcs
{
    public class NpcShopDialog : IDialog
    {
        public NpcShopDialog(NpcSpawn npc, Character dialoger, List<ItemToSellInNpcShop> itemsToSell)
        {
            Npc = npc;
            Dialoger = new NpcShopDialoger(dialoger, this);
            ItemsToSell = itemsToSell;
        }

        public NpcSpawn Npc
        {
            get;
            set;
        }

        public NpcShopDialoger Dialoger
        {
            get;
            set;
        }

        public List<ItemToSellInNpcShop> ItemsToSell
        {
            get;
            set;
        }

        public void BuyItem()
        {
            throw new NotImplementedException();
        }

        public void SellItem()
        {
            throw new NotImplementedException();
        }

        public void EndDialog()
        {
            try
            {
                DialogHandler.SendLeaveDialogMessage(Dialoger.Character.Client);
            }
            finally
            {
                Dialoger.Character.Dialoger = null;
            }
        }
    }
}
