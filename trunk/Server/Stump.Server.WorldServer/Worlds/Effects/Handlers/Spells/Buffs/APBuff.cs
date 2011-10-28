using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_RegainAP)]
    [EffectHandler(EffectsEnum.Effect_AddAP_111)]
    public class APBuff : SpellEffectHandler
    {
        public APBuff(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override void Apply()
        {
            var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

            if (integerEffect == null)
                return;

            foreach (FightActor actor in GetAffectedActors())
            {
                if (Effect.Duration > 0)
                {
                    AddStatBuff(actor, integerEffect.Value, CaracteristicsEnum.AP, true);
                }
                else
                {
                    actor.RegainAP(integerEffect.Value);
                }
            }
        }
    }
}