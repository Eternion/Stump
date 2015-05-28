using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_RegainAP)]
    [EffectHandler(EffectsEnum.Effect_AddAP_111)]
    public class APBuff : SpellEffectHandler
    {
        public APBuff(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
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

                if (Effect.Duration > 1 || Effect.Duration == -1)
                {
                    AddStatBuff(actor, integerEffect.Value, PlayerFields.AP, true);
                }
                else
                {
                    actor.RegainAP(integerEffect.Value);
                }
            }

            return true;
        }
    }
}