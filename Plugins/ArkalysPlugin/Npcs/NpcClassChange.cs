using System.Drawing;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Npcs;
using Stump.Server.WorldServer.Database.Npcs.Actions;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Breeds;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;

namespace ArkalysPlugin.Npcs
{
    class NpcClassChange
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static int NpcId = 3013;

        //Oyé oyé nobles aventuriers,
        //Vous voulez assumer votre véritable personnalité et changez votre façon de jouer en changeant de classe et en conservant la même puissance ?
        //Etant en étroite relation avec les divinité je vous offre le choix, à quelle divinité voulez-vous être affilié ?
        [Variable]
        public static int MessageId = 20112;

        //Je souhaite devenir Féca !
        public static short ReplyFeca = 20113;

        //Je souhaite devenir Osamodas !
        public static short ReplyOsamodas = 20114;

        //Je souhaite devenir Enutrof !
        public static short ReplyEnutrof = 20115;

        //Je souhaite devenir Sram !
        public static short ReplySram = 20116;

        //Je souhaite devenir Xélor !
        public static short ReplyXelor = 20117;

        //Je souhaite devenir Ecaflip !
        public static short ReplyEcaflip = 20118;

        //Je souhaite devenir Eniripsa !
        public static short ReplyEniripsa = 20119;

        //Je souhaite devenir Iop !
        public static short ReplyIop = 20120;

        //Je souhaite devenir Crâ !
        public static short ReplyCra = 20121;

        //Je souhaite devenir Sadida !
        public static short ReplySadida = 20122;

        //Je souhaite devenir Sacrieur !
        public static short ReplySacrieur = 20123;

        //Je souhaite devenir Pandawa !
        public static short ReplyPandawa = 20124;

        //Je souhaite devenir Roublard !
        public static short ReplyRoublard = 20125;

        //Je souhaite devenir Zobal !
        public static short ReplyZobal = 20126;

        //Je souhaite devenir Steamer !
        public static short ReplySteamer = 20127;

        //Je n'ai pas assez de Jetons pour le moment, je repasserais plus tard !
        public static short ReplyNoTokens = 20128;

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

        private static void OnNpcSpawned(NpcTemplate template, Npc npc)
        {
            if (m_scriptDisabled)
                template.NpcSpawned -= OnNpcSpawned;

            npc.Actions.RemoveAll(x => x.ActionType.Contains(NpcActionTypeEnum.ACTION_TALK));
            npc.Actions.Add(new NpcClassChangeScript());
        }
    }

    public class NpcClassChangeScript : NpcAction
    {
        public override NpcActionTypeEnum[] ActionType
        {
            get { return new [] { NpcActionTypeEnum.ACTION_TALK }; }
        }

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new NpcClassChangeDialog(character, npc);
            dialog.Open();
        }
    }

    public class NpcClassChangeDialog : NpcDialog
    {
        private int m_price;

        public NpcClassChangeDialog(Character character, Npc npc)
            : base(character, npc)
        {
            CurrentMessage = NpcClassChange.Message;
        }

        public override void Open()
        {
            base.Open();

            m_price = 700 + (40 * Character.PrestigeRank);

            if (Character.Inventory.Tokens == null || Character.Inventory.Tokens.Stack < m_price)
            {
                ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage, new[] { NpcClassChange.ReplyNoTokens }, m_price.ToString());
                return;
            }

            ContextRoleplayHandler.SendNpcDialogQuestionMessage(Character.Client, CurrentMessage, new[] {
                NpcClassChange.ReplyFeca,
                NpcClassChange.ReplyOsamodas,
                NpcClassChange.ReplyEnutrof,
                NpcClassChange.ReplySram,
                NpcClassChange.ReplyXelor,
                NpcClassChange.ReplyEcaflip,
                NpcClassChange.ReplyEniripsa,
                NpcClassChange.ReplyIop,
                NpcClassChange.ReplyCra,
                NpcClassChange.ReplySadida,
                NpcClassChange.ReplySacrieur,
                NpcClassChange.ReplyPandawa,
                NpcClassChange.ReplyRoublard,
                NpcClassChange.ReplyZobal,
                NpcClassChange.ReplySteamer
            }, m_price.ToString());
        }

        public override void Reply(short replyId)
        {
            var bread = PlayableBreedEnum.UNDEFINED;

            if (replyId == NpcClassChange.ReplyFeca)
                bread = PlayableBreedEnum.Feca;
            else if (replyId == NpcClassChange.ReplyOsamodas)
                bread = PlayableBreedEnum.Osamodas;
            else if (replyId == NpcClassChange.ReplyEnutrof)
                bread = PlayableBreedEnum.Enutrof;
            else if (replyId == NpcClassChange.ReplySram)
                bread = PlayableBreedEnum.Sram;
            else if (replyId == NpcClassChange.ReplyXelor)
                bread = PlayableBreedEnum.Xelor;
            else if (replyId == NpcClassChange.ReplyEcaflip)
                bread = PlayableBreedEnum.Ecaflip;
            else if (replyId == NpcClassChange.ReplyEniripsa)
                bread = PlayableBreedEnum.Eniripsa;
            else if (replyId == NpcClassChange.ReplyIop)
                bread = PlayableBreedEnum.Iop;
            else if (replyId == NpcClassChange.ReplyCra)
                bread = PlayableBreedEnum.Cra;
            else if (replyId == NpcClassChange.ReplySadida)
                bread = PlayableBreedEnum.Sadida;
            else if (replyId == NpcClassChange.ReplySacrieur)
                bread = PlayableBreedEnum.Sacrieur;
            else if (replyId == NpcClassChange.ReplyPandawa)
                bread = PlayableBreedEnum.Pandawa;
            else if (replyId == NpcClassChange.ReplyRoublard)
                bread = PlayableBreedEnum.Roublard;
            else if (replyId == NpcClassChange.ReplyZobal)
                bread = PlayableBreedEnum.Zobal;
            else if (replyId == NpcClassChange.ReplySteamer)
                bread = PlayableBreedEnum.Steamer;

            if (bread == PlayableBreedEnum.UNDEFINED)
            {
                Close();
                return;
            }

            Character.Inventory.UnStackItem(Character.Inventory.Tokens, m_price);
            ChangeBreed(bread);

            Close();
        }

        private static SpellLevelTemplate GetSpecialSpell(PlayableBreedEnum breedId)
        {
            var spellId = -1;

            switch (breedId)
            {
                case PlayableBreedEnum.Feca:
                    spellId = 2108;
                    break;
                case PlayableBreedEnum.Osamodas:
                    spellId = 2098;
                    break;
                case PlayableBreedEnum.Enutrof:
                    spellId = 2123;
                    break;
                case PlayableBreedEnum.Sram:
                    spellId = 2078;
                    break;
                case PlayableBreedEnum.Xelor:
                    spellId = 2118;
                    break;
                case PlayableBreedEnum.Ecaflip:
                    spellId = 2058;
                    break;
                case PlayableBreedEnum.Eniripsa:
                    spellId = 2133;
                    break;
                case PlayableBreedEnum.Iop:
                    spellId = 2048;
                    break;
                case PlayableBreedEnum.Cra:
                    spellId = 2088;
                    break;
                case PlayableBreedEnum.Sadida:
                    spellId = 2128;
                    break;
                case PlayableBreedEnum.Sacrieur:
                    spellId = 2103;
                    break;
                case PlayableBreedEnum.Pandawa:
                    spellId = 2113;
                    break;
                case PlayableBreedEnum.Roublard:
                    spellId = 2148;
                    break;
                case PlayableBreedEnum.Zobal:
                    spellId = 18596;
                    break;
                case PlayableBreedEnum.Steamer:
                    spellId = 20109;
                    break;
            }

            return SpellManager.Instance.GetSpellLevel(spellId);
        }

        private void ChangeBreed(PlayableBreedEnum breed)
        {
            Character.Spells.ForgetAllSpells();
            Character.ResetStats();

            var specialSpell = GetSpecialSpell(Character.BreedId);

            foreach (var breedSpell in Character.Breed.Spells)
            {
                Character.Spells.UnLearnSpell(breedSpell.Spell);
            }

            Character.SetBreed(breed);

            foreach (var breedSpell in Character.Breed.Spells.Where(breedSpell => breedSpell.ObtainLevel <= Character.Level))
            {
                Character.Spells.LearnSpell(breedSpell.Spell);
            }

            if (Character.Spells.HasSpell((int)specialSpell.SpellId))
            {
                Character.Spells.UnLearnSpell((int)specialSpell.SpellId);

                specialSpell = GetSpecialSpell(Character.BreedId);

                Character.Spells.LearnSpell((int)specialSpell.SpellId);
            }

            Character.Record.Relook = 1;

            Character.RealLook = Character.Breed.GetLook(Character.Sex, true);
            Character.Head = BreedManager.Instance.GetHead(x => x.Breed == Character.Breed.Id && x.Gender == (int)Character.Sex);
            Character.RealLook.SetColors(Character.Breed.GetColors(Character.Sex).Select(x => Color.FromArgb((int)x)).ToArray());

            foreach (var skin in Character.Head.Skins)
                Character.RealLook.AddSkin(skin);

            Character.SendSystemMessage(50, true, "Pour finaliser le changement de classe reconnectez vous sur", Character.Name);
            Character.Client.Disconnect();
        }
    }
}
