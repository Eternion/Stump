using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.CONTAMINATION)]
    public class ContaminationChallenge : DefaultChallenge
    {
        readonly Dictionary<FightActor, int> m_contaminedFighters = new Dictionary<FightActor, int>();

        public ContaminationChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 60;
            BonusMax = 60;
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var fighter in Fight.GetAllFighters<CharacterFighter>())
            {
                fighter.DamageInflicted += OnDamageInflicted;
                fighter.Dead += OnDead;
            }

            Fight.TurnStarted += OnTurnStarted;
        }

        public override bool IsEligible() => Fight.GetAllFighters<CharacterFighter>().Count() > 1;

        void OnTurnStarted(IFight fight, FightActor fighter)
        {
            if (!(fighter is CharacterFighter))
                return;

            int round;
            m_contaminedFighters.TryGetValue(fighter, out round);

            if ((Fight.TimeLine.RoundNumber - round) >= 5)
                UpdateStatus(ChallengeStatusEnum.FAILED, fighter);
        }

        void OnDead(FightActor fighter, FightActor killer)
        {
            if (m_contaminedFighters.ContainsKey(fighter))
                m_contaminedFighters.Remove(fighter);
        }

        void OnDamageInflicted(FightActor fighter, Damage damage)
        {
            if (!(fighter is CharacterFighter))
                return;

            if (damage.Source is MonsterFighter && !m_contaminedFighters.ContainsKey(fighter))
                m_contaminedFighters.Add(fighter, Fight.TimeLine.RoundNumber);
        }

        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            base.OnWinnersDetermined(fight, winners, losers, draw);

            foreach (var fighter in Fight.GetAllFighters<CharacterFighter>())
            {
                fighter.DamageInflicted -= OnDamageInflicted;
                fighter.Dead -= OnDead;
            }

            Fight.TurnStarted -= OnTurnStarted;
        }
    }
}
