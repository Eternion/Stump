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
            private set;
        }

        public ExchangeTypeEnum ExchangeType
        {
            get { return ExchangeTypeEnum.BANK; }
        }

        public DialogTypeEnum DialogType
        {
            get { return DialogTypeEnum.DIALOG_EXCHANGE; }
        }

        public void Open()
        {
            if (Character.CheckBankIsLoaded(() => Character.Area.AddMessage(OpenCallBack)))
                OpenCallBack();
        }

        private void OpenCallBack()
        {
            InventoryHandler.SendExchangeStartedMessage(Character.Client, ExchangeType);
            InventoryHandler.SendStorageInventoryContentMessage(Character.Client, Customer.Character.Bank);
            Character.SetDialoger(Customer);
        }

        public void Close()
        {
            Character.ResetDialog();
        }
    }
}