using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_IncreaseFinalDamages)]
    [EffectHandler(EffectsEnum.Effect_ReduceFinalDamages)]
    public class DamageModificator : SpellEffectHandler
    {
        public DamageModificator(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach(var actor in GetAffectedActors())
            {
                AddTriggerBuff(actor, false, BuffTriggerType.BeforeAttack, ModifyDamages);
            }

            return true;
        }

        private void ModifyDamages(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var damage = token as Fights.Damage;
            if (damage == null)
                return;

            var effect = GenerateEffect();

            if (effect == null)
                return;

            if (Effect.EffectId == EffectsEnum.Effect_IncreaseFinalDamages)
                damage.Amount += (int) (damage.Amount * effect.Value/100d);
            else if (Effect.EffectId == EffectsEnum.Effect_ReduceFinalDamages)
                damage.Amount -= (int)(damage.Amount * effect.Value / 100d);
        }
    }
}