using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.INCURABLE)]
    public class IncurableChallenge : DefaultChallenge
    {
        public IncurableChallenge(IFight fight)
            : base(fight)
        {
        }

        public IncurableChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 20;

            foreach (var fighter in Fight.GetAllFighters<CharacterFighter>())
            {
                fighter.LifePointsChanged += OnLifePointsChanged;
            }
        }

        private void OnLifePointsChanged(FightActor fighter, int delta, int shieldDamages, int permanentDamages, FightActor from)
        {
            if (delta > 0)
                UpdateStatus(ChallengeStatusEnum.FAILED, fighter);
        }
    }
}
