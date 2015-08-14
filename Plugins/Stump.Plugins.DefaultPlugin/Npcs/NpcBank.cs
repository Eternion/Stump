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
using Stump.Server.WorldServer.Game.Exchanges.Bank;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;

namespace Stump.Plugins.DefaultPlugin.Npcs
{
    class NpcBank
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static int NpcId = 100;

        [Variable]
        public static int MessageId = 318;

        [Variable]
        public static int InfosMessageId = 410;

        [Variable]
        public static int WrongLevelMessageId = 6205;

        [Variable]
        public static short ReplyInfos = 329;

        [Variable]
        public static short ReplyConsult = 259;

        public static NpcMessage Message;
        public static NpcMessage InfosMessage;
        public static NpcMessage WrongLevelMessage;

        private static bool m_scriptDisabled;

        [Initialization(typeof(NpcManager), Silent = true)]
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
            WrongLevelMessage = NpcManager.Instance.GetNpcMessage(WrongLevelMessageId);
            InfosMessage = NpcManager.Instance.GetNpcMessage(InfosMessageId);

            if (Message != null && WrongLevelMessage != null && InfosMessage != null)
                return;

            Logger.Error("Message {0} not found, script is disabled", MessageId);
            m_scriptDisabled = true;
        }

        private static void OnNpcSpawned(NpcTemplate template, Npc npc)
        {
            if (m_scriptDisabled)
                template.NpcSpawned -= OnNpcSpawned;

            npc.Actions.RemoveAll(x => x.ActionType.Contains(NpcActionTypeEnum.ACTION_TALK));
            npc.Actions.Add(new NpcBankScript());
        }
    }

    public class NpcBankScript : NpcAction
    {
        public override NpcActionTypeEnum[] ActionType
        {
            get { return new []{ NpcActionTypeEnum.ACTION_TALK }; }
        }

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new NpcBankDialog(character, npc);
            dialog.Open();
        }
    }

    public class NpcBankDialog : NpcDialog
    {
         public NpcBankDialog(Character character, Npc npc) : base(character, npc)
         {
             CurrentMessage = character.Level >= 10 ? NpcBank.Message : NpcBank.WrongLevelMessage;
         }

        public override void Open()
        {
            base.Open();

            if (Character.CheckBankIsLoaded(() => Character.Area.AddMessage(OpenCallBack)))
                OpenCallBack();
        }

        private void OpenCallBack()
        {
            ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage, CurrentMessage == NpcBank.Message ? new[] { NpcBank.ReplyInfos, NpcBank.ReplyConsult } : new[] { NpcBank.ReplyInfos }, Character.Bank.GetAccessPrice().ToString());
        }

        public override void Reply(short replyId)
        {
            if (replyId == NpcBank.ReplyConsult)
            {
                var accessPrice = Character.Bank.GetAccessPrice();

                if (Character.Kamas < accessPrice)
                {
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 82);
                }
                else
                {
                    Character.Inventory.SubKamas(accessPrice);

                    var dialog = new BankDialog(Character);
                    dialog.Open(); 
                }

                Close();
            }
            else if (replyId == NpcBank.ReplyInfos)
            {
                CurrentMessage = NpcBank.InfosMessage;
                ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage, new short[0]);
            }
            else
            {
                Close();
            }
        }
    }
}
