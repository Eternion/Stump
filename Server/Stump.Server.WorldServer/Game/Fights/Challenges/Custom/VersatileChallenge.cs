using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.VERSATILE)]
    public class VersatileChallenge : DefaultChallenge
    {
        public VersatileChallenge(int id, IFight fight)
            : base(id, fight)
        {
            Bonus = 50;

            foreach (var fighter in Fight.GetAllFighters<CharacterFighter>())
            {
                fighter.SpellCasting += OnSpellCasting;
            }
        }

        private void OnSpellCasting(FightActor caster, Spell spell, Cell target, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            if (critical == FightSpellCastCriticalEnum.CRITICAL_FAIL)
                return;

            var entries =
                caster.SpellHistory.GetEntries(
                    x => x.Spell.SpellId == spell.Id && x.CastRound == Fight.TimeLine.RoundNumber);
            if (!entries.Any())
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED);
        }
    }
}
