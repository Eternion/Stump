using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Merchants;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;
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
        }

        public Character Character
        {
            get;
        }

        public DialogTypeEnum DialogType => DialogTypeEnum.DIALOG_EXCHANGE;

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

            if (item == null || item.Stack <= 0 || quantity <= 0 || !CanBuy(item, quantity))
            {
                Character.Client.Send(new ExchangeErrorMessage((int)ExchangeErrorEnum.BUY_ERROR));
                return false;
            }

            if (Character.Inventory.IsFull(item.Template, quantity))
            {
                Character.Client.Send(new ExchangeErrorMessage((int)ExchangeErrorEnum.REQUEST_CHARACTER_OVERLOADED));
                return false;
            }

            var removed = Merchant.Bag.RemoveItem(item, quantity);

            var newItem = ItemManager.Instance.CreatePlayerItem(Character, item.Template, removed, item.Effects);
            Character.Inventory.AddItem(newItem);

            var finalPrice = item.Price * removed;
            Character.Inventory.SubKamas((int)finalPrice);

            //Vous avez perdu %1 kamas.
            Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 46, finalPrice);

            //Vous avez obtenu %1 '$item%2'.
            Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 21, newItem.Stack, newItem.Template.Id);

            Character.Client.Send(new ExchangeBuyOkMessage());

            Merchant.Save();
            Character.SaveLater();

            return true;
        }

        public bool CanBuy(MerchantItem item, int amount) => Character.Inventory.Kamas >= item.Price * amount || !Merchant.CanBeSee(Character);

        public bool SellItem(int id, int quantity)
        {
            Character.Client.Send(new ExchangeErrorMessage((int)ExchangeErrorEnum.SELL_ERROR));
            return false;
        }
    }
}