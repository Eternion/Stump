using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using System.Linq;
using Stump.Server.WorldServer.Game.Spells.Casts;
using System;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_DispatchDamages)]
    public class DispatchDamages : SpellEffectHandler
    {
        public DispatchDamages(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            AddTriggerBuff(Caster, FightDispellableEnum.DISPELLABLE_BY_DEATH, BuffTriggerType.AfterDamaged, BuffTrigger);

            return true;
        }

        void BuffTrigger(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var damage = token as Fights.Damage;
            if (damage == null)
                return;

            if (damage.IsWeaponAttack)
                return;

            if (damage.Source == null)
                return;

            var damages = (int)Math.Floor((damage.Amount * Dice.DiceNum) / 100.0);

            foreach (var actor in GetAffectedActors())
            {
                var reflectDamage = new Fights.Damage(damages)
                {
                    Source = buff.Target,
                    School = damage.School,
                    IsCritical = damage.IsCritical,
                    IgnoreDamageBoost = true,
                    IgnoreDamageReduction = false,
                    Spell = null
                };

                actor.InflictDamage(reflectDamage);
            }

            buff.Target.RemoveBuff(buff);
        }
    }
}