using Stump.Core.Memory;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Others
{
    [EffectHandler(EffectsEnum.Effect_RandDownModifier)]
    [EffectHandler(EffectsEnum.Effect_RandUpModifier)]
    public class RandomModifier : SpellEffectHandler
    {
        public RandomModifier(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var target in GetAffectedActors())
            {
                if (Dice.EffectId == EffectsEnum.Effect_RandDownModifier)
                {
                    AddTriggerBuff(target, BuffTriggerType.AfterRollCritical, RollTrigger);
                }

                AddTriggerBuff(target, Dice.EffectId == EffectsEnum.Effect_RandDownModifier ?
                BuffTriggerType.BeforeAttack : BuffTriggerType.BeforeDamaged, DamageModifier);
            }

            return true;
        }

        void RollTrigger(TriggerBuff buff, FightActor triggerer, BuffTriggerType trigger, object token)
        {
            if (token is Ref<FightSpellCastCriticalEnum> @ref)
                @ref.Target = FightSpellCastCriticalEnum.NORMAL;
        }

        void DamageModifier(TriggerBuff buff, FightActor triggerer, BuffTriggerType trigger, object token)
        {
            var damage = token as Fights.Damage;
            if (damage == null)
                return;

            damage.EffectGenerationType = Dice.EffectId == EffectsEnum.Effect_RandDownModifier ?
                EffectGenerationType.MinEffects : EffectGenerationType.MaxEffects;

            damage.Generated = false;
            damage.GenerateDamages();
        }
    }
}