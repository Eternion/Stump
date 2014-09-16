using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_SubVitalityPercent)]
    public class SubVitalityPercent : SpellEffectHandler
    {
        public SubVitalityPercent(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
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

                var bonus = actor.Stats.Health.TotalMax * ( integerEffect.Value / 100d );

                AddStatBuff(actor, (short) bonus, PlayerFields.Vitality, true, (short) EffectsEnum.Effect_SubVitalityPercent);
            }

            return true;
        }
    }
}
