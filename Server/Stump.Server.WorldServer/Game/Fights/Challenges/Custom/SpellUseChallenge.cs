using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.JARDINIER)]
    [ChallengeIdentifier((int)ChallengeEnum.FOSSOYEUR)]
    [ChallengeIdentifier((int)ChallengeEnum.CASINO_ROYAL)]
    [ChallengeIdentifier((int)ChallengeEnum.ARAKNOPHILE)]
    public class SpellUseChallenge : DefaultChallenge
    {
        private readonly int m_spell;

        public SpellUseChallenge(int id, IFight fight)
            : base(id, fight)
        {
            if (id == (int)ChallengeEnum.CASINO_ROYAL)
            {
                BonusMin = 30;
                BonusMax = 30;
            }
            else
            {
                BonusMin = 10;
                BonusMax = 20;
            }

            switch ((ChallengeEnum)Id)
            {
                case ChallengeEnum.JARDINIER:
                    m_spell = (int)SpellIdEnum.CAWOTTE;
                    break;
                case ChallengeEnum.FOSSOYEUR:
                    m_spell = (int)SpellIdEnum.INVOCATION_DE_CHAFERFU;
                    break;
                case ChallengeEnum.CASINO_ROYAL:
                    m_spell = (int)SpellIdEnum.ROULETTE;
                    break;
                case ChallengeEnum.ARAKNOPHILE:
                    m_spell = (int)SpellIdEnum.INVOCATION_D_ARAKNE_370;
                    break;
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            Fight.TurnStopped += OnTurnStopped;
        }

        public override bool IsEligible()
        {
            return Fight.GetAllFighters<CharacterFighter>().Any(x => x.HasSpell(m_spell));
        }

        private void OnTurnStopped(IFight fight, FightActor fighter)
        {
            if (!(fighter is CharacterFighter))
                return;

            var spell = fighter.GetSpell(m_spell);

            if (spell == null)
                return;

            if (fighter.SpellHistory.GetEntries(x => x.CastRound == fight.TimeLine.RoundNumber && x.Spell.SpellId == m_spell).Any())
                return;

            if (fighter.SpellHistory.CanCastSpell(spell.CurrentSpellLevel))
                UpdateStatus(ChallengeStatusEnum.FAILED, fighter);
        }


        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            OnTurnStopped(fight, fight.FighterPlaying);

            base.OnWinnersDetermined(fight, winners, losers, draw);
        }
    }
}
