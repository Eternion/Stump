using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.BLITZKRIEG)]
    public class BlitzkriegChallenge : DefaultChallenge
    {
        public BlitzkriegChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 80;
            BonusMax = 125;
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
                fighter.BeforeDamageInflicted += OnBeforeDamageInflicted;

            Fight.TurnStarted += OnTurnStarted;
        }

        public override bool IsEligible() => Fight.GetAllFighters<MonsterFighter>().Count() > 1;

        void OnBeforeDamageInflicted(FightActor fighter, Damage damage)
        {
            if (fighter.IsFriendlyWith(damage.Source))
                return;

            Target = fighter;
        }

        void OnTurnStarted(IFight fight, FightActor fighter)
        {
            if (fighter == Target)
                UpdateStatus(ChallengeStatusEnum.FAILED);
        }

        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            base.OnWinnersDetermined(fight, winners, losers, draw);

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
                fighter.BeforeDamageInflicted -= OnBeforeDamageInflicted;

            Fight.TurnStarted -= OnTurnStarted;
        }
    }
}
