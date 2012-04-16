using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_AddMP)]
    [EffectHandler(EffectsEnum.Effect_AddMP_128)]
    public class MPBuff : SpellEffectHandler
    {
        public MPBuff(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (FightActor actor in GetAffectedActors())
            {
                var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

                if (integerEffect == null)
                    return false;

                if (Effect.Duration > 0)
                {
                    AddStatBuff(actor, integerEffect.Value, PlayerFields.MP, true);
                }
                else
                {
                    actor.RegainMP(integerEffect.Value);
                }
            }

            return true;
        }
    }
}