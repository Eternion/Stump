using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_MinimizeEffects)]
    public class MinimizeEffects : SpellEffectHandler
    {
        public MinimizeEffects(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var buffId = actor.PopNextBuffId();

                var buff = new TriggerBuff(buffId, actor, Caster, Dice, Spell, false, false,
                    BuffTriggerType.BEFORE_ATTACK, MinimizeEffectsBuffTrigger);

                actor.AddAndApplyBuff(buff);
            }

            return true;
        }

        private static void MinimizeEffectsBuffTrigger(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var damage = token as Fights.Damage;
            if (damage == null)
                return;

            damage.EffectGenerationType = EffectGenerationType.MinEffects;
            damage.Generated = false;
            damage.GenerateDamages();
        }
    }
}
