using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges.Bank
{
    public class BankDialog : IExchange
    {
        public BankDialog(Character character)
        {
            Customer = new BankCustomer(character, this);
        }

        public Character Character => Customer.Character;

        public BankCustomer Customer
        {
            get;
        }

        public ExchangeTypeEnum ExchangeType => ExchangeTypeEnum.BANK;

        public DialogTypeEnum DialogType => DialogTypeEnum.DIALOG_EXCHANGE;

        public void Open()
        {
            InventoryHandler.SendExchangeStartedWithStorageMessage(Character.Client, ExchangeType, int.MaxValue);
            InventoryHandler.SendStorageInventoryContentMessage(Character.Client, Customer.Character.Bank);
            Character.SetDialoger(Customer);
        }

        public void Close()
        {
            InventoryHandler.SendExchangeLeaveMessage(Character.Client, DialogType, false);
            Character.ResetDialog();
        }
    }
}