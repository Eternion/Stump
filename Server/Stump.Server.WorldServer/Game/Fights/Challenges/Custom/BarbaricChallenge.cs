using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int) ChallengeEnum.BARBARE)]
    public class BarbaricChallenge : DefaultChallenge
    {
        public BarbaricChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 60;

            foreach (var fighter in fight.GetAllFighters<CharacterFighter>())
            {
                fighter.SpellCasted += OnSpellCasted;
            }
        }

        private void OnSpellCasted(FightActor caster, Spell spell, Cell target, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            UpdateStatus(ChallengeStatusEnum.FAILED, caster);
        }
    }
}
