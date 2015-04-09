using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.ELITISTE)]
    public class ElitisteChallenge : DefaultChallenge
    {
        public ElitisteChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 50;

            Target = fight.GetRandomFighter<MonsterFighter>();
            Target.Dead += OnDead;

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
            {
                fighter.DamageInflicted += OnDamageInflicted;
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
            Target.Dead += OnDead;
        }

        private void OnDamageInflicted(FightActor fighter, Damage damage)
        {
            if (!(damage.Source is CharacterFighter))
                return;

            if (Target != fighter)
                UpdateStatus(ChallengeStatusEnum.FAILED, damage.Source);
        }
    }
}
