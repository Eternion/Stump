using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Handlers.Inventory;
using Stump.Server.WorldServer.Handlers.TaxCollector;

namespace Stump.Server.WorldServer.Game.Exchanges.TaxCollector
{
    public class TaxCollectorExchange : IExchange
    {
        readonly CharacterCollector m_collector;

        public TaxCollectorExchange(TaxCollectorNpc taxCollector, Character character)
        {
            TaxCollector = taxCollector;
            Character = character;
            m_collector = new CharacterCollector(taxCollector, character, this);
        }

        public TaxCollectorNpc TaxCollector
        {
            get;
        }

        public Character Character
        {
            get;
        }

        public ExchangeTypeEnum ExchangeType => ExchangeTypeEnum.TAXCOLLECTOR;

        public DialogTypeEnum DialogType => DialogTypeEnum.DIALOG_EXCHANGE;

        #region IDialog Members

        public void Open()
        {
            Character.SetDialoger(m_collector);
            TaxCollector.OnDialogOpened(this);

            InventoryHandler.SendExchangeStartedTaxCollectorShopMessage(Character.Client, TaxCollector);

            //Attention, la fenêtre d'échange se fermera automatiquement dans %1 minutes.
            Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 139, 5);
            Character.Area.CallDelayed(5000, Close);
        }

        public void Close()
        {
            TaxCollectorHandler.SendGetExchangeGuildTaxCollectorMessage(TaxCollector.Guild.Clients, TaxCollector);

            InventoryHandler.SendExchangeLeaveMessage(Character.Client, DialogType, false);
            Character.CloseDialog(this);
            TaxCollector.OnDialogClosed(this);
            
            TaxCollector.Guild.AddXP(TaxCollector.GatheredExperience);
            TaxCollector.Delete();
        }

        #endregion
    }
}
