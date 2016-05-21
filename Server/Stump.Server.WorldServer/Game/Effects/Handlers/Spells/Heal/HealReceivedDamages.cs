using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells.Casts;
using System;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Heal
{
    [EffectHandler(EffectsEnum.Effect_HealReceivedDamages)]
    public class HealReceivedDamages : SpellEffectHandler
    {
        public HealReceivedDamages(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            AddTriggerBuff(Caster, BuffTriggerType.AfterDamaged, BuffTrigger);

            return true;
        }

        void BuffTrigger(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var damage = token as Fights.Damage;
            if (damage == null)
                return;

            if (damage.Source == null)
                return;

            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return;

            var healAmount = (int)Math.Floor((damage.Amount * Dice.DiceNum) / 100.0);

            foreach (var actor in GetAffectedActors())
            {
                actor.Heal(healAmount, Caster, true);
            }

            buff.Caster.RemoveBuff(buff);
        }
    }
}
