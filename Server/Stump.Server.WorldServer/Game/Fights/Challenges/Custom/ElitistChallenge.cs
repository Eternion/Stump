using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.ELITISTE)]
    public class ElitistChallenge : DefaultChallenge
    {
        public ElitistChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 50;
            BonusMax = 50;
        }

        public override void Initialize()
        {
            base.Initialize();

            Target = Fight.GetRandomFighter<MonsterFighter>();
            Target.Dead += OnDead;

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
                fighter.BeforeDamageInflicted += OnBeforeDamageInflicted;
        }

        public override bool IsEligible() => Fight.GetAllFighters<MonsterFighter>().Count() > 1;

        void OnDead(FightActor fighter, FightActor killer)
        {
            Target.Dead -= OnDead;
            Target.BeforeDamageInflicted -= OnBeforeDamageInflicted;

            UpdateStatus(ChallengeStatusEnum.SUCCESS);
        }

        void OnBeforeDamageInflicted(FightActor fighter, Damage damage)
        {
            if (!(damage.Source is CharacterFighter))
                return;

            if (Target != fighter)
                UpdateStatus(ChallengeStatusEnum.FAILED, damage.Source);
        }

        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            base.OnWinnersDetermined(fight, winners, losers, draw);

            Target.Dead -= OnDead;

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
                fighter.BeforeDamageInflicted -= OnBeforeDamageInflicted;
        }
    }
}
