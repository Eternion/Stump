using System.Linq;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Others
{
    [EffectHandler(EffectsEnum.Effect_RandDownModifier)]
    [EffectHandler(EffectsEnum.Effect_RandUpModifier)]
    public class RandomModifier : SpellEffectHandler
    {
        public RandomModifier(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var target in GetAffectedActors())
            {
                AddTriggerBuff(target, true, BuffTriggerType.BEFORE_ATTACK, DamageModifier);
            }

            return true;
        }


        private void DamageModifier(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var damage = token as Fights.Damage;
            if (damage == null)
                return;

            if (Dice.EffectId == EffectsEnum.Effect_RandDownModifier)
                damage.EffectGenerationType = EffectGenerationType.MinEffects;
            else
                damage.EffectGenerationType = EffectGenerationType.MaxEffects;
        }
    }
}