using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.AI.Fights.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Steamer
{
    [SpellCastHandler(SpellIdEnum.BOUMF_I)]
    [SpellCastHandler(SpellIdEnum.BOUMF_II)]
    [SpellCastHandler(SpellIdEnum.BOUMF_III)]
    [SpellCastHandler(SpellIdEnum.BOUME_I)]
    [SpellCastHandler(SpellIdEnum.BOUME_II)]
    [SpellCastHandler(SpellIdEnum.BOUME_III)]
    [SpellCastHandler(SpellIdEnum.BOUMT_I)]
    [SpellCastHandler(SpellIdEnum.BOUMT_II)]
    [SpellCastHandler(SpellIdEnum.BOUMT_III)]
    [SpellCastHandler(SpellIdEnum.CINÉTIK_I)]
    [SpellCastHandler(SpellIdEnum.CINÉTIK_II)]
    [SpellCastHandler(SpellIdEnum.CINÉTIK_III)]
    [SpellCastHandler(SpellIdEnum.MAGNÉTOR_I)]
    [SpellCastHandler(SpellIdEnum.MAGNÉTOR_II)]
    [SpellCastHandler(SpellIdEnum.MAGNÉTOR_III)]
    [SpellCastHandler(SpellIdEnum.MAINTENANCE_I)]
    [SpellCastHandler(SpellIdEnum.MAINTENANCE_II)]
    [SpellCastHandler(SpellIdEnum.MAINTENANCE_III)]
    public class TurretCastHandler : DefaultSpellCastHandler
    {
        public TurretCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override bool Initialize()
        {
            base.Initialize();

            return !Handlers.Any(handler =>
                handler.GetAffectedActors().Any(actor =>
                    (actor.HasState((int) SpellStatesEnum.Corselet) && Caster.IsFriendlyWith(actor) &&
                     handler.Category != SpellCategory.Healing)
                    ||
                    (actor.HasState((int) SpellStatesEnum.Dreadnaut) && !Caster.IsFriendlyWith(actor) &&
                     handler.Category == SpellCategory.Healing)));
        }
    }
}
