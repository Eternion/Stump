using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Others
{
    [EffectHandler(EffectsEnum.Effect_TriggerGlyphs)]
    public class TriggerGlyphs : SpellEffectHandler
    {
        public TriggerGlyphs(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var triggers = actor.Fight.GetTriggersByCell(actor.Cell);

                foreach (var trigger in triggers)
                {
                    foreach (var shape in trigger.Shapes)
                    {
                        foreach (var cell in shape.GetCells())
                        {
                            foreach (var fighter in actor.Fight.GetAllFighters(x => x.Cell == cell))
                                actor.Fight.TriggerMarks(cell, fighter, Fights.Triggers.TriggerType.MOVE);
                        }
                    }
                }
            }

            return true;
        }
    }
}
