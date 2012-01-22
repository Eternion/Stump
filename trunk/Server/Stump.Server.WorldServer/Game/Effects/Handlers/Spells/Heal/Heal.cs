using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Heal
{
    [EffectHandler(EffectsEnum.Effect_HealHP_108)]
    [EffectHandler(EffectsEnum.Effect_HealHP_143)]
    [EffectHandler(EffectsEnum.Effect_HealHP_81)]
    public class Heal : SpellEffectHandler
    {
        public Heal(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override void Apply()
        {
            foreach (FightActor actor in GetAffectedActors())
            {
                var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

                if (integerEffect == null)
                    return;

                if (Effect.Duration > 0)
                {
                    AddTriggerBuff(actor, true, TriggerType.TURN_BEGIN, HealBuffTrigger);
                }
                else
                {
                    actor.Heal(Caster, integerEffect.Value);
                }
            }
        }

        private static void HealBuffTrigger(TriggerBuff buff, TriggerType trigger)
        {
            var integerEffect = buff.Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

            if (integerEffect == null)
                return;

            buff.Target.Heal(buff.Caster, integerEffect.Value);
        }
    }
}