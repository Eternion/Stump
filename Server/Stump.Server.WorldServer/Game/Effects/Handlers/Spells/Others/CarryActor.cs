using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Others
{
    [EffectHandler(EffectsEnum.Effect_Carry)]
    public class CarryActor : SpellEffectHandler
    {
        public CarryActor(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var affectedActor in GetAffectedActors())
            {
                if (!Caster.CarryActor(affectedActor, Effect, Spell, CastHandler))
                    return false;
            }

            return true;
        }
    }

    [EffectHandler(EffectsEnum.Effect_Throw)]
    public class ThrowActor : SpellEffectHandler
    {
        public ThrowActor(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            if (Caster.IsCarrying())
                Caster.ThrowActor(TargetedCell);

            return true;
        }
    }
}
