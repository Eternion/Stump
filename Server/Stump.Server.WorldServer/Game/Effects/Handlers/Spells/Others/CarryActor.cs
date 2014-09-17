using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Others
{
    [EffectHandler(EffectsEnum.Effect_Carry)]
    public class CarryActor : SpellEffectHandler
    {
        public CarryActor(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var affectedActor in GetAffectedActors())
            {
                Caster.CarryActor(affectedActor, Effect, Spell);
            }

            return true;
        }
    }

    [EffectHandler(EffectsEnum.Effect_Throw)]
    public class ThrowActor : SpellEffectHandler
    {
        public ThrowActor(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            if (Caster.IsCarrying())
                Caster.ThrowActor(Caster.GetCarriedActor(), TargetedCell);

            return true;
        }
    }
}
