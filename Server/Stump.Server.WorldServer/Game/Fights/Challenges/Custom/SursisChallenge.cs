using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier(4)]
    public class SursisChallenge : DefaultChallenge
    {
        public SursisChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 20;

            Target = fight.GetRandomFighter<MonsterFighter>();

            Target.Dead += OnDead;

            TargetId = Target.Id;
        }

        private void OnDead(FightActor victim, FightActor killer)
        {
            UpdateStatus(!victim.Team.GetAllFighters<MonsterFighter>(x => x.IsAlive()).Any()
                ? ChallengeStatusEnum.SUCCESS
                : ChallengeStatusEnum.FAILED);
        }

        public MonsterFighter Target
        {
            get;
            private set;
        }
    }
}
