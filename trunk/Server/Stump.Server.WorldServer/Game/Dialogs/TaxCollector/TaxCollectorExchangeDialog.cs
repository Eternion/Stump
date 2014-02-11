using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Dialogs.TaxCollector
{
    public class TaxCollectorExchangeDialog : IDialog
    {
        public TaxCollectorExchangeDialog(TaxCollectorNpc taxCollector, Character character)
        {
            TaxCollector = taxCollector;
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

        #region IDialog Members

        public void Open()
        {
            Character.SetDialog(this);
            TaxCollector.OnDialogOpened(this);

            InventoryHandler.SendStorageInventoryContentMessage(Character.Client, TaxCollector);
            //Todo: Attention, la fenêtre d'échange se fermera automatiquement dans %1 minutes.
        }

        public void Close()
        {
            InventoryHandler.SendExchangeLeaveMessage(Character.Client, DialogType, false);
            Character.CloseDialog(this);
            TaxCollector.OnDialogClosed(this);
        }

        #endregion
    }
}
