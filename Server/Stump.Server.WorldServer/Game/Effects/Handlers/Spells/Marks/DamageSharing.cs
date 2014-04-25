using System.Drawing;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Marks
{
    [EffectHandler(EffectsEnum.Effect_DamageSharing)]
    public class DamageSharing : SpellEffectHandler
    {
        public DamageSharing(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var glyph = new FractionGlyph((short) Fight.PopNextTriggerId(), Caster, Spell, Dice, TargetedCell,
                (byte) Effect.ZoneSize, Color.White);

            Fight.AddTriger(glyph);

            return true;
        }
    }
}