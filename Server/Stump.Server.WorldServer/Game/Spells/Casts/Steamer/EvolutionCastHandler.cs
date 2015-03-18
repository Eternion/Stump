using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Steamer
{
    [SpellCastHandler(SpellIdEnum.ÉVOLUTION)]
    public class EvolutionCastHandler : DefaultSpellCastHandler
    {
        public EvolutionCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            var affectedActors = Handlers[0].GetAffectedActors();

            foreach (var actor in affectedActors)
            {
                if (!actor.HasState((int)SpellStatesEnum.Evolution_II) && !actor.HasState((int)SpellStatesEnum.Evolution_III))
                    actor.CastSpell(new Spell((int)SpellIdEnum.ÉVOLUTION_II, 1), TargetedCell, true, true);
                else if (!actor.HasState((int) SpellStatesEnum.Evolution_III))
                    actor.CastSpell(new Spell((int)SpellIdEnum.ÉVOLUTION_III, 1), TargetedCell, true, true);
            }
        }
    }
}
