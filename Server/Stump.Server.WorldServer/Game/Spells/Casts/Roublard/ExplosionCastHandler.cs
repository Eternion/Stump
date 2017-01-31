using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Targets;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Roublard
{
    [SpellCastHandler(SpellIdEnum.EXPLOSION_ROUBLARDE)]
    [SpellCastHandler(SpellIdEnum.AVERSE_ROUBLARDE)]
    [SpellCastHandler(SpellIdEnum.TORNADE_ROUBLARDE)]
    public class ExplosionCastHandler : DefaultSpellCastHandler
    {
        public ExplosionCastHandler(SpellCastInformations cast)
            : base(cast)
        {
        }

        public override bool Initialize()
        {
            if (base.Initialize())
            {
                foreach (var handler in Handlers.Where(x => x.Targets.OfType<StateCriterion>().Any(y => y.State == (int) SpellStatesEnum.KABOOM_92)))
                    handler.DefaultDispellableStatus = FightDispellableEnum.DISPELLABLE_BY_STRONG_DISPEL;
                return true;
            }
            return false;
        }
    }
}