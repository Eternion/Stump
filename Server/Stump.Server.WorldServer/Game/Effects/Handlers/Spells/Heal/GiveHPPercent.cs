using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells.Casts;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Heal
{
    [EffectHandler(EffectsEnum.Effect_GiveHPPercent)]
    public class GiveHpPercent : SpellEffectHandler
    {
        public GiveHpPercent(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return false;

            var affectedActors = GetAffectedActors();

            if (!affectedActors.Any())
                return false;

            var healAmount = (int)(Caster.LifePoints * (integerEffect.Value / 100d));

            foreach (var actor in affectedActors)
                actor.Heal(healAmount, Caster, true);

            if (Effect.Duration == 0 && healAmount > 0)
                Caster.InflictDirectDamage(healAmount, Caster);

            return true;
        }
    }
}