using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.DUEL)]
    public class DuelChallenge : DefaultChallenge
    {
        private readonly Dictionary<MonsterFighter, CharacterFighter> m_history = new Dictionary<MonsterFighter, CharacterFighter>();

        public DuelChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 40;
            BonusMax = 40;
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
            {
                fighter.DamageInflicted += OnDamageInflicted;
            }
        }

        public override bool IsEligible()
        {
            return Fight.GetAllFighters<MonsterFighter>().Count() > 1;
        }

        private void OnDamageInflicted(FightActor fighter, Damage damage)
        {
            var source = (damage.Source is SummonedFighter) ? ((SummonedFighter) damage.Source).Summoner : damage.Source;

            if (!(source is CharacterFighter))
                return;

            CharacterFighter caster;
            m_history.TryGetValue((MonsterFighter) fighter, out caster);

            if (caster == null)
            {
                m_history.Add((MonsterFighter)fighter, (CharacterFighter)source);
                return;
            }

            if (caster == source)
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED, source);
        }
    }
}
