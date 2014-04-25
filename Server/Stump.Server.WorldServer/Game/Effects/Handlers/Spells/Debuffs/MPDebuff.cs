using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_SubMP)]
    [EffectHandler(EffectsEnum.Effect_SubMP_1080)]
    public class MPDebuff: SpellEffectHandler
    {
        public MPDebuff(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
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

                if (Effect.Duration > 1)
                {
                    AddStatBuff(actor, (short)(-(integerEffect.Value)), PlayerFields.MP, true, (short)EffectsEnum.Effect_SubMP);
                }
                else
                {
                    actor.LostMP(integerEffect.Value);
                }
            }

            return true;
        }
    }
}
