﻿using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Heal
{
    [EffectHandler(EffectsEnum.Effect_HealWhenAttack)]
    public class GiveHpWhenAttack : SpellEffectHandler
    {
        public GiveHpWhenAttack(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var integerEffect = GenerateEffect();

                if (integerEffect == null)
                    return false;

                AddTriggerBuff(actor, FightDispellableEnum.DISPELLABLE_BY_DEATH, BuffTriggerType.OnDamaged, OnBuffTriggered);
            }

            return true;
        }

        void OnBuffTriggered(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var damage = token as Fights.Damage;
            if (damage == null)
                return;

            buff.Target.Heal(damage.Amount, damage.Source);
            damage.Amount = 0;
        }
    }

    [EffectHandler(EffectsEnum.Effect_GiveHpPercentWhenAttack)]
    public class GiveHpPercentWhenAttack : SpellEffectHandler
    {
        public GiveHpPercentWhenAttack(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var integerEffect = GenerateEffect();

                if (integerEffect == null)
                    return false;

                AddTriggerBuff(actor, FightDispellableEnum.DISPELLABLE, BuffTriggerType.AfterDamaged, OnBuffTriggered);
            }

            return true;
        }

        void OnBuffTriggered(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return;

            var damage = token as Fights.Damage;
            if (damage == null)
                return;

            var source = damage.Source;
            if (Spell.Id == (int)SpellIdEnum.MANSOMNAMBULE)
                source = buff.Target;

            HealHpPercent(source, damage.Amount, integerEffect.Value);
        }

        static void HealHpPercent(FightActor actor, int amount, int percent)
        {
            var healAmount = (int)(amount * (percent / 100d));

            actor.Heal(healAmount, actor, false);
        }
    }
}
