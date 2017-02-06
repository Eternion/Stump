using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Heal
{
    [EffectHandler(EffectsEnum.Effect_StealHPFix)]
    public class StealHpFix : SpellEffectHandler
    {
        public StealHpFix(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
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

                if (Effect.Duration != 0 || Effect.Delay != 0)
                {
                    AddTriggerBuff(actor, BuffTriggerType.OnTurnBegin, OnBuffTriggered);
                }
                else
                {
                    var damages = new Fights.Damage(Dice, EffectSchoolEnum.Neutral, Caster, Spell, TargetedCell, EffectZone)
                    {
                        IsCritical = Critical,
                        IgnoreDamageBoost = true,
                        IgnoreDamageReduction = false
                    };

                    damages.GenerateDamages();
                    var inflictedDamages = actor.InflictDamage(damages);

                    var heal = (int)Math.Floor(inflictedDamages / 2d);
                    Caster.Heal(heal, actor, true);
                }
            }

            return true;
        }

        static void OnBuffTriggered(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var damages = new Fights.Damage(buff.Dice, EffectSchoolEnum.Unknown, buff.Caster, buff.Spell, buff.Target.Cell)
            {
                Buff = buff,
                IsCritical = buff.Critical,
            };

            damages.GenerateDamages();
            buff.Target.InflictDirectDamage(damages.Amount);

            var heal = (int)Math.Floor(damages.Amount / 2d);
            buff.Caster.Heal(heal, buff.Target, true);
        }
    }
}