using System.Collections.Generic;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.ELÉMENTAIRE)]
    public class ElementaireChallenge : DefaultChallenge
    {
        private readonly Dictionary<CharacterFighter, EffectSchoolEnum> m_fightersElements = new Dictionary<CharacterFighter, EffectSchoolEnum>();
 
        public ElementaireChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 30;

            foreach (var fighter in fight.GetAllFighters<MonsterFighter>())
            {
                fighter.BeforeDamageInflicted += OnBeforeDamageInflicted;
            }
        }

        private void OnBeforeDamageInflicted(FightActor fighter, Damage damage)
        {
            if (!(damage.Source is CharacterFighter))
                return;

            if (m_fightersElements.ContainsKey((CharacterFighter)damage.Source))
            {
                EffectSchoolEnum school;
                m_fightersElements.TryGetValue((CharacterFighter) damage.Source, out school);

                if (damage.School != school)
                    UpdateStatus(ChallengeStatusEnum.FAILED, damage.Source);
            }
            else
            {
                m_fightersElements.Add((CharacterFighter)damage.Source, damage.School);
            }
        }
    }
}
