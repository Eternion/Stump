using System.Collections.Generic;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Npcs.Actions;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Npcs;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace GameplayPlugin.Npcs
{
    public static class NpcDofus
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static int NpcId = 3005;
        [Variable]
        public static int MessageId = 20024;
        [Variable]
        public static short ReplyId = 20025;

        [Variable]
        public static int RewardItemId = 20392;

        public static NpcMessage Message;

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

            Message = NpcManager.Instance.GetNpcMessage(MessageId);
        }

        private static void OnNpcSpawned(NpcTemplate template, Npc npc)
        {
            if (m_scriptDisabled)
                template.NpcSpawned -= OnNpcSpawned;

            npc.Actions.RemoveAll(x => x.ActionType.Contains(NpcActionTypeEnum.ACTION_EXCHANGE) || x.ActionType.Contains(NpcActionTypeEnum.ACTION_TALK));
            npc.Actions.Add(new NpcDofusActionScript());
            npc.Actions.Add(new NpcDofusTalkScript());
        }
    }

    public class NpcDofusTalkScript : NpcAction
    {
        public override NpcActionTypeEnum[] ActionType
        {
            get
            {
                return new [] { NpcActionTypeEnum.ACTION_TALK };
            }
        }

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new NpcDofusTalkDialog(character, npc);
            dialog.Open();
        }
    }

    public class NpcDofusTalkDialog : NpcDialog
    {
        public NpcDofusTalkDialog(Character character, Npc npc)
            : base(character, npc)
        {
            CurrentMessage = NpcDofus.Message;
        }

        public override void Open()
        {
            base.Open();

            ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage, new[] { NpcDofus.ReplyId });
        }

        public override void Reply(short replyId)
        {
            Close();
        }
    }

    public class NpcDofusActionScript : NpcAction
    {
        public override NpcActionTypeEnum[] ActionType
        {
            get { return new [] { NpcActionTypeEnum.ACTION_EXCHANGE }; }
        }

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new NpcDofusExchangeDialog(character, npc);
            dialog.Open();
        }
    }

    public class NpcDofusExchangeDialog : NpcTrade
    {
        private readonly List<RequiredItem> REQUIRED_ITEMS = new List<RequiredItem>
            {
                new RequiredItem(972, 50),
                new RequiredItem(20015, 30),
                new RequiredItem(694, 35),
                new RequiredItem(7113, 30),
                new RequiredItem(739, 25),
                new RequiredItem(6980, 15),
                new RequiredItem(20140, 20),
                new RequiredItem(7754, 30)
            };

        public NpcDofusExchangeDialog(Character character, Npc npc)
             : base(character, npc)
        {
        }

        protected override void OnTraderItemMoved(Trader trader, TradeItem item, bool modified, int difference)
        {
            base.OnTraderItemMoved(trader, item, modified, difference);

            if (!(trader is PlayerTrader))
                return;

            AdjustLoots();
        }

        protected override void OnTraderReadyStatusChanged(Trader trader, bool status)
        {
            if (CanExchange())
                base.OnTraderReadyStatusChanged(trader, status);
        }

        private void AdjustLoots()
        {
            ResetRequiredItems();

            foreach (var item in FirstTrader.Items.ToArray())
            {
                var requiredItem = GetRequiredItem(item.Template.Id);
                if (requiredItem != null)
                {
                    requiredItem.Count += item.Stack;
                }
                else
                {
                    FirstTrader.MoveItemToInventory(item, (int) item.Stack);
                }
            }

            var dofusLegendaire = ItemManager.Instance.TryGetTemplate(NpcDofus.RewardItemId);
            var itemDofus = SecondTrader.Items.FirstOrDefault(item => item.Template.Id == dofusLegendaire.Id);

            if (itemDofus == null)
            {
                if (!CanExchange())
                    return;

                SecondTrader.AddItem(dofusLegendaire, 1);
                return;
            }

            SecondTrader.RemoveItem(itemDofus.Template, itemDofus.Stack);
            InventoryHandler.SendExchangeObjectRemovedMessage(FirstTrader.Character.Client, true, itemDofus.Guid);
        }

        private RequiredItem GetRequiredItem(int itemId)
        {
            return REQUIRED_ITEMS.FirstOrDefault(x => x.ItemId == itemId);
        }

        private bool CanExchange()
        {
            return REQUIRED_ITEMS.All(requiredItem => requiredItem.Count >= requiredItem.Amount);
        }

        private void ResetRequiredItems()
        {
            REQUIRED_ITEMS.ForEach(x => x.Count = 0);
        }
    }

    public class RequiredItem
    {
        public RequiredItem(int itemId, int amount)
        {
            ItemId = itemId;
            Amount = amount;
            Count = 0;
        }

        public int ItemId
        {
            get;
            private set;
        }

        public int Amount
        {
            get;
            private set;
        }

        public long Count
        {
            get;
            set;
        }
    }
}
