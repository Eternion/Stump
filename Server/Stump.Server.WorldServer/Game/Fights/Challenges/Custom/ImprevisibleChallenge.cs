using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.IMPRÉVISIBLE)]
    public class ImprevisibleChallenge : DefaultChallenge
    {
        public ImprevisibleChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 50;

            Fight.TurnStarted += OnTurnStarted;

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
            {
                fighter.DamageInflicted += OnDamageInflicted;
            }
        }

        private void OnTurnStarted(IFight fight, FightActor fighter)
        {
            if (fighter is CharacterFighter)
                Target = Fight.GetRandomFighter<MonsterFighter>();
        }

        public override bool IsEligible()
        {
            return Fight.GetAllFighters<MonsterFighter>().Count() > 1;
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
