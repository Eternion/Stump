using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_MinimizeEffects)]
    public class MaximizeEffect : SpellEffectHandler
    {
        public MaximizeEffect(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var buffId = actor.PopNextBuffId();

                var buff = new TriggerBuff(buffId, actor, Caster, Dice, Spell, false, false,
                    BuffTriggerType.BEFORE_ATTACK, MaximizeEffectsBuffTrigger);

                actor.AddAndApplyBuff(buff);
            }

            return true;
        }

        private static void MaximizeEffectsBuffTrigger(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var damage = token as Fights.Damage;
            if (damage == null)
                return;

            damage.EffectGenerationType = EffectGenerationType.MaxEffects;
            damage.Generated = false;
            damage.GenerateDamages();
        }
    }
}
