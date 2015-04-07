using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.MYSTIQUE)]
    public class MystiqueChallenge : DefaultChallenge
    {
        public MystiqueChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 40;

            foreach (var fighter in fight.GetAllFighters<CharacterFighter>())
            {
                fighter.WeaponUsed += OnWeaponUsed;
            }
        }

        private void OnWeaponUsed(FightActor caster, WeaponTemplate weapon, Cell target, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            UpdateStatus(ChallengeStatusEnum.FAILED);
        }
    }
}
