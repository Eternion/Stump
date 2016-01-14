using Stump.Core.Memory;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Others
{
    [EffectHandler(EffectsEnum.Effect_RandDownModifier)]
    [EffectHandler(EffectsEnum.Effect_RandUpModifier)]
    public class RandomModifier : SpellEffectHandler
    {
        public RandomModifier(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var target in GetAffectedActors())
            {
                if (Dice.EffectId == EffectsEnum.Effect_RandDownModifier)
                {
                    AddTriggerBuff(target, true, BuffTriggerType.AfterRollCritical, RollTrigger);
                }

                AddTriggerBuff(target, true, Dice.EffectId == EffectsEnum.Effect_RandDownModifier ?
                BuffTriggerType.BeforeAttack : BuffTriggerType.BeforeDamaged, DamageModifier);
            }

            return true;
        }

        void RollTrigger(TriggerBuff buff, FightActor triggerer, BuffTriggerType trigger, object token)
        {
            var @ref = token as Ref<FightSpellCastCriticalEnum>;
            if (@ref != null)
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