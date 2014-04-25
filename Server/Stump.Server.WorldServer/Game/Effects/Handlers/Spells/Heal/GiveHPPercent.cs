using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Heal
{
    [EffectHandler(EffectsEnum.Effect_GiveHPPercent)]
    public class GiveHpPercent : SpellEffectHandler
    {
        public GiveHpPercent(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var integerEffect = GenerateEffect();

                if (integerEffect == null)
                    return false;

                if (Effect.Duration > 0)
                {
                    AddTriggerBuff(actor, true, BuffTriggerType.TURN_BEGIN, OnBuffTriggered);
                }
                else
                {
                    HealHpPercent(actor, integerEffect.Value);
                    DealHpPercent(Caster, integerEffect.Value);
                }
            }

            return true;
        }

        private void OnBuffTriggered(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return;

            HealHpPercent(buff.Target, integerEffect.Value);
            DealHpPercent(Caster, integerEffect.Value);
        }

        // Todo: reduce duplication (see RestoreHpPercent)
        private void HealHpPercent(FightActor actor, int percent)
        {
            int healAmount = (int)(actor.MaxLifePoints * (percent / 100d));

            actor.Heal(healAmount, Caster, false);
        }

        private void DealHpPercent(FightActor actor, int percent)
        {
            int damageAmount = (int)(actor.MaxLifePoints * (percent / 100d));

            actor.InflictDirectDamage(damageAmount, Caster);
        }
    }
}