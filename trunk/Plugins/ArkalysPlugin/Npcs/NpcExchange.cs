using System;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Npcs.Actions;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;

namespace ArkalysPlugin.Npcs
{
    public static class NpcExchange
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static int NpcId = 1419;
        [Variable]
        public static int MessageId = 20004;
        [Variable]
        public static short ReplyExchangeOrbsId = 20005;
        [Variable]
        public static short ReplyNoOrbsId = 20006;

        public static NpcMessage Message;

        private static ItemTemplate m_orbeTemplate;
        private static bool m_scriptDisabled = false;

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

            if (Message == null)
            {
                logger.Error("Message {0} not found, script is disabled", MessageId);
                m_scriptDisabled = true;
            }
        }

        [Initialization(typeof(OrbsManager), Silent = true)]
        public static void InitializeItem()
        {
            if (OrbsManager.OrbItemTemplate == null)
            {
                logger.Error("No orb item, script is disabled");
                m_scriptDisabled = true;
            }
        }

        private static void OnNpcSpawned(NpcTemplate template, Npc npc)
        {
            if (m_scriptDisabled)
                template.NpcSpawned -= OnNpcSpawned;

            npc.Actions.RemoveAll(x => x.ActionType == NpcActionTypeEnum.ACTION_TALK);
            npc.Actions.Add(new NpcExchangeActionScript());
        }
    }

    public class NpcExchangeActionScript : NpcAction
    {
        public override NpcActionTypeEnum ActionType
        {
            get { return NpcActionTypeEnum.ACTION_TALK; }
        }

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new NpcExchangeDialog(character, npc);
            dialog.Open();
        }
    }

    public class NpcExchangeDialog : NpcDialog
    {
        private uint m_requieredOrbs;
        private int m_exchangeKamas;

        public NpcExchangeDialog(Character character, Npc npc)
            : base(character, npc)
        {
            m_requieredOrbs = 10;
            m_exchangeKamas = 10000;
            CurrentMessage = NpcExchange.Message;
        }

        public override void Open()
        {
            base.Open();

            var orbs = Character.Inventory.TryGetItem(OrbsManager.OrbItemTemplate);

            if (orbs != null && orbs.Stack >= m_requieredOrbs)
                ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage, new[] { NpcExchange.ReplyExchangeOrbsId }, m_requieredOrbs.ToString());
            else
                ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage, new[] { NpcExchange.ReplyNoOrbsId }, m_requieredOrbs.ToString());
        }

        public override void Reply(short replyId)
        {
            var orbs = Character.Inventory.TryGetItem(OrbsManager.OrbItemTemplate);

            if (replyId == NpcExchange.ReplyExchangeOrbsId)
            {
                if (orbs == null || orbs.Stack < m_requieredOrbs)
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 1);
                else
                {
                    Character.Inventory.RemoveItem(orbs, m_requieredOrbs);
                    Character.Inventory.AddKamas(m_exchangeKamas);
                }
            }

            Close();
        }
    }
}
