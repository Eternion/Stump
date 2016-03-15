using System;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Armor
{
    [EffectHandler(EffectsEnum.Effect_AddArmorDamageReduction)]
    [EffectHandler(EffectsEnum.Effect_AddGlobalDamageReduction_105)]
    public class DamageArmor : SpellEffectHandler
    {
        public DamageArmor(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var integerEffect = Effect.GenerateEffect(EffectGenerationContext.Spell) as EffectInteger;

                if (integerEffect == null)
                    return false;

                if (Effect.Duration == 0)
                    return false;

                // these spells cannot stacks
                if (actor.GetBuffs(x => x.Effect.EffectId == Effect.EffectId && x.Spell.Template.Id == Spell.Template.Id).Any())
                    continue;

                AddTriggerBuff(actor, ApplyArmorBuff);
            }

            return true;
        }

        public static void ApplyArmorBuff(TriggerBuff buff, FightActor triggerer, BuffTriggerType trigger, object token)
        {
            var damage = token as Fights.Damage;
            if (damage == null)
                return;

            var integerEffect = buff.GenerateEffect();

            if (integerEffect == null)
                return;

            var target = buff.Target;
            if (target is SummonedBomb)
            {
                target = ((SummonedBomb) target).Summoner;
            }

            var reduction = target.CalculateArmorValue(integerEffect.Value);
            var dmgReduction = Math.Min(damage.Amount, target.CalculateArmorValue(integerEffect.Value));

            ActionsHandler.SendGameActionFightReduceDamagesMessage(target.Fight.Clients, damage.Source, target, reduction);
            damage.Amount -= dmgReduction;
        }
    }
}