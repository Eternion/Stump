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
        private List<FightActor> m_targets;

        public Sacrifice(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                //m_targets.Add(actor);

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

            target.IsSacrificeProtected = true;

            var damage = token as Fights.Damage;
            if (damage == null)
                return;

            Caster.ExchangePositions(target);

            // first, apply damage to sacrifier
            Caster.InflictDamage(damage);
            // then, negate damage done to target
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