using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_DamageIntercept)]
    public class Sacrifice : SpellEffectHandler
    {
        public Sacrifice(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool CanApply()
        {
            return !GetAffectedActors().Any(x => x.GetBuffs(y => y.Effect.EffectId == EffectsEnum.Effect_DamageIntercept).Any());
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                AddTriggerBuff(actor, false, BuffTriggerType.BEFORE_ATTACKED, TriggerBuffApply);
                AddTriggerBuff(actor, false, BuffTriggerType.AFTER_ATTACKED, PostTriggerBuffApply);
            }

            return true;
        }

        public void TriggerBuffApply(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var target = buff.Target;

            if (target == null)
                return;

            if (target.IsSacrificeProtected)
                return;

            var damage = token as Fights.Damage;
            if (damage == null || damage.Amount == 0 || damage.MarkTrigger != null)
                return;

            target.IsSacrificeProtected = true;

            if (buff.Spell.Template.Id == (int)SpellIdEnum.SACRIFICE_440)
                Caster.ExchangePositions(target);
            else if (Caster is SummonedTurret)
            {
                target.IsSacrificeProtected = false;

                var source = damage.Source;

                if (!source.Position.Point.IsAdjacentTo(target.Position.Point))
                    return;

                if (!Caster.Position.Point.IsAdjacentTo(target.Position.Point))
                    return;
            }

            // first, apply damage to sacrifier
            Caster.InflictDamage(damage);

            // then, negate damage given to target
            damage.IgnoreDamageBoost = true;
            damage.IgnoreDamageReduction = true;
            damage.Generated = true;
            damage.Amount = 0;
        }

        public void PostTriggerBuffApply(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var target = buff.Target;

            if (target == null)
                return;

            target.IsSacrificeProtected = false;
        }
    }
}