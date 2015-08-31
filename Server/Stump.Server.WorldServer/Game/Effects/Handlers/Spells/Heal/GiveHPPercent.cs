using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
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
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return false;

            foreach (var actor in GetAffectedActors())
            {
                HealHpPercent(actor, integerEffect.Value);
            }

            if (Effect.Duration <= 0)
                DealHpPercent(integerEffect.Value);

            return true;
        }

        // Todo: reduce duplication (see RestoreHpPercent)
        private void HealHpPercent(FightActor actor, int percent)
        {
            var healAmount = (int)(Caster.LifePoints * (percent / 100d));

            actor.Heal(healAmount, Caster, false);
        }

        private void DealHpPercent(int percent)
        {
            var damageAmount = (int)(Caster.LifePoints * (percent / 100d));

            Caster.InflictDirectDamage(damageAmount, Caster);
        }
    }
}