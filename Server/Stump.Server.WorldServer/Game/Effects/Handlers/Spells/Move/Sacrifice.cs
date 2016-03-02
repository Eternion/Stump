using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;

using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_DamageIntercept)]
    public class Sacrifice : SpellEffectHandler
    {
        public Sacrifice(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        public override bool CanApply()
        {
            return !GetAffectedActors().Any(x => x.GetBuffs(y => y.Effect.EffectId == EffectsEnum.Effect_DamageIntercept).Any());
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                AddTriggerBuff(actor, FightDispellableEnum.DISPELLABLE_BY_DEATH, BuffTriggerType.OnDamaged, TriggerBuffApply);
                AddTriggerBuff(actor, FightDispellableEnum.DISPELLABLE_BY_DEATH, BuffTriggerType.AfterDamaged, PostTriggerBuffApply);
            }

            return true;
        }

        public void TriggerBuffApply(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var target = buff.Target;

            if (target == null)
                return;

            if (target.IsSacrificeProtected)
                return;

            var damage = token as Fights.Damage;
            if (damage == null || damage.Amount == 0 /*|| damage.MarkTrigger != null*/)
                return;

            target.IsSacrificeProtected = true;

            // first, apply damage to sacrifier
            Caster.InflictDamage(damage);

            // then, negate damage given to target
            damage.IgnoreDamageBoost = true;
            damage.IgnoreDamageReduction = true;
            damage.Generated = true;
            damage.Amount = 0;
        }

        public void PostTriggerBuffApply(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var target = buff.Target;

            if (target == null)
                return;

            target.IsSacrificeProtected = false;
        }
    }
}