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
        public static int MessageId = 20003;
        //Je créer ma guilde !
        [Variable]
        public static short ReplyGuildSuccessId = 20010;
        //Vous n'avez pas le niveau requis pour créer une guilde(Niveau 200)
        [Variable]
        public static short ReplyGuildFailId = 20010;
        //Vous possédez déjà une guilde. Quittez la ou passez votre chemin
        [Variable]
        public static short ReplyAlreadyHaveGuild = 20010;

        public static NpcMessage Message;
        private static bool _scriptDisabled;

        [Initialization(typeof (NpcManager), Silent = true)]
        public static void Initialize()
        {
            if (_scriptDisabled)
                return;

            var npc = NpcManager.Instance.GetNpcTemplate(NpcId);

            if (npc == null)
            {
                Logger.Error("Npc {0} not found, script is disabled", NpcId);
                _scriptDisabled = true;
                return;
            }

            npc.NpcSpawned += OnNpcSpawned;

            //
            Message = NpcManager.Instance.GetNpcMessage(MessageId);

            if (Message != null)
                return;

            Logger.Error("Message {0} not found, script is disabled", MessageId);
            _scriptDisabled = true;
        }

        private static void OnNpcSpawned(NpcTemplate template, Npc npc)
        {
            if (_scriptDisabled)
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
            var dialog = new NpcRestatDialog(character, npc);
            dialog.Open();
        }
    }

    public class NpcGuildsDialog : NpcDialog
    {
        public NpcGuildsDialog(Character character, Npc npc) : base(character, npc)
        {
            CurrentMessage = NpcGuilds.Message;
        }

        public override void Open()
        {
            base.Open();

            if (Character.Guild != null)
                ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage, new[] {NpcGuilds.ReplyAlreadyHaveGuild});
            else
            {
                ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage,
                                                                    Character.Level == 200
                                                                        ? new[] { NpcGuilds.ReplyGuildSuccessId }
                                                                        : new[] { NpcGuilds.ReplyGuildFailId });  
            }
        }

        public override void Reply(short replyId)
        {
            if (replyId == NpcGuilds.ReplyGuildSuccessId)
            {
                Character.Client.Send(new GuildCreationStartedMessage());
            }

            Close();
        }
    }
}
