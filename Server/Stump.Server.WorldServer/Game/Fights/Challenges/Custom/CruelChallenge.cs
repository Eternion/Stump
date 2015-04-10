using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.CRUEL)]
    [ChallengeIdentifier((int)ChallengeEnum.ORDONNÉ)]
    public class CruelChallenge : DefaultChallenge
    {
        private readonly MonsterFighter[] m_monsters;

        public CruelChallenge(IFight fight)
            : base(fight)
        {
        }

        public CruelChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 40;

            m_monsters = fight.GetAllFighters<MonsterFighter>().ToArray();
            foreach (var fighter in m_monsters)
            {
                fighter.Dead += OnDead;
            }

            Target = GetNextTarget();
        }

        public override bool IsEligible()
        {
            return Fight.GetAllFighters<MonsterFighter>().Count() > 1;
        }

        private void OnDead(FightActor victim, FightActor killer)
        {
            if (victim == Target)
            {
                Target = GetNextTarget();
                return;
            }

            UpdateStatus(ChallengeStatusEnum.FAILED, killer);
        }

        private MonsterFighter GetNextTarget()
        {
            var monsters = m_monsters.OrderBy(x => x.Level);
            if (Id == (int)ChallengeEnum.ORDONNÉ)
                monsters = m_monsters.OrderByDescending(x => x.Level);

            return monsters.FirstOrDefault(x => x.IsAlive());
        }
    }
}
