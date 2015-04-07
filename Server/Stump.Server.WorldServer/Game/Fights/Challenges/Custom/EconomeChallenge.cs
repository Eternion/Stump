using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier(5)]
    public class EconomeChallenge : DefaultChallenge
    {
        public EconomeChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 160;

            foreach (var fighter in Fight.GetAllFighters<CharacterFighter>())
            {
                fighter.SpellCasting += OnSpellCasting;
            }
        }

        private void OnSpellCasting(FightActor caster, Spell spell, Cell target, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            if (critical == FightSpellCastCriticalEnum.CRITICAL_FAIL)
                return;

            var entries = caster.SpellHistory.GetEntries(x => x.Spell.SpellId == spell.Id);
            if (!entries.Any())
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED);
        }
    }
}
