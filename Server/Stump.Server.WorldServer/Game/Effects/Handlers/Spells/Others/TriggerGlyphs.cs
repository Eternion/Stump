using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Spells;
using System.Linq;

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
                var fight = actor.Fight;
                var triggers = fight.GetTriggersByCell(actor.Cell);

                foreach (var trigger in triggers.Where(x => x is Glyph))
                {
                    foreach (var fighter in fight.GetAllFighters(x => trigger.ContainsCell(x.Cell)))
                    {
                        fight.TriggerMarks(fighter.Cell, fighter, TriggerType.OnTurnBegin);
                    }
                }
            }

            return true;
        }
    }
}
