using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.INCURABLE)]
    public class IncurableChallenge : DefaultChallenge
    {
        public IncurableChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 20;
            BonusMax = 40;
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var fighter in Fight.GetAllFighters<CharacterFighter>())
            {
                fighter.LifePointsChanged += OnLifePointsChanged;
            }
        }

        public override bool IsEligible()
        {
            return Fight.GetAllCharacters().Any(x => x.BreedId != PlayableBreedEnum.Pandawa);
        }

        private void OnLifePointsChanged(FightActor fighter, int delta, int shieldDamages, int permanentDamages, FightActor from)
        {
            if (delta > 0)
                UpdateStatus(ChallengeStatusEnum.FAILED, fighter);
        }
    }
}
