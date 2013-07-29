using System;
using System.Drawing;
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
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;

namespace ArkalysPlugin
{
    public static class NpcRestatScript
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        [Variable] public static double OrbFactor = 5;

        [Variable] public static int NpcId = 0;
        [Variable] public static int MessageId = 0;
        [Variable] public static short ReplyRestatId = 0;
        [Variable] public static short ReplySpellForgetId = 0;
        [Variable] public static short ReplyNoOrbsId = 0;

        public static NpcMessage Message;

        private static ItemTemplate m_orbeTemplate;
        private static bool m_scriptDisabled = false;

        [Initialization(typeof (NpcManager), Silent = true)]
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

        [Initialization(typeof (OrbsManager), Silent = true)]
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
            npc.Actions.Add(new NpcRestatActionScript());
        }
    }

    public class NpcRestatActionScript : NpcAction
    {
        public override NpcActionTypeEnum ActionType
        {
            get { return NpcActionTypeEnum.ACTION_TALK; }
        }

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new NpcRestatDialog(character, npc);
            dialog.Open();
        }
    }

    public class NpcRestatDialog : NpcDialog
    {
        private uint m_requieredOrbs;
        public NpcRestatDialog(Character character, Npc npc)
            : base(character, npc)
        {
            m_requieredOrbs = (uint)Math.Floor(Character.Level*NpcRestatScript.OrbFactor);
            CurrentMessage = NpcRestatScript.Message;
        }

        public override void Open()
        {
            base.Open();

            var orbs = Character.Inventory.TryGetItem(OrbsManager.OrbItemTemplate);

            if (orbs != null && orbs.Stack >= m_requieredOrbs)
                ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage, new [] {NpcRestatScript.ReplyRestatId, NpcRestatScript.ReplySpellForgetId}, m_requieredOrbs.ToString());
            else
                ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage, new[] { NpcRestatScript.ReplyNoOrbsId }, m_requieredOrbs.ToString());
        }

        public override void Reply(short replyId)
        {
            var orbs = Character.Inventory.TryGetItem(OrbsManager.OrbItemTemplate);

            if (replyId == NpcRestatScript.ReplyRestatId)
            {
                if (orbs == null || orbs.Stack < m_requieredOrbs)
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 1);
                else
                {
                    Character.Inventory.RemoveItem(orbs, m_requieredOrbs);
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 22, m_requieredOrbs, orbs.Template.Id);

                    Character.Stats.Agility.Base = Character.PermanentAddedAgility;
                    Character.Stats.Strength.Base = Character.PermanentAddedStrength;
                    Character.Stats.Vitality.Base = Character.PermanentAddedVitality;
                    Character.Stats.Wisdom.Base = Character.PermanentAddedWisdom;
                    Character.Stats.Intelligence.Base = Character.PermanentAddedIntelligence;
                    Character.Stats.Chance.Base = Character.PermanentAddedChance;

                    Character.StatsPoints = (ushort) (Character.Level*5);

                    Character.RefreshStats();
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 15, Character.Level * 5);
                }
            }
            else if (replyId == NpcRestatScript.ReplySpellForgetId)
            {
                if (orbs == null || orbs.Stack < m_requieredOrbs)
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 1);
                else
                {
                    Character.Inventory.RemoveItem(orbs, m_requieredOrbs);
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 22, m_requieredOrbs, orbs.Template.Id);

                    Character.Spells.ForgetAllSpells();
                }
            }
               
            Close();
        }

        public override void ChangeMessage(NpcMessage message)
        {
            
        }
    }
}