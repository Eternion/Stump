using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.BLITZKRIEG)]
    public class BlitzkriegChallenge : DefaultChallenge
    {
        public BlitzkriegChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 80;
            BonusMax = 125;
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
            {
                fighter.BeforeDamageInflicted += OnBeforeDamageInflicted;
            }

            Fight.TurnStarted += OnTurnStarted;
        }

        public override bool IsEligible()
        {
            return Fight.GetAllFighters<MonsterFighter>().Count() > 1;
        }

        private void OnBeforeDamageInflicted(FightActor fighter, Damage damage)
        {
            if (fighter.IsFriendlyWith(damage.Source))
                return;

            Target = fighter;
        }

        private void OnTurnStarted(IFight fight, FightActor fighter)
        {
            if (fighter == Target)
                UpdateStatus(ChallengeStatusEnum.FAILED);
        }
    }
}
