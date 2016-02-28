using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.CRUEL)]
    [ChallengeIdentifier((int)ChallengeEnum.ORDONNÉ)]
    public class CruelChallenge : DefaultChallenge
    {
        private MonsterFighter[] m_monsters;

        public CruelChallenge(int id, IFight fight)
            : base(id, fight)
        {
            if (id == (int)ChallengeEnum.CRUEL)
            {
                BonusMin = 40;
                BonusMax = 80;
            }
            else
            {
                BonusMin = 40;
                BonusMax = 60;
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            m_monsters = Fight.GetAllFighters<MonsterFighter>().ToArray();
            foreach (var fighter in m_monsters)
                fighter.Dead += OnDead;

            Target = GetNextTarget();
        }

        public override bool IsEligible() => Fight.GetAllFighters<MonsterFighter>().Count() > 1;

        void OnDead(FightActor victim, FightActor killer)
        {
            if (Target == null)
                return;

            if (victim == Target || victim.Level == Target.Level)
            {
                Target = GetNextTarget();
                return;
            }

            UpdateStatus(ChallengeStatusEnum.FAILED, killer);
        }

        MonsterFighter GetNextTarget()
        {
            var monsters = m_monsters.OrderBy(x => x.Level);
            if (Id == (int)ChallengeEnum.ORDONNÉ)
                monsters = m_monsters.OrderByDescending(x => x.Level);

            return monsters.FirstOrDefault(x => x.IsAlive());
        }

        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            base.OnWinnersDetermined(fight, winners, losers, draw);

            foreach (var fighter in m_monsters)
                fighter.Dead -= OnDead;
        }
    }
}
