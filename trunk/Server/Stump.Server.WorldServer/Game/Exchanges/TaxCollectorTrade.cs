using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Handlers.Inventory;
using Stump.Server.WorldServer.Handlers.TaxCollector;

namespace Stump.Server.WorldServer.Game.Exchanges
{
    public class TaxCollectorTrade : Trade<CollectorTrader, EmptyTrader>
    {
        public TaxCollectorTrade(TaxCollectorNpc taxCollector, Character character)
        {
            TaxCollector = taxCollector;
            Character = character;
            FirstTrader = new CollectorTrader(taxCollector, character, this);
            SecondTrader = new EmptyTrader(taxCollector.Id, this);
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
        public override ExchangeTypeEnum ExchangeType
        {
            get { return ExchangeTypeEnum.TAXCOLLECTOR; }
        }

        #region IDialog Members

        public override void Open()
        {
            base.Open();

            Character.SetDialoger(FirstTrader);
            TaxCollector.OnDialogOpened(this);

            InventoryHandler.SendStorageInventoryContentMessage(Character.Client, TaxCollector);

            //Attention, la fenêtre d'échange se fermera automatiquement dans %1 minutes.
            Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 139, 2);
        }


        public override void Close()
        {
            base.Close();

            InventoryHandler.SendExchangeLeaveMessage(Character.Client, DialogType, false);
            Character.CloseDialog(this);
            TaxCollector.OnDialogClosed(this);
            
            TaxCollector.Guild.AddXP(TaxCollector.GatheredExperience);
            //<b>%3</b> a relevé la collecte sur le percepteur %1 en <b>%2</b> et recolté : %4
            TaxCollectorHandler.SendTaxCollectorMovementMessage(TaxCollector.Guild.Clients, false, TaxCollector, Character.Name);

            TaxCollector.Delete();
        }

        protected override void Apply()
        {
            // nothing to do here
        }

        #endregion
    }
}
