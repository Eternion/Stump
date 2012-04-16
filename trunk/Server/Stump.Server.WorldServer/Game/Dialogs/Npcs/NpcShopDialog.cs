using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Database.Items.Shops;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Dialogs;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Dialogs.Npcs
{
    public class NpcShopDialog : IDialog
    {
        public NpcShopDialog(Character character, Npc npc, IEnumerable<NpcItem> items)
        {
            Character = character;
            Npc = npc;
            Items = items;
        }

        public IEnumerable<NpcItem> Items
        {
            get;
            private set;
        }

        public Character Character
        {
            get;
            private set;
        }

        public Npc Npc
        {
            get;
            private set;
        }

        public void Open()
        {
            Character.SetDialog(this);
            InventoryHandler.SendExchangeStartOkNpcShopMessage(Character.Client, this);
        }

        public void Close()
        {
            DialogHandler.SendLeaveDialogMessage(Character.Client);
            Character.ResetDialog();
        }

        public void BuyItem(int itemId, uint amount)
        {
            var itemToSell = Items.Where(entry => entry.Item.Id == itemId).FirstOrDefault();

            if (itemToSell == null)
            {
                Character.Client.Send(new ExchangeErrorMessage((int)ExchangeErrorEnum.BUY_ERROR));
                return;
            }

            var finalPrice = (int) (itemToSell.Price * amount);

            if (finalPrice < 0 || Character.Inventory.Kamas < finalPrice)
            {
                Character.Client.Send(new ExchangeErrorMessage((int)ExchangeErrorEnum.BUY_ERROR));
                return;
            }

            BasicHandler.SendTextInformationMessage(Character.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 46, finalPrice);
            BasicHandler.SendTextInformationMessage(Character.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 21, amount, itemId);
            
            var item = ItemManager.Instance.CreatePlayerItem(Character, itemId, amount);

            Character.Inventory.AddItem(item);
            Character.Inventory.SubKamas(finalPrice);

            Character.Client.Send(new ExchangeBuyOkMessage());
        }

        public void SellItem(int guid, uint amount)
        {
           var item = Character.Inventory.TryGetItem(guid);

           if (item == null)
           {
               Character.Client.Send(new ExchangeErrorMessage((int)ExchangeErrorEnum.SELL_ERROR));
               return;
           }

            var saleItem = Items.Where(entry => entry.Item.Id == item.Template.Id).FirstOrDefault();
    
            int price;

            if (saleItem != null)
                price = (int)Math.Ceiling(saleItem.Price / 10);
            else
                price = (int)Math.Ceiling(item.Template.Price / 10);

            if (price == 0)
                price = 1;

            BasicHandler.SendTextInformationMessage(Character.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 45, price);
            BasicHandler.SendTextInformationMessage(Character.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 22, amount, item.Template.Id);

            Character.Inventory.RemoveItem(item, amount);
            Character.Inventory.AddKamas(price);

            Character.Client.Send(new ExchangeSellOkMessage());
        }
    }
}