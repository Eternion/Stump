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
using Stump.Server.WorldServer.Game.Dialogs.Guilds;
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

        /*
         * Bonjour jeune aventurier, si tu viens me voir c'est que tu es possédé par un désire incommensurable de créer ta propre guilde. Bien entendue, tu t'engages à diriger un certain nombre de joueur. Il faut que tu gardes en tête que quoi qu'il en soit nous sommes sur un jeu et donc en venant me voir tu as longtemps réfléchis au règles que tu comptes imposer. Garde toujours un esprit bon enfant.
         * Il faut que tu saches que 3 choix s’offrent à toi :
         * 1 - si tu as drop une Guildalogemme tu peux directement cliquer sur « Ouvrir le Panel de création »
         * 2 - Faire une collecte groupé afin d’acheter une Guildalogemme disponible à 50.000 Orbes.
         * 3 - Pour les personnes qui ne sont aucunement patient, un achat via des jetons est possible. Bien sur le prix est élevé pour vous obliger à choisir en 1er lieu les 2 premières possibilités.
        */
        [Variable]
        public static int MessageId = 20009;

        //Guildalogemme
        [Variable]
        public static int RequiredItemId = (int)ItemIdEnum.Guildalogem;

        internal static ItemTemplate RequieredItem;

        //Ouvrir le "Panel de création".
        [Variable]
        public static short ReplyGuildSuccessId = 20005;

        //Voulez-vous acheter une Guildalogemme pour XX orbes?
        [Variable] public static short ReplyGuildBuyId = 20010;

        //Arf, une erreur inattendu t'as poussé à annulé la création de ta guilde. Je suppose que tu n'as pas encore l'étoffe d'un héros... Enfin je veux dire par la que tu préfères te faire diriger que de diriger les autres. C’est un choix et je l'accepte à bientôt si tu changes d'avis.
        [Variable]
        public static short ReplyGuildFailId = 20007;

        //Ohhh non.... toi tu vas devoir perdre 30 secondes de ton temps afin de quitter ton ancienne guilde et venir me reparler afin de pouvoir enfin créer ta propre guilde.
        [Variable]
        public static short ReplyAlreadyHaveGuild = 20008;

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
            RequieredItem = ItemManager.Instance.TryGetTemplate(RequiredItemId);

            if (Message != null && RequieredItem != null)
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
            {
                template.NpcSpawned -= OnNpcSpawned;
                return;
            }

            npc.Actions.RemoveAll(x => x.ActionType.Contains(NpcActionTypeEnum.ACTION_TALK));
            npc.Actions.Add(new NpcGuildsScript());
        }
    }

    public class NpcGuildsScript : NpcAction
    {
        public override NpcActionTypeEnum[] ActionType
        {
            get { return new [] { NpcActionTypeEnum.ACTION_TALK }; }
        }

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new NpcGuildsDialog(character, npc);
            dialog.Open();
        }
    }

    public class NpcGuildsDialog : NpcDialog
    {
        private readonly int m_requieredOrbs;

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
            var guildalogemme = Character.Inventory.TryGetItem(NpcGuilds.RequieredItem);
            if (replyId == NpcGuilds.ReplyGuildSuccessId && guildalogemme != null)
            {
                var panel = new GuildCreationPanel(Character);
                panel.Open();
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

                    Character.Inventory.AddItem(NpcGuilds.RequieredItem);
                }
            }

            Close();
        }
    }
}
