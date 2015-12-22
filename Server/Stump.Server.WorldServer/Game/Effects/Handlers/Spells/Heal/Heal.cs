using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Heal
{
    [EffectHandler(EffectsEnum.Effect_HealHP_108)]
    [EffectHandler(EffectsEnum.Effect_HealHP_143)]
    [EffectHandler(EffectsEnum.Effect_HealHP_81)]
    public class Heal : SpellEffectHandler
    {
        public Heal(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
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

                if (Effect.Duration > 0)
                {
                    var triggerType = BuffTriggerType.TURN_BEGIN;

                    switch (Spell.Id)
                    {
                        case (int)SpellIdEnum.SPORE_TEILLE:
                            triggerType = BuffTriggerType.TACKLE;
                            break;
                    }

                    AddTriggerBuff(actor, true, triggerType, HealBuffTrigger);
                }
                else
                {
                    if (actor.IsAlive())
                        actor.Heal(integerEffect.Value, Caster);
                }
            }

            return true;
        }

        private static void HealBuffTrigger(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var integerEffect = buff.GenerateEffect();

            if (integerEffect == null)
                return;

            if (buff.Target.IsAlive())
                buff.Target.Heal(integerEffect.Value, buff.Caster);
        }
    }
}