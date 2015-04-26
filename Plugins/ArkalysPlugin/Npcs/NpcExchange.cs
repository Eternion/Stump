using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Npcs.Actions;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Exchanges;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Npcs;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;

namespace ArkalysPlugin.Npcs
{
    public static class NpcExchange
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static int NpcId = 1419;

        public static NpcMessage Message;

        private static ItemTemplate m_orbeTemplate;
        private static bool m_scriptDisabled;

        [Initialization(typeof(NpcManager), Silent = true)]
        public static void Initialize()
        {
            if (m_scriptDisabled)
                return;

            var npc = NpcManager.Instance.GetNpcTemplate(NpcId);

            if (npc == null)
            {
                logger.Error("Npc {0} not found, script is disabled", NpcId);
                m_scriptDisabled = true;
                return;
            }

            npc.NpcSpawned += OnNpcSpawned;
        }

        [Initialization(typeof(OrbsManager), Silent = true)]
        public static void InitializeItem()
        {
            if (OrbsManager.OrbItemTemplate != null)
                return;

            logger.Error("No orb item, script is disabled");
            m_scriptDisabled = true;
        }

        private static void OnNpcSpawned(NpcTemplate template, Npc npc)
        {
            if (m_scriptDisabled)
                template.NpcSpawned -= OnNpcSpawned;

            npc.Actions.RemoveAll(x => x.ActionType.Contains(NpcActionTypeEnum.ACTION_EXCHANGE));
            npc.Actions.Add(new NpcExchangeActionScript());
        }
    }

    public class NpcExchangeActionScript : NpcAction
    {
        public override NpcActionTypeEnum[] ActionType
        {
            get { return new [] { NpcActionTypeEnum.ACTION_EXCHANGE }; }
        }

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new NpcExchangeDialog(OrbsManager.OrbsExchangeRate, character, npc);
            dialog.Open();
        }
    }

    public class NpcExchangeDialog : NpcTrade
    {
        public NpcExchangeDialog(double rate, Character character, Npc npc)
             : base(character, npc)
        {
            OrbToKamasRate = rate;
        }

        protected override void OnTraderItemMoved(Trader trader, TradeItem item, bool modified, int difference)
        {
            base.OnTraderItemMoved(trader, item, modified, difference);

            if (trader is PlayerTrader)
            {
                AdjustLoots();
            }
        }

        public double OrbToKamasRate
        {
            get;
            set;
        }

        private void AdjustLoots()
        {
            foreach (var item in FirstTrader.Items.ToArray().Where(item => item.Template.Id != OrbsManager.OrbItemTemplate.Id))
            {
                FirstTrader.MoveItemToInventory(item.Guid, item.Stack);
            }

            var orbs = FirstTrader.Items.FirstOrDefault(x => x.Template == OrbsManager.OrbItemTemplate);

            if (orbs != null)
            {
                SecondTrader.SetKamas((int) (orbs.Stack*OrbToKamasRate));
            }
            else
            {
                SecondTrader.SetKamas(0);
            }
        }
    }
}
