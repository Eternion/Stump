using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_TriggerBuff)]
    [EffectHandler(EffectsEnum.Effect_TriggerBuff_793)]
    public class BuffTrigger : SpellEffectHandler
    {
        public BuffTrigger(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var triggerType = BuffTriggerType.AFTER_ATTACKED;
                TriggerBuffApplyHandler triggerHandler = DefaultBuffTrigger;

                switch ((SpellIdEnum)Spell.Id)
                {
                    case SpellIdEnum.FRICTION:
                        triggerHandler = FrictionBuffTrigger;
                        break;
                    case SpellIdEnum.POUTCH:
                    case SpellIdEnum.BRISE_L_ÂME:
                        triggerType = BuffTriggerType.BUFF_ADDED;
                        break;
                    case SpellIdEnum.RÉMISSION:
                        triggerHandler = RemissionBuffTrigger;
                        break;
                    case SpellIdEnum.MOT_LOTOF:
                        triggerType = BuffTriggerType.TURN_BEGIN;
                        break;
                    case SpellIdEnum.SACCHAROSE:
                        triggerType = BuffTriggerType.LOST_MP;
                        break;
                    case SpellIdEnum.MANSOMURE:
                        triggerType = BuffTriggerType.AFTER_HEALED;
                        break;
                    case SpellIdEnum.INIMOUTH:
                        triggerType = BuffTriggerType.DAMAGES_PUSHBACK;
                        break;
                    case SpellIdEnum.RATTRAPAGE:
                        triggerType = BuffTriggerType.TACKLED;
                        break;
                    case SpellIdEnum.ÉVOLUTION:
                        triggerType = BuffTriggerType.BUFF_ADDED;
                        triggerHandler = EvolutionBuffTrigger;
                        break;
                    case SpellIdEnum.GLOURS_POURSUITE:
                    case SpellIdEnum.GLOURSON_DE_CLOCHE:
                        break;
                    default:
                        return false;
                }

                var buffId = actor.PopNextBuffId();

                var spell = new Spell(Dice.DiceNum, Spell.CurrentLevel);
                var effect = spell.CurrentSpellLevel.Effects[0];

                var buff = new TriggerBuff(buffId, actor, Caster, effect, spell, false, false,
                    triggerType, triggerHandler)
                {
                    Duration = (short)Dice.Duration
                };

                actor.AddAndApplyBuff(buff);
            }

            return true;
        }

        private static void DefaultBuffTrigger(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            buff.Target.CastSpell(buff.Spell, buff.Target.Cell, true, true);
        }

        private static void EvolutionBuffTrigger(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            if (!buff.Target.HasState((int)SpellStatesEnum.Evolution_II) && !buff.Target.HasState((int)SpellStatesEnum.Evolution_III))
                buff.Target.CastSpell(new Spell((int)SpellIdEnum.ÉVOLUTION_II, buff.Spell.CurrentLevel), buff.Target.Cell, true, true);
            else if (buff.Target.HasState((int)SpellStatesEnum.Evolution_II) && !buff.Target.HasState((int)SpellStatesEnum.Evolution_III))
                buff.Target.CastSpell(new Spell((int)SpellIdEnum.ÉVOLUTION_III, buff.Spell.CurrentLevel), buff.Target.Cell, true, true);
        }

        private static void RemissionBuffTrigger(TriggerBuff buff, BuffTriggerType trigger, object token)
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

        private static void FrictionBuffTrigger(TriggerBuff buff, BuffTriggerType trigger, object token)
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
            return !actor.IsPoisonSpellCast(spell) && !actor.IsIndirectSpellCast(spell);
        }
    }
}
