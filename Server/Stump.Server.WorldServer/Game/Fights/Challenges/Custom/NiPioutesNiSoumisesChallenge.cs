using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.NI_PIOUTES_NI_SOUMISES)]
    public class NiPioutesNiSoumisesChallenge : DefaultChallenge
    {
        public NiPioutesNiSoumisesChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 40;

            foreach (var fighter in Fight.GetAllFighters<MonsterFighter>())
            {
                fighter.Dead += OnDead;
            }
        }

        public override bool IsEligible()
        {
            return Fight.GetAllCharacters().Any(x => x.Sex == SexTypeEnum.SEX_MALE) &&
                   Fight.GetAllCharacters().Any(x => x.Sex == SexTypeEnum.SEX_FEMALE);
        }

        private void OnDead(FightActor fighter, FightActor killer)
        {
            if (!(killer is CharacterFighter))
                return;

            if (((CharacterFighter)killer).Character.Sex == SexTypeEnum.SEX_FEMALE)
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED);
        }
    }
}
