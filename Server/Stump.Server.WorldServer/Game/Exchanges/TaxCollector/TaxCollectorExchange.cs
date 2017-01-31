using Stump.Core.Mathematics;
using Stump.Core.Timers;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Handlers.Inventory;
using Stump.Server.WorldServer.Handlers.TaxCollector;
using System.Linq;

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

        private TimedTimerEntry Timer
        {
            get;
            set;
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
            Timer = Character.Area.CallDelayed((5 * 60 * 1000), Close);
        }

        public void Close()
        {
            Character.Area.UnregisterTimer(Timer);

            Character.CloseDialog(this);
            TaxCollector.OnDialogClosed(this);

            if (TaxCollector.IsDisposed)
                return;

            TaxCollectorHandler.SendGetExchangeGuildTaxCollectorMessage(TaxCollector.Guild.Clients, GetExchangeGuildTaxCollector());
            InventoryHandler.SendExchangeLeaveMessage(Character.Client, DialogType, false);

            TaxCollector.Guild.AddXP(TaxCollector.GatheredExperience);
            TaxCollector.Delete();
        }

        #endregion

        public ExchangeGuildTaxCollectorGetMessage GetExchangeGuildTaxCollector()
        {
            return new ExchangeGuildTaxCollectorGetMessage($"{TaxCollector.FirstNameId.ToBase(36)},{TaxCollector.LastNameId.ToBase(36)}", (short)TaxCollector.Position.Map.Position.X, (short)TaxCollector.Position.Map.Position.Y, TaxCollector.Position.Map.Id,
                (short)TaxCollector.Position.Map.SubArea.Id, Character.Name, TaxCollector.Record.CallerId, TaxCollector.Record.CallerName, TaxCollector.GatheredExperience,
                (short)m_collector.RecoltedItems.Values.Sum(x => x.Template.RealWeight * x.Stack), m_collector.RecoltedItems.Values.Select(x => x.GetObjectItemGenericQuantity()));
        }
    }
}
