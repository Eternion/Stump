using System.Linq;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.LES_MULES_D_ABORD)]
    public class MulesChallenge : DefaultChallenge
    {
        public MulesChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 30;
            BonusMax = 30;
        }

        public override void Initialize()
        {
            base.Initialize();

            Target = Fight.GetAllFighters<CharacterFighter>().OrderBy(x => x.Level).FirstOrDefault();

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
            {
                fighter.Dead += OnDead;
            }
        }

        public override bool IsEligible()
        {
            return (Fight.GetAllFighters<CharacterFighter>().Select(x => x.Level).Max() -
                   Fight.GetAllFighters<CharacterFighter>().Select(x => x.Level).Min()) > 50;
        }

        private void OnDead(FightActor victim, FightActor killer)
        {
            if (killer == Target)
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED, killer);
        }
    }
}
