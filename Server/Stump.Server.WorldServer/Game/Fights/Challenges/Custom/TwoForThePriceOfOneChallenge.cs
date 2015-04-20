using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.DEUX_POUR_LE_PRIX_D_UN)]
    public class TwoForThePriceOfOneChallenge : DefaultChallenge
    {
        private int m_kills;

        public TwoForThePriceOfOneChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 60;
            BonusMax = 60;

            m_kills = 0;
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
            {
                fighter.Dead += OnDead;
            }

            Fight.BeforeTurnStopped += OnBeforeTurnStopped;
        }

        public override bool IsEligible()
        {
            return Fight.GetAllFighters<MonsterFighter>().Count() % 2 == 0;
        }

        private void OnBeforeTurnStopped(IFight fight, FightActor fighter)
        {
            if (fighter is SummonedFighter && m_kills > 1)
                UpdateStatus(ChallengeStatusEnum.FAILED, fighter);

            if (!(fighter is CharacterFighter))
            {
                m_kills = 0;
                return;
            }

            if (m_kills == 0)
                return;

            if (m_kills != 2)
                UpdateStatus(ChallengeStatusEnum.FAILED, fighter);

            m_kills = 0;
        }

        private void OnDead(FightActor fighter, FightActor killer)
        {
            if (fighter.IsFriendlyWith(killer))
                return;

            m_kills++;
        }
    }
}
