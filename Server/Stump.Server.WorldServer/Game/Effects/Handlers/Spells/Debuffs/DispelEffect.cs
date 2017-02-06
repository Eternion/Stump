using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_DispelMagicEffects)]
    public class DispelMagicEffects : SpellEffectHandler
    {
        public DispelMagicEffects(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                if (Duration != 0)
                {
                    AddTriggerBuff(actor, OnBuffTriggered);
                }
                else
                {
                    actor.RemoveAndDispellAllBuffs();
                }
            }

            return true;
        }

        static void OnBuffTriggered(TriggerBuff buff, FightActor triggerer, BuffTriggerType trigger, object token)
        {
            buff.Target.RemoveAndDispellAllBuffs();
        }
    }

    [EffectHandler(EffectsEnum.Effect_RemoveSpellEffects)]
    public class RemoveSpellEffects : SpellEffectHandler
    {
        public RemoveSpellEffects(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                if (Duration != 0 || Delay != 0)
                {
                    AddTriggerBuff(actor, OnBuffTriggered);
                }
                else
                {
                    var integerEffect = GenerateEffect();

                    if (integerEffect == null)
                        return false;

                    actor.RemoveSpellBuffs(integerEffect.Value);
                }
            }

            return true;
        }

        void OnBuffTriggered(TriggerBuff buff, FightActor triggerer, BuffTriggerType trigger, object token)
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return;

            buff.Target.RemoveSpellBuffs(integerEffect.Value);
        }
    }
}
