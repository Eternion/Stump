using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;using Stump.Server.WorldServer.Game.Spells.Casts;
using System;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_IncreaseFinalDamages)]
    [EffectHandler(EffectsEnum.Effect_ReduceFinalDamages)]
    public class DamageModificator : SpellEffectHandler
    {
        public DamageModificator(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        int IncreasePercent
        {
            get;
            set;
        }

        protected override bool InternalApply()
        {
            foreach(var actor in GetAffectedActors())
            {
                AddTriggerBuff(actor, FightDispellableEnum.DISPELLABLE_BY_DEATH, ModifyDamages);
            }

            return true;
        }

        void ModifyDamages(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var damage = token as Fights.Damage;
            if (damage == null)
                return;

            var effect = GenerateEffect();

            if (effect == null)
                return;

            IncreasePercent += effect.Value;

            if (Effect.EffectId == EffectsEnum.Effect_IncreaseFinalDamages)
                damage.Amount += (int)Math.Ceiling(damage.Amount * IncreasePercent / 100.0);
            else if (Effect.EffectId == EffectsEnum.Effect_ReduceFinalDamages)
                damage.Amount -= (int)Math.Ceiling(damage.Amount * IncreasePercent / 100.0);
        }
    }
}