using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.CRUEL)]
    public class CruelChallenge : DefaultChallenge
    {
        private readonly MonsterFighter[] m_monsters;

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
            return m_monsters.OrderByDescending(x => x.Level).FirstOrDefault(x => x.IsAlive());
        }
    }
}
