using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Roublard
{
    [SpellCastHandler(SpellIdEnum.AIMANTATION)]
    public class MagnetSpellCastHandler : DefaultSpellCastHandler
    {
        public MagnetSpellCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override bool Initialize()
        {
            base.Initialize();

            foreach (var handler in Handlers.OfType<Pull>())
            {
                handler.CastPoint = handler.TargetedPoint;
            }

            return true;
        }
    }
}
