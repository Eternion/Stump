using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Spells;
using System.Linq;
using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Others
{
    [EffectHandler(EffectsEnum.Effect_TriggerGlyphs)]
    public class TriggerGlyphs : SpellEffectHandler
    {
        public TriggerGlyphs(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical) : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            var fight = Caster.Fight;
            var triggers = fight.GetTriggersByCell(Caster.Cell);

            foreach (var trigger in triggers.OfType<Glyph>().Where(x => x.CanBeForced && x.Caster == Caster))
            {
                using (fight.StartSequence(SequenceTypeEnum.SEQUENCE_GLYPH_TRAP))
                    trigger.TriggerAllCells(Caster);

            }

            return true;
        }
    }
}
