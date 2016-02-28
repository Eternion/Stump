using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.NI_PIOUTES_NI_SOUMISES)]
    [ChallengeIdentifier((int)ChallengeEnum.NI_PIOUS_NI_SOUMIS)]
    public class SexChallenge : DefaultChallenge
    {
        readonly SexTypeEnum m_sexType;

        public SexChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 35;
            BonusMax = 35;

            m_sexType = id == (int)ChallengeEnum.NI_PIOUTES_NI_SOUMISES ? SexTypeEnum.SEX_FEMALE : SexTypeEnum.SEX_MALE;
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
                fighter.Dead += OnDead;
        }

        public override bool IsEligible() => Fight.GetAllCharacters().Any(x => x.Sex == SexTypeEnum.SEX_MALE) &&
                   Fight.GetAllCharacters().Any(x => x.Sex == SexTypeEnum.SEX_FEMALE);

        void OnDead(FightActor fighter, FightActor killer)
        {
            if (!(killer is CharacterFighter))
                return;

            if (((CharacterFighter)killer).Character.Sex == m_sexType)
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED);
        }

        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            base.OnWinnersDetermined(fight, winners, losers, draw);

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
                fighter.Dead -= OnDead;
        }
    }
}
