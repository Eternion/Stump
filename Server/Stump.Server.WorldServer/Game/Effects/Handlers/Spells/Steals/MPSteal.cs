using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Steals
{
    [EffectHandler(EffectsEnum.Effect_StealMP_441)]
    public class MPSteal : SpellEffectHandler
    {
        public MPSteal(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var integerEffect = GenerateEffect();

                if (integerEffect == null)
                    return false;

                AddStatBuff(actor, (short)( -( integerEffect.Value ) ), PlayerFields.MP, (short)EffectsEnum.Effect_SubMP);
                actor.TriggerBuffs(actor, BuffTriggerType.OnMPLost);

                if (Effect.Duration != 0 || Effect.Delay != 0)
                {
                    AddStatBuff(Caster, integerEffect.Value, PlayerFields.MP, (short)EffectsEnum.Effect_AddMP_128);
                }
                else
                {
                    Caster.RegainMP(integerEffect.Value);
                }
            }

            return true;
        }
    }
}