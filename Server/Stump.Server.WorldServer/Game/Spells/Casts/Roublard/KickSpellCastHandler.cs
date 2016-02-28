using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Roublard
{
    [SpellCastHandler(SpellIdEnum.BOTTE)]
    public class KickSpellCastHandler : DefaultSpellCastHandler
    {
        public KickSpellCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override bool Initialize()
        {
            CheckWhenExecute = true;

            base.Initialize();

            Handlers = Handlers.OrderByDescending(entry => entry.EffectZone.MinRadius).ToArray();

            return true;
        }
    }
}