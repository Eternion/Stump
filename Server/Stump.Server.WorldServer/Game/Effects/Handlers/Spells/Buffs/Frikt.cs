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
                

                if (Spell.Id == (int)SpellIdEnum.FRICTION)
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
                else
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
            }

            return true;
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

            var source = damage.Source;
            var target = buff.Target;

            if (damage.Source == target)
                return;

            if (!target.Position.Point.IsOnSameLine(source.Position.Point))
                return;

            var effect = new Pull(buff.Dice, source, buff.Spell, source.Cell, buff.Critical);
            effect.AddAffectedActor(target);
            effect.Apply();
        }
    }
}
