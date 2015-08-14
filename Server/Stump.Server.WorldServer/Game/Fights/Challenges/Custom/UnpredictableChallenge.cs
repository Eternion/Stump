using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.IMPRÉVISIBLE)]
    public class UnpredictableChallenge : DefaultChallenge
    {
        public UnpredictableChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 50;
            BonusMax = 70;
        }

        public override void Initialize()
        {
            base.Initialize();

            Fight.TurnStarted += OnTurnStarted;

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
            {
                fighter.BeforeDamageInflicted += OnBeforeDamageInflicted;
                fighter.Dead += OnDead;
            }
        }

        private void OnDead(FightActor fighter, FightActor killer)
        {
            if (Target == fighter)
                Target = Fight.GetRandomFighter<MonsterFighter>();
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

        private void OnBeforeDamageInflicted(FightActor fighter, Damage damage)
        {
            if (!(damage.Source is CharacterFighter))
                return;

            if (Target != fighter)
                UpdateStatus(ChallengeStatusEnum.FAILED, damage.Source);
        }
    }
}
