using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.CHACUN_SON_MONSTRE)]
    public class ToEachHisPwn : DefaultChallenge
    {
        private readonly Dictionary<MonsterFighter, CharacterFighter> m_history = new Dictionary<MonsterFighter, CharacterFighter>();
        private readonly List<CharacterFighter> m_killers = new List<CharacterFighter>();

        public ToEachHisPwn(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 60;
            BonusMax = 90;
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
            {
                fighter.DamageInflicted += OnDamageInflicted;
                fighter.Dead += OnDead;
            }
        }

        public override bool IsEligible() => Fight.GetAllFighters<MonsterFighter>().Count() > 1 && Fight.GetAllFighters<CharacterFighter>().Count() > 1
                && Fight.GetAllFighters<MonsterFighter>().Count() >= Fight.GetAllFighters<CharacterFighter>().Count();

        void OnDamageInflicted(FightActor fighter, Damage damage)
        {
            var source = (damage.Source is SummonedFighter) ? ((SummonedFighter)damage.Source).Summoner : damage.Source;

            if (!(source is CharacterFighter))
                return;

            CharacterFighter caster;
            m_history.TryGetValue((MonsterFighter)fighter, out caster);

            if (caster == null)
            {
                m_history.Add((MonsterFighter)fighter, (CharacterFighter)source);
                return;
            }

            if (caster == source)
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED, source);
        }

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
            {
                UpdateStatus(ChallengeStatusEnum.FAILED, winner);
            }

            UpdateStatus(ChallengeStatusEnum.SUCCESS);

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
            {
                fighter.DamageInflicted -= OnDamageInflicted;
                fighter.Dead -= OnDead;
            }
        }
    }
}
