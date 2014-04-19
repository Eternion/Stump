using System;
using System.Collections;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Heal
{
    [EffectHandler(EffectsEnum.Effect_StealHPAir)]
    [EffectHandler(EffectsEnum.Effect_StealHPEarth)]
    [EffectHandler(EffectsEnum.Effect_StealHPFire)]
    [EffectHandler(EffectsEnum.Effect_StealHPWater)]
    [EffectHandler(EffectsEnum.Effect_StealHPNeutral)]
    [EffectHandler(EffectsEnum.Effect_StealHPFix)]
    public class StealHpFix : SpellEffectHandler
    {
        public StealHpFix(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
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
                    AddTriggerBuff(actor, true, BuffTriggerType.TURN_BEGIN, OnBuffTriggered);
                }
                else
                {
                    var damage = new Fights.Damage(Dice, GetEffectSchool(Dice.EffectId), Caster, Spell);
                    damage.GenerateDamages();

                    if (Dice.EffectId == EffectsEnum.Effect_StealHPFix)
                        actor.InflictDirectDamage(damage.Amount); // I could use a semicolon operator right here ...
                    else
                        actor.InflictDamage(damage);
                }
            }

            return true;
        }

        private static void OnBuffTriggered(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
        }

        private EffectSchoolEnum GetEffectSchool(EffectsEnum effect)
        {
            switch (effect)
            {
                case EffectsEnum.Effect_StealHPAir:
                    return EffectSchoolEnum.Air;
                case EffectsEnum.Effect_StealHPEarth:
                    return EffectSchoolEnum.Earth;
                case EffectsEnum.Effect_StealHPFire:
                    return EffectSchoolEnum.Fire;
                case EffectsEnum.Effect_StealHPWater:
                    return EffectSchoolEnum.Water;
                case EffectsEnum.Effect_StealHPNeutral:
                    return EffectSchoolEnum.Neutral;
                default:
                    throw new Exception(string.Format("Effect {0} has not associated School Type", effect));
            }
        }
    }
}