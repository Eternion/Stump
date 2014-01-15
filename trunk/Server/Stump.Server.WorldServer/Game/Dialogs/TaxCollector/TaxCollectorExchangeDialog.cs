using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Dialogs.TaxCollector
{
    public class TaxCollectorExchangeDialog : IDialog
    {
        public TaxCollectorExchangeDialog(TaxCollectorNpc merchant, Character character)
        {
            TaxCollector = merchant;
            Character = character;
        }

        public TaxCollectorNpc TaxCollector
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
            TaxCollector.OnDialogOpened(this);
            InventoryHandler.SendExchangeGuildTaxCollector(Character.Client, TaxCollector.Id);
        }

        public void Close()
        {
            InventoryHandler.SendExchangeLeaveMessage(Character.Client, DialogType, false);
            Character.CloseDialog(this);
            TaxCollector.OnDialogClosed(this);
        }
    }
}
