using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.ELITISTE)]
    public class ElitistChallenge : DefaultChallenge
    {
        public ElitistChallenge(IFight fight)
            : base(fight)
        {
        }

        public ElitistChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 50;

            Target = fight.GetRandomFighter<MonsterFighter>();
            Target.Dead += OnDead;

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
            {
                fighter.BeforeDamageInflicted += OnBeforeDamageInflicted;
            }
        }

        public override bool IsEligible()
        {
            return Fight.GetAllFighters<MonsterFighter>().Count() > 1;
        }

        private void OnDead(FightActor fighter, FightActor killer)
        {
            Target.Dead -= OnDead;
            Target = Fight.GetRandomFighter<MonsterFighter>();

            if (Target != null)
                Target.Dead += OnDead;
        }

        private void OnBeforeDamageInflicted(FightActor fighter, Damage damage)
        {
            if (!(damage.Source is CharacterFighter))
                return;

            if (Target != fighter)
                UpdateStatus(ChallengeStatusEnum.FAILED, damage.Source);
        }
    }
}
