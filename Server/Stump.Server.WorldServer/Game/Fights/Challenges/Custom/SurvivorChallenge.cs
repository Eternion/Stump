using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.SURVIVANT)]
    [ChallengeIdentifier((int)ChallengeEnum.PROTÉGEZ_VOS_MULES)]
    public class SurvivorChallenge : DefaultChallenge
    {
        public SurvivorChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 30;
            BonusMax = 30;
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var fighter in Fight.GetAllFighters<CharacterFighter>())
                fighter.Dead += OnDead;
        }

        public override bool IsEligible() => Fight.GetAllFighters<CharacterFighter>().Count() > 1;

        void OnDead(FightActor fighter, FightActor killer)
        {
            UpdateStatus(ChallengeStatusEnum.FAILED, fighter);
        }

        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            base.OnWinnersDetermined(fight, winners, losers, draw);

            foreach (var fighter in Fight.GetAllFighters<CharacterFighter>())
                fighter.Dead -= OnDead;
        }
    }
}
