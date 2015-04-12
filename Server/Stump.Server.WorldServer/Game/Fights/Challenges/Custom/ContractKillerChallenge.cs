using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.TUEUR_À_GAGES)]
    public class ContractKillerChallenge : DefaultChallenge
    {
        public ContractKillerChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 45;
            BonusMax = 65;
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
            {
                fighter.Dead += OnDead;
            }

            Target = Fight.GetRandomFighter<MonsterFighter>();
        }

        public override bool IsEligible()
        {
            return Fight.GetAllFighters<MonsterFighter>().Count() > 1;
        }

        private void OnDead(FightActor victim, FightActor killer)
        {
            if (victim == Target)
            {
                Target = Fight.GetRandomFighter<MonsterFighter>();
                return;
            }

            UpdateStatus(ChallengeStatusEnum.FAILED, killer);
        }
    }
}
