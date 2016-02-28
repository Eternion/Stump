using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.LES_MULES_D_ABORD)]
    public class MulesChallenge : DefaultChallenge
    {
        public MulesChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 30;
            BonusMax = 30;
        }

        public override void Initialize()
        {
            base.Initialize();

            Target = Fight.GetAllFighters<CharacterFighter>().OrderBy(x => x.Level).FirstOrDefault();

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
                fighter.Dead += OnDead;
        }

        public override bool IsEligible() => (Fight.GetAllFighters<CharacterFighter>().Select(x => x.Level).Max() -
                   Fight.GetAllFighters<CharacterFighter>().Select(x => x.Level).Min()) > 50;

        void OnDead(FightActor victim, FightActor killer)
        {
            if (killer == Target)
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED, killer);
        }

        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            base.OnWinnersDetermined(fight, winners, losers, draw);

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
                fighter.Dead -= OnDead;
        }
    }
}
