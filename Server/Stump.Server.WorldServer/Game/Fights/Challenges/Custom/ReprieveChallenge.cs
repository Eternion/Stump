using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.SURSIS)]
    public class ReprieveChallenge : DefaultChallenge
    {
        public ReprieveChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 20;
        }

        public override void Initialize()
        {
            base.Initialize();

            Target = Fight.GetRandomFighter<MonsterFighter>();

            Target.Dead += OnDead;
        }

        public override bool IsEligible()
        {
            return Fight.GetAllFighters<MonsterFighter>().Count() > 1;
        }

        private void OnDead(FightActor victim, FightActor killer)
        {
            UpdateStatus(!victim.Team.GetAllFighters<MonsterFighter>(x => x.IsAlive()).Any()
                ? ChallengeStatusEnum.SUCCESS
                : ChallengeStatusEnum.FAILED);
        }
    }
}
