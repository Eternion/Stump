#region License GNU GPL
// MerchantTrade.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Merchants;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Dialogs.Merchants
{
    public class MerchantShopDialog : IShopDialog
    {
        public MerchantShopDialog(Merchant merchant, Character character)
        {
            Merchant = merchant;
            Character = character;
        }

        public Merchant Merchant
        {
            get;
            private set;
        }

        public Character Character
        {
            get;
            private set;
        }

        public DialogTypeEnum DialogType
        {
            get
            {
                return DialogTypeEnum.DIALOG_EXCHANGE;
            }
        }

        public void Open()
        {
            Character.SetDialog(this);
            Merchant.OnDialogOpened(this);
            InventoryHandler.SendExchangeStartOkHumanVendorMessage(Character.Client, Merchant);
        }

        public void Close()
        {
            InventoryHandler.SendExchangeLeaveMessage(Character.Client, DialogType, false);
            Character.CloseDialog(this);
            Merchant.OnDialogClosed(this);
        }

        public bool BuyItem(int itemGuid, int quantity)
        {
            var item = Merchant.Bag.FirstOrDefault(x => x.Guid == itemGuid);

            if (item == null || quantity <= 0 || !CanBuy(item, quantity))
            {
                Character.Client.Send(new ExchangeErrorMessage((int)ExchangeErrorEnum.BUY_ERROR));
                return false;
            }

            Merchant.Bag.RemoveItem(item, quantity);

            var newItem = ItemManager.Instance.CreatePlayerItem(Character, item.Template, quantity,
                                                            item.Effects);

            Character.Inventory.AddItem(newItem);
            BasicHandler.SendTextInformationMessage(Character.Client, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE,
                                                    21, quantity, item.Template.Id);

            var finalPrice = item.Price*quantity;
            Character.Inventory.SubKamas((int)finalPrice);

            Merchant.KamasEarned += (uint)finalPrice;

            Character.Client.Send(new ExchangeBuyOkMessage());

            Merchant.Save();
            Character.SaveLater();

            return true;
        }

        public bool CanBuy(MerchantItem item, int amount)
        {
            return Character.Inventory.Kamas >= item.Price * amount || !Merchant.CanBeSee(Character);
        }

        public bool SellItem(int id, int quantity)
        {
            Character.Client.Send(new ExchangeErrorMessage((int)ExchangeErrorEnum.SELL_ERROR));
            return false;
        }
    }
}