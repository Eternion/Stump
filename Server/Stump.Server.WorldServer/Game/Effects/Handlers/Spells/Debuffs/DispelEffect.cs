using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_DispelMagicEffects)]
    public class DispelMagicEffects : SpellEffectHandler
    {
        public DispelMagicEffects(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                actor.RemoveAndDispellAllBuffs();
            }

            return true;
        }
    }

    [EffectHandler(EffectsEnum.Effect_RemoveSpellEffects)]
    public class RemoveSpellEffects : SpellEffectHandler
    {
        public RemoveSpellEffects(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var integerEffect = GenerateEffect();

                if (integerEffect == null)
                    return false;

                actor.RemoveSpellBuffs(integerEffect.Value);
            }

            return true;
        }
    }
}
