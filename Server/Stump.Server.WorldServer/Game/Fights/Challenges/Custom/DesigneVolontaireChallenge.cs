using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.DÉSIGNÉ_VOLONTAIRE)]
    public class DesigneVolontaireChallenge : DefaultChallenge
    {
        public DesigneVolontaireChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 25;

            Target = fight.GetRandomFighter<MonsterFighter>();

            foreach (var fighter in Target.Team.Fighters)
            {
                fighter.Dead += OnDead;
            }
        }

        public override bool IsEligible()
        {
            return Fight.GetAllFighters<MonsterFighter>().Count() > 1;
        }

        private void OnDead(FightActor victim, FightActor killer)
        {
            if (victim == Target)
                UpdateStatus(ChallengeStatusEnum.SUCCESS);

            UpdateStatus(ChallengeStatusEnum.FAILED);
        }
    }
}
