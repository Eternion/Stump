using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.SURVIVANT)]
    public class SurvivorChallenge : DefaultChallenge
    {
        public SurvivorChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 30;

            foreach (var fighter in Fight.GetAllFighters<CharacterFighter>())
            {
                fighter.Dead += OnDead;
            }
        }

        public override bool IsEligible()
        {
            return Fight.GetAllFighters<CharacterFighter>().Count() > 1;
        }

        private void OnDead(FightActor fighter, FightActor killer)
        {
            UpdateStatus(ChallengeStatusEnum.FAILED, fighter);
        }
    }
}
