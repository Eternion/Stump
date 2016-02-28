using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.LES_PETITS_D_ABORD)]
    public class LowLevelChallenge : DefaultChallenge
    {
        public LowLevelChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 40;
            BonusMax = 40;
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
                fighter.Dead += OnDead;
        }

        public override bool IsEligible() => Fight.GetAllCharacters().Count() > 1;

        void OnDead(FightActor fighter, FightActor killer)
        {
            fighter.Dead -= OnDead;

            if (!(killer is CharacterFighter))
                return;

            if (killer.Team.Fighters.OrderBy(x => x.Level).FirstOrDefault() == killer)
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
