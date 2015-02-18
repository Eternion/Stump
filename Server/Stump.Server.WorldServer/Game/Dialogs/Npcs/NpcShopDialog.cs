using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Database.Items.Shops;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Dialogs;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Dialogs.Npcs
{
    public class NpcShopDialog : IShopDialog
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

        public DialogTypeEnum DialogType
        {
            get
            {
                return DialogTypeEnum.DIALOG_PURCHASABLE;
            }
        }

        public IEnumerable<NpcItem> Items
        {
            get;
            protected set;
        }

        public ItemTemplate Token
        {
            get;
            protected set;
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
            DialogHandler.SendLeaveDialogMessage(Character.Client, DialogType);
            Character.CloseDialog(this);
        }

        #endregion

        public virtual bool BuyItem(int itemId, int amount)
        {
            var itemToSell = Items.FirstOrDefault(entry => entry.Item.Id == itemId);

            if (itemToSell == null)
            {
                Character.Client.Send(new ExchangeErrorMessage((int) ExchangeErrorEnum.BUY_ERROR));
                return false;
            }

            var finalPrice = (int) (itemToSell.Price*amount);

            if (amount <= 0 || !CanBuy(itemToSell, amount))
            {
                Character.Client.Send(new ExchangeErrorMessage((int) ExchangeErrorEnum.BUY_ERROR));
                return false;
            }

            BasicHandler.SendTextInformationMessage(Character.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE,
                                                    21, amount, itemId);

            var item = ItemManager.Instance.CreatePlayerItem(Character, itemId, amount, MaxStats || itemToSell.MaxStats);

            //Todo: Find better way to assign Mount Effects
            var mount = MountManager.Instance.CreateMount(Character, itemId);
            if (mount == null)
                Character.Inventory.AddItem(item);
            else
                finalPrice = (int)itemToSell.Price;

            if (Token != null)
            {
                Character.Inventory.UnStackItem(Character.Inventory.TryGetItem(Token), finalPrice);
                // decrease performance, not the right way to fix it
                /*WorldServer.Instance.IOTaskPool.AddMessage(() =>
                {
                    Character.Inventory.Save();
                });*/
            }
            else
            {
                Character.Inventory.SubKamas(finalPrice);
            }

            Character.Client.Send(new ExchangeBuyOkMessage());
            return true;
        }

        public bool CanBuy(NpcItem item, int amount)
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

        public bool SellItem(int guid, int amount)
        {
            if (!CanSell || amount <= 0)
            {
                Character.Client.Send(new ExchangeErrorMessage((int) ExchangeErrorEnum.SELL_ERROR));
                return false;
            }

            var item = Character.Inventory.TryGetItem(guid);

            if (item == null)
            {
                Character.Client.Send(new ExchangeErrorMessage((int) ExchangeErrorEnum.SELL_ERROR));
                return false;
            }

            if (item.Stack < amount)
            {
                Character.Client.Send(new ExchangeErrorMessage((int)ExchangeErrorEnum.SELL_ERROR));
                return false;
            } 
            
            var saleItem = Items.FirstOrDefault(entry => entry.Item.Id == item.Template.Id);

            int price;

            if (saleItem != null)
                price = (int) Math.Ceiling(saleItem.Price/10) * amount;
            else
                price = (int) Math.Ceiling(item.Template.Price / 10) * amount;

            BasicHandler.SendTextInformationMessage(Character.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE,
                                                    22, amount, item.Template.Id);

            Character.Inventory.RemoveItem(item, amount);

            if (Token != null)
            {
                Character.Inventory.AddItem(Token, price);
            }
            else
            {
                Character.Inventory.AddKamas(price);
            }

            Character.Client.Send(new ExchangeSellOkMessage());
            return true;
        }
    }
}