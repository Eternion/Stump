using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_Frikt)]
    public class Frikt : SpellEffectHandler
    {
        public Frikt(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var buffId = actor.PopNextBuffId();


                switch (Spell.Id)
                {
                    case (int)SpellIdEnum.FRICTION:
                    {
                        var spell = new Spell(Dice.DiceNum, 1);
                        var effect = spell.CurrentSpellLevel.Effects[0];

                        var buff = new TriggerBuff(buffId, actor, Caster, effect, spell, false, false,
                            BuffTriggerType.AFTER_ATTACKED, FriktBuffTrigger)
                        {
                            Duration = (short)Dice.Duration
                        };

                        actor.AddAndApplyBuff(buff);
                    }
                        break;
                    case (int)SpellIdEnum.MOT_LOTOF:
                    {
                        var spell = new Spell(Dice.DiceNum, Spell.CurrentLevel);
                        var effect = spell.CurrentSpellLevel.Effects[0];

                        var buff = new TriggerBuff(buffId, actor, Caster, effect, spell, false, false,
                            BuffTriggerType.TURN_BEGIN, MotLotofBuffTrigger)
                        {
                            Duration = (short)Dice.Duration
                        };

                        actor.AddAndApplyBuff(buff);
                    }
                        break;
                    default:
                    {
                        var spell = new Spell(Dice.DiceNum, Spell.CurrentLevel);
                        var effect = spell.CurrentSpellLevel.Effects[0];

                        var buff = new TriggerBuff(buffId, actor, Caster, effect, spell, false, false,
                            BuffTriggerType.AFTER_ATTACKED, SuppressionBuffTrigger)
                        {
                            Duration = (short)Dice.Duration
                        };

                        actor.AddAndApplyBuff(buff);
                    }
                        break;
                }
            }

            return true;
        }

        private static void MotLotofBuffTrigger(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            buff.Target.CastSpell(buff.Spell, buff.Target.Cell, true, true);
        }

        private static void SuppressionBuffTrigger(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var damage = token as Fights.Damage;
            if (damage == null)
                return;

            if (damage.Source == null)
                return;

            var source = damage.Source;
            var target = buff.Target;

            if (damage.Source == target)
                return;

            if (!target.Position.Point.IsAdjacentTo(source.Position.Point))
                return;

            var effect = new Push(buff.Dice, target, buff.Spell, target.Cell, buff.Critical);
            effect.AddAffectedActor(source);
            effect.Apply();
        }

        private static void FriktBuffTrigger(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var damage = token as Fights.Damage;
            if (damage == null)
                return;

            if (damage.Spell == null)
                return;

            if (damage.Spell.Id == 0)
                return;

            if (damage.Source == null)
                return;

            if (damage.School == EffectSchoolEnum.Pushback)
                return;

            var source = damage.Source;
            var target = buff.Target;

            if (damage.Source == target)
                return;

            if (!IsValidSpell(target, damage.Spell))
                return;

            if (!target.Position.Point.IsOnSameLine(source.Position.Point))
                return;

            var effect = new Pull(buff.Dice, source, buff.Spell, source.Cell, buff.Critical);
            effect.AddAffectedActor(target);
            effect.Apply();
        }

        private static bool IsValidSpell(FightActor actor, Spell spell)
        {
            if (actor.IsPoisonSpellCast(spell))
                return false;

            if (spell.Template.Id == (int) SpellIdEnum.PIÈGE_DE_MASSE ||
                spell.Template.Id == (int) SpellIdEnum.PIÈGE_DE_MASSE_DU_DOPEUL ||
                spell.Template.Id == (int) SpellIdEnum.PIÈGE_MORTEL ||
                spell.Template.Id == (int) SpellIdEnum.PIÈGE_DE_SILENCE ||
                spell.Template.Id == (int) SpellIdEnum.PIÈGE_DE_SILENCE_DU_DOPEUL ||
                spell.Template.Id == (int) SpellIdEnum.CONCENTRATION_DE_CHAKRA ||
                spell.Template.Id == (int) SpellIdEnum.VERTIGE ||
                spell.Template.Id == (int) SpellIdEnum.GLYPHE_ENFLAMMÉ ||
                spell.Template.Id == (int)SpellIdEnum.GLYPHE_ENFLAMMÉ_DU_DOPEUL ||
                spell.Template.Id == (int)SpellIdEnum.GLYPHE_AGRESSIF_1503 ||
                spell.Template.Id == (int)SpellIdEnum.GLYPHE_AGRESSIF_17 ||
                spell.Template.Id == (int)SpellIdEnum.GLYPHE_AGRESSIF_DU_DOPEUL ||
                spell.Template.Id == (int)SpellIdEnum.GLYPHE_DE_RÉPULSION ||
                spell.Template.Id == (int)SpellIdEnum.GLYPHE_DE_RÉPULSION_DU_DOPEUL ||
                spell.Template.Id == (int)SpellIdEnum.CONTRE ||
                spell.Template.Id == (int)SpellIdEnum.MOT_D_EPINE ||
                spell.Template.Id == (int)SpellIdEnum.MOT_D_EPINE_DU_DOPEUL ||
                spell.Template.Id == (int)SpellIdEnum.MUR_DE_FEU ||
                spell.Template.Id == (int)SpellIdEnum.MUR_D_AIR ||
                spell.Template.Id == (int)SpellIdEnum.MUR_D_EAU)
                return false;

            return true;
        }
    }
}
