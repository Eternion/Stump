using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_SubMP)]
    [EffectHandler(EffectsEnum.Effect_LosingMP)]
    [EffectHandler(EffectsEnum.Effect_LostMP)]
    public class MPDebuff : SpellEffectHandler
    {
        public MPDebuff(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
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
                
                if (Effect.Duration > 1)
                {
                    AddStatBuff(actor, (short)( -integerEffect.Value ), CaracteristicsEnum.MP, true);
                }
                else
                {
                    actor.LostMP(integerEffect.Value);
                }
            }
        }
    }
}