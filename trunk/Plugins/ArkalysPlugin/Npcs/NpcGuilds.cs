using System;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Npcs.Actions;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;

namespace ArkalysPlugin.Npcs
{
    public static class NpcGuilds
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static int NpcId = 3001;
        //Tu as l'âme d'un meneur? Tu pense pouvoir mener des guerriers dans la bataille? Alors créer une guilde !
        [Variable]
        public static int MessageId = 20007;

        [Variable]
        public static int RequiredItemId = 1575;

        //Créer ma guilde et perdre une guildalogemme
        [Variable]
        public static short ReplyGuildSuccessId = 20010;
        //Voulez-vous acheter une Guildalogemme pour XX orbes?
        [Variable] public static short ReplyGuildBuyId = 20010;
        //Vous n'avez pas le niveau requis pour créer une guilde(Niveau 200)
        [Variable]
        public static short ReplyGuildFailId = 20010;
        //Vous possédez déjà une guilde. Quittez la ou passez votre chemin
        [Variable]
        public static short ReplyAlreadyHaveGuild = 20010;

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
                Logger.Error("Npc {0} not found, script is disabled", NpcId);
                m_scriptDisabled = true;
                return;
            }

            npc.NpcSpawned += OnNpcSpawned;

            Message = NpcManager.Instance.GetNpcMessage(MessageId);

            if (Message != null)
                return;

            Logger.Error("Message {0} not found, script is disabled", MessageId);
            m_scriptDisabled = true;
        }

        [Initialization(typeof(OrbsManager), Silent = true)]
        public static void InitializeItem()
        {
            if (OrbsManager.OrbItemTemplate != null)
                return;

            Logger.Error("No orb item, script is disabled");
            m_scriptDisabled = true;
        }

        private static void OnNpcSpawned(NpcTemplate template, Npc npc)
        {
            if (m_scriptDisabled)
                template.NpcSpawned -= OnNpcSpawned;

            npc.Actions.RemoveAll(x => x.ActionType == NpcActionTypeEnum.ACTION_TALK);
            npc.Actions.Add(new NpcGuildsScript());
        }
    }

    public class NpcGuildsScript : NpcAction
    {
        public override NpcActionTypeEnum ActionType
        {
            get { return NpcActionTypeEnum.ACTION_TALK; }
        }

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new NpcGuildsDialog(character, npc);
            dialog.Open();
        }
    }

    public class NpcGuildsDialog : NpcDialog
    {
        private readonly uint m_requieredOrbs;

        public NpcGuildsDialog(Character character, Npc npc) : base(character, npc)
        {
            m_requieredOrbs = 30000;
            CurrentMessage = NpcGuilds.Message;
        }

        public override void Open()
        {
            base.Open();

            if (Character.Guild != null)
                ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage, new[] { NpcGuilds.ReplyAlreadyHaveGuild });
            else
            {
                var guildalogemme = Character.Inventory.TryGetItem(ItemManager.Instance.TryGetTemplate(NpcGuilds.RequiredItemId));
                ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage,
                                                                    guildalogemme != null
                                                                        ? new[] {NpcGuilds.ReplyGuildSuccessId}
                                                                        : new[] { NpcGuilds.ReplyGuildBuyId }, m_requieredOrbs.ToString());
            }
        }

        public override void Reply(short replyId)
        {
            if (replyId == NpcGuilds.ReplyGuildSuccessId)
            {
                Character.Client.Send(new GuildCreationStartedMessage());
            }
            else if (replyId == NpcGuilds.ReplyGuildBuyId)
            {
                var orbs = Character.Inventory.TryGetItem(OrbsManager.OrbItemTemplate);

                if (orbs == null || orbs.Stack <= m_requieredOrbs)
                {
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 1);
                }
                else
                {
                    Character.Inventory.RemoveItem(orbs, m_requieredOrbs);
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 22, m_requieredOrbs, orbs.Template.Id);

                    var guildalogemme = Character.Inventory.TryGetItem(ItemManager.Instance.TryGetTemplate(NpcGuilds.RequiredItemId));
                    Character.Inventory.AddItem(guildalogemme);
                }
            }

            Close();
        }
    }
}
