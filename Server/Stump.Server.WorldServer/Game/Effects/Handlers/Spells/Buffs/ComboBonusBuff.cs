using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_AddComboBonus)]
    public class ComboBonusBuff : SpellEffectHandler
    {
        public ComboBonusBuff(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
            DefaultDispellableStatus = FightDispellableEnum.DISPELLABLE_BY_DEATH;
        }


        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var bomb = actor as SummonedBomb;
                if (bomb == null)
                    continue;

                var integerEffect = GenerateEffect();

                if (integerEffect == null)
                    return false;

                if (Effect.Duration != 0 || Effect.Delay != 0)
                    AddStatBuff(actor, integerEffect.Value, PlayerFields.ComboBonus);
                else
                    bomb.IncreaseDamageBonus(integerEffect.Value);
            }

            return true;
        }
    }
}
