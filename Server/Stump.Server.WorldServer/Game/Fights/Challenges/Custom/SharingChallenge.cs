using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.PARTAGE)]
    public class SharingChallenge : DefaultChallenge
    {
        readonly List<CharacterFighter> m_killers = new List<CharacterFighter>();

        public SharingChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 50;
            BonusMax = 50;
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
                fighter.Dead += OnDead;
        }

        public override bool IsEligible() => Fight.GetAllFighters<CharacterFighter>().Count() > 1
                && Fight.GetAllFighters<MonsterFighter>().Count() >= Fight.GetAllFighters<CharacterFighter>().Count();

        void OnDead(FightActor fighter, FightActor killer)
        {
            if (killer is CharacterFighter)
                m_killers.Add((CharacterFighter)killer);
        }

        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            if (winners is FightMonsterTeam)
            {
                UpdateStatus(ChallengeStatusEnum.FAILED);
                return;
            }

            foreach (var winner in winners.Fighters.OfType<CharacterFighter>().Where(winner => !m_killers.Contains(winner)))
                UpdateStatus(ChallengeStatusEnum.FAILED, winner);

            UpdateStatus(ChallengeStatusEnum.SUCCESS);

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
                fighter.Dead -= OnDead;
        }
    }
}
