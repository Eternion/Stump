using System;
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
using Stump.Server.WorldServer.Game.Dialogs.Spells;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;

namespace ArkalysPlugin.Npcs
{
    public static class NpcRestatScript
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        [Variable] public static double OrbFactor = 5;

        [Variable] public static int NpcId = 3000;
        //Approche jeune aventurier, je sais que tu as fait de mauvais choix, et je peux t’aider à tout oublier … contre suffisamment d’orbes bien entendu. 
        //Donnes moi #1 orbes et tu auras une autre chance de choisir ta voie 
        [Variable] public static int MessageId = 20003;
        [Variable]
        public static short ReplyRestatId = 20010;
        [Variable]
        public static short ReplySpellForgetId = 20011;
        [Variable] 
        public static short ReplySpellForgetPanelId = 20033;
        [Variable]
        public static short ReplyNoOrbsId = 20012;

        public static NpcMessage Message;

        private static bool m_scriptDisabled;

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

            if (Message != null)
                return;

            logger.Error("Message {0} not found, script is disabled", MessageId);
            m_scriptDisabled = true;
        }

        [Initialization(typeof (OrbsManager), Silent = true)]
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

            npc.Actions.RemoveAll(x => x.ActionType.Contains(NpcActionTypeEnum.ACTION_TALK));
            npc.Actions.Add(new NpcRestatActionScript());
        }
    }

    public class NpcRestatActionScript : NpcAction
    {
        public override NpcActionTypeEnum[] ActionType
        {
            get { return new [] { NpcActionTypeEnum.ACTION_TALK }; }
        }

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new NpcRestatDialog(character, npc);
            dialog.Open();
        }
    }

    public class NpcRestatDialog : NpcDialog
    {
        private readonly int m_requieredOrbs;
        public NpcRestatDialog(Character character, Npc npc)
            : base(character, npc)
        {
            m_requieredOrbs = (int)Math.Floor(Character.Level*NpcRestatScript.OrbFactor);
            CurrentMessage = NpcRestatScript.Message;
        }

        public override void Open()
        {
            base.Open();

            var orbs = Character.Inventory.TryGetItem(OrbsManager.OrbItemTemplate);

            if (orbs != null && orbs.Stack >= m_requieredOrbs)
                ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage, new[] { NpcRestatScript.ReplyRestatId, NpcRestatScript.ReplySpellForgetId, NpcRestatScript.ReplySpellForgetPanelId }, m_requieredOrbs.ToString());
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

                    Character.ResetStats();
                }
            }
            else if (replyId == NpcRestatScript.ReplySpellForgetId)
            {
                if (orbs == null || orbs.Stack < m_requieredOrbs)
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 1);
                else
                {
                    Character.Inventory.RemoveItem(orbs, m_requieredOrbs);

                    var points = (Character.Spells.CountSpentBoostPoint() + Character.SpellsPoints);

                    Character.Spells.ForgetAllSpells();
                    Character.SpellsPoints = (ushort)(points >= 0 ? points : 0);
                    Character.RefreshStats();

                    Character.SaveLater();
                }
            }
            else if (replyId == NpcRestatScript.ReplySpellForgetPanelId)
            {
                if (orbs == null || orbs.Stack < m_requieredOrbs)
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 1);
                else
                {
                    Character.Inventory.RemoveItem(orbs, m_requieredOrbs);

                    var panel = new SpellForgetPanel(Character);
                    panel.Open();
                }
            }
               
            Close();
        }
    }
}