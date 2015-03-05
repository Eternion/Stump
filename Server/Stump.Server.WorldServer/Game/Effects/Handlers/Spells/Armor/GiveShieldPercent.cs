using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Armor
{
    [EffectHandler(EffectsEnum.Effect_AddShieldPercent)]
    public class GiveShieldPercent : SpellEffectHandler
    {
        public GiveShieldPercent(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
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

                var shieldAmount = (short) (actor.MaxLifePoints*(integerEffect.Value/100d));

                if (Effect.Duration > 1)
                {
                    AddStatBuff(actor, shieldAmount, PlayerFields.Shield, true);
                }
                else
                {
                    actor.Stats[PlayerFields.Shield].Context += shieldAmount;
                }
            }

            return true;
        }
    }
}
