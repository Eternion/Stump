using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_AddVitalityPercent)]
    public class VitalityPercent : SpellEffectHandler
    {
        public VitalityPercent(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
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

                var bonus = (int)(actor.Stats.Health.TotalMax * (integerEffect.Value / 100d));

                if (Effect.Duration > 0)
                {
                    AddStatBuff(actor, (short)bonus, PlayerFields.Health, true,
                        (short)EffectsEnum.Effect_AddVitalityPercent);
                }
                else
                    actor.Stats[PlayerFields.Health].Context += bonus;
            }

            return true;
        }
    }
}