using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Database.Items.Shops;
using Stump.Server.WorldServer.Database.Items.Templates;
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
            CanSell = true;
        }

        public NpcShopDialog(Character character, Npc npc, IEnumerable<NpcItem> items, ItemTemplate token)
        {
            Character = character;
            Npc = npc;
            Items = items;
            Token = token;
            CanSell = true;
        }

        public IEnumerable<NpcItem> Items
        {
            get;
            private set;
        }

        public ItemTemplate Token
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

        public bool CanSell
        {
            get;
            set;
        }

        public bool MaxStats
        {
            get;
            set;
        }

        #region IDialog Members

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

        #endregion

        public void BuyItem(int itemId, uint amount)
        {
            NpcItem itemToSell = Items.Where(entry => entry.Item.Id == itemId).FirstOrDefault();

            if (itemToSell == null)
            {
                Character.Client.Send(new ExchangeErrorMessage((int) ExchangeErrorEnum.BUY_ERROR));
                return;
            }

            var finalPrice = (int) (itemToSell.Price*amount);

            if (!CanBuy(itemToSell, amount))
            {
                Character.Client.Send(new ExchangeErrorMessage((int) ExchangeErrorEnum.BUY_ERROR));
                return;
            }

            BasicHandler.SendTextInformationMessage(Character.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE,
                                                    21, amount, itemId);

            PlayerItem item = ItemManager.Instance.CreatePlayerItem(Character, itemId, amount, MaxStats || itemToSell.MaxStats);

            Character.Inventory.AddItem(item);
            if (Token != null)
            {
                Character.Inventory.UnStackItem(Character.Inventory.TryGetItem(Token), finalPrice);
            }
            else
            {
                Character.Inventory.SubKamas(finalPrice);
                BasicHandler.SendTextInformationMessage(Character.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE,
                                                        46, finalPrice);
            }

            Character.Client.Send(new ExchangeBuyOkMessage());
        }

        public bool CanBuy(NpcItem item, uint amount)
        {
            if (Token != null)
            {
                var token = Character.Inventory.TryGetItem(Token);

                if (token == null || token.Stack < item.Price * amount)
                    return false;
            }

            else
            {
                if (Character.Inventory.Kamas < item.Price * amount)
                    return false;
            }

            return true;
        }

        public void SellItem(int guid, uint amount)
        {
            if (!CanSell)
            {
                Character.Client.Send(new ExchangeErrorMessage((int) ExchangeErrorEnum.SELL_ERROR));
                return;
            }

            PlayerItem item = Character.Inventory.TryGetItem(guid);

            if (item == null)
            {
                Character.Client.Send(new ExchangeErrorMessage((int) ExchangeErrorEnum.SELL_ERROR));
                return;
            }

            if (item.Stack < amount)
            {
                Character.Client.Send(new ExchangeErrorMessage((int)ExchangeErrorEnum.SELL_ERROR));
                return;
            } 
            
            NpcItem saleItem = Items.FirstOrDefault(entry => entry.Item.Id == item.Template.Id);

            int price;

            if (saleItem != null)
                price = (int) ((int) Math.Ceiling(saleItem.Price/10) * amount);
            else
                price = (int) ((int)Math.Ceiling(item.Template.Price / 10) * amount);

            BasicHandler.SendTextInformationMessage(Character.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE,
                                                    22, amount, item.Template.Id);

            Character.Inventory.RemoveItem(item, amount);

            if (Token != null)
            {
                Character.Inventory.AddItem(Token, (uint) price);
            }
            else
            {
                Character.Inventory.AddKamas(price);
                BasicHandler.SendTextInformationMessage(Character.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE,
                                                        45, price);
            }

            Character.Client.Send(new ExchangeSellOkMessage());
        }
    }
}