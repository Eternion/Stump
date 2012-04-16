using System.Drawing;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Marks
{
    [EffectHandler(EffectsEnum.Effect_Glyph)]
    [EffectHandler(EffectsEnum.Effect_Glyph_402)]
    public class GlyphSpawn : SpellEffectHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public GlyphSpawn(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var glyphSpell = new Spell(Dice.DiceNum, (sbyte) Dice.DiceFace);

            if (glyphSpell.Template == null || !glyphSpell.ByLevel.ContainsKey(Dice.DiceFace))
            {
                logger.Error("Cannot find glyph spell id = {0}, level = {1}. Casted Spell = {2}", Dice.DiceNum, Dice.DiceFace, Spell.Id);
                return false;
            }
            
            // todo : find usage of Dice.Value
            Glyph glyph = EffectZone.ShapeType == SpellShapeEnum.Q ?
                new Glyph((short)Fight.PopNextTriggerId(), Caster, Spell, Dice, glyphSpell, TargetedCell, GameActionMarkCellsTypeEnum.CELLS_CROSS, Effect.ZoneSize, GetGlyphColorBySpell(Spell)) :
                new Glyph((short)Fight.PopNextTriggerId(), Caster, Spell, Dice, glyphSpell, TargetedCell, Effect.ZoneSize, GetGlyphColorBySpell(Spell));

            Fight.AddTriger(glyph);

            return true;
        }

        private static Color GetGlyphColorBySpell(Spell spell)
        {
            switch (spell.Id)
            {
                default:
                    return Color.Red;
            }
        }
    }
}