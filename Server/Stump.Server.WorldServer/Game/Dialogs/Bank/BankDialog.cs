using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Dialogs.Bank
{
    public class BankDialog : IDialog
    {
        public BankDialog(Character character)
        {
            Character = character;
        }

        public Character Character
        {
            get;
            private set;
        }

        public DialogTypeEnum DialogType
        {
            get { return DialogTypeEnum.DIALOG_EXCHANGE; }
        }

        public void Open()
        {
            if (Character.CheckBankIsLoaded(OpenCallBack))
                OpenCallBack();
        }

        public void OpenCallBack()
        {
            Character.SetDialog(this);
        }

        public void Close()
        {
            Character.ResetDialog();
        }
    }
}