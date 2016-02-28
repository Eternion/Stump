using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Fights.Teams;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.ELÉMENTAIRE)]
    public class ElementaryChallenge : DefaultChallenge
    {
        private EffectSchoolEnum m_element;

        public ElementaryChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 30;
            BonusMax = 50;

            m_element = EffectSchoolEnum.Unknown;
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
                fighter.BeforeDamageInflicted += OnBeforeDamageInflicted;
        }

        void OnBeforeDamageInflicted(FightActor fighter, Damage damage)
        {
            if (!(damage.Source is CharacterFighter))
                return;

            if (damage.ReflectedDamages)
                return;

            if (m_element == EffectSchoolEnum.Unknown)
                m_element = damage.School;
            else if (m_element != damage.School)
                UpdateStatus(ChallengeStatusEnum.FAILED, damage.Source);
        }

        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            base.OnWinnersDetermined(fight, winners, losers, draw);

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
                fighter.BeforeDamageInflicted -= OnBeforeDamageInflicted;
        }
    }
}
