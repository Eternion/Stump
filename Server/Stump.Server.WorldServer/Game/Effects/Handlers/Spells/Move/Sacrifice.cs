using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;
using Stump.Server.WorldServer.Handlers.Context;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_DamageIntercept)]
    public class Sacrifice : SpellEffectHandler
    {
        private FightActor m_target;

        public Sacrifice(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            m_target = GetAffectedActors().FirstOrDefault();
            if (m_target == null)
                return true;

            AddTriggerBuff(m_target, false, BuffTriggerType.BEFORE_ATTACKED, TriggerBuffApply);
            AddTriggerBuff(m_target, false, BuffTriggerType.AFTER_ATTACKED, PostTriggerBuffApply);

            return true;
        }

        public void TriggerBuffApply(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            if (m_target.IsSacrificeProtected)
                return;
            m_target.IsSacrificeProtected = true;

            var damage = token as Fights.Damage;
            if (damage == null)
                return;

            Caster.ExchangePositions(m_target);

            // first, apply damage to sacrifier
            Caster.InflictDamage(damage);
            // then, negate damage done to target
            damage.Amount = 0;
        }

        public void PostTriggerBuffApply(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            m_target.IsSacrificeProtected = false;
        }
    }
}