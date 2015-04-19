using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;

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
            {
                fighter.BeforeDamageInflicted += OnBeforeDamageInflicted;
            }
        }

        private void OnBeforeDamageInflicted(FightActor fighter, Damage damage)
        {
            if (!(damage.Source is CharacterFighter))
                return;

            if (m_element == EffectSchoolEnum.Unknown)
                m_element = damage.School;
            else if (m_element != damage.School)
                UpdateStatus(ChallengeStatusEnum.FAILED, damage.Source);
        }
    }
}
