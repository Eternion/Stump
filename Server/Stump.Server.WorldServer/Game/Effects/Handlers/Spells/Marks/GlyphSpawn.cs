using System.Drawing;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Marks
{
    [EffectHandler(EffectsEnum.Effect_Glyph)]
    [EffectHandler(EffectsEnum.Effect_Glyph_402)]
    [EffectHandler(EffectsEnum.Effect_GlyphAura)]
    public class GlyphSpawn : SpellEffectHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public GlyphSpawn(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var glyphSpell = new Spell(Dice.DiceNum, (byte) Dice.DiceFace);

            if (glyphSpell.Template == null || !glyphSpell.ByLevel.ContainsKey(Dice.DiceFace))
            {
                logger.Error("Cannot find glyph spell id = {0}, level = {1}. Casted Spell = {2}", Dice.DiceNum, Dice.DiceFace, Spell.Id);
                return false;
            }

            var spell = Spell;

            if (spell.Id == (int) SpellIdEnum.DAIPIPAY)
                spell = glyphSpell;

            Glyph glyph;
            if (Effect.EffectId == EffectsEnum.Effect_GlyphAura)
                glyph = new GlyphAura((short) Fight.PopNextTriggerId(), Caster, spell, Dice, glyphSpell, TargetedCell,
                    EffectZone.ShapeType, (byte) Effect.ZoneSize, GetGlyphColorBySpell(Spell));
            else
                glyph = new Glyph((short)Fight.PopNextTriggerId(), Caster, spell, Dice, glyphSpell, TargetedCell,
                    EffectZone.ShapeType, (byte)Effect.ZoneSize, GetGlyphColorBySpell(Spell));

            Fight.AddTriger(glyph);

            return true;
        }

        private static Color GetGlyphColorBySpell(Spell spell)
        {
            switch (spell.Id)
            {
                case (int)SpellIdEnum.DAIPIPAY:
                    return Color.White;
                case (int)SpellIdEnum.GLYPHE_OPTIQUE:
                    return Color.Cyan;
                case (int)SpellIdEnum.GLYPHE_D_AVEUGLEMENT:
                    return Color.Orange;
                case (int)SpellIdEnum.GLYPHE_GRAVITATIONNEL:
                    return Color.Green;
                case (int)SpellIdEnum.GLYPHE_DE_RÉPULSION:
                    return ColorTranslator.FromHtml("#505167");
                default:
                    return Color.Red;
            }
        }
    }
}