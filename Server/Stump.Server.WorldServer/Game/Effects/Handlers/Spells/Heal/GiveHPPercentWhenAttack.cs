using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Heal
{
    [EffectHandler(EffectsEnum.Effect_GiveHpPercentWhenAttack)]
    public class GiveHpPercentWhenAttack : SpellEffectHandler
    {
        public GiveHpPercentWhenAttack(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
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

                AddTriggerBuff(actor, true, BuffTriggerType.AFTER_ATTACKED, OnBuffTriggered);
            }

            return true;
        }

        private void OnBuffTriggered(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return;

            var damage = token as Fights.Damage;
            if (damage == null)
                return;

            HealHpPercent(damage.Source, damage.Amount, integerEffect.Value);
        }

        private static void HealHpPercent(FightActor actor, int amount, int percent)
        {
            var healAmount = (int)(amount * (percent / 100d));

            actor.Heal(healAmount, actor, false);
        }
    }
}
