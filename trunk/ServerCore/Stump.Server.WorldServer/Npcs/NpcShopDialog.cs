using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
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

        public void BuyItem(int itemId, uint amount)
        {
            var itemToSell = ItemsToSell.Where(entry => entry.ItemId == itemId).FirstOrDefault();

            if (itemToSell == null)
            {
                Dialoger.Character.Client.Send(new ExchangeErrorMessage((int)ExchangeErrorEnum.BUY_ERROR));
                return;
            }
            long finalPrice = itemToSell.Price == 0 ? itemToSell.Template.Price*amount : itemToSell.Price*amount;

            if (Dialoger.Character.Inventory.Kamas < finalPrice)
            {
                Dialoger.Character.Client.Send(new ExchangeErrorMessage((int) ExchangeErrorEnum.BUY_ERROR));
                return;
            }

            BasicHandler.SendTextInformationMessage(Dialoger.Character.Client, 0, 46, finalPrice);
            BasicHandler.SendTextInformationMessage(Dialoger.Character.Client, 0, 21, amount, itemId);

            Dialoger.Character.Inventory.AddItem(itemId, amount);
            Dialoger.Character.Inventory.SubKamas(finalPrice);

            Dialoger.Character.Client.Send(new ExchangeBuyOkMessage());
        }

        public void SellItem(long guid, uint amount)
        {
            var item = Dialoger.Character.Inventory.GetItem(guid);

            if (item == null)
            {
                Dialoger.Character.Client.Send(new ExchangeErrorMessage((int)ExchangeErrorEnum.SELL_ERROR));
                return;
            }

            long price = item.Template.Price/10;

            if (price == 0)
                price = 1;

            BasicHandler.SendTextInformationMessage(Dialoger.Character.Client, 0, 45, price);
            BasicHandler.SendTextInformationMessage(Dialoger.Character.Client, 0, 22, amount, item.ItemId);

            Dialoger.Character.Inventory.DeleteItem(guid, amount);
            Dialoger.Character.Inventory.AddKamas(price);

            Dialoger.Character.Client.Send(new ExchangeSellOkMessage());
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
