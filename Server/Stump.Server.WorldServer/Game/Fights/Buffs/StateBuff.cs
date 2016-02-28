using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using System;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    public class StateBuff : Buff
    {
        public StateBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, FightDispellableEnum dispelable, SpellState state)
            : base(id, target, caster, effect, spell, false, dispelable)
        {
            State = state;
        }

        public StateBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, FightDispellableEnum dispelable, int priority, short customActionId, SpellState state)
            : base(id, target, caster, effect, spell, false, dispelable, priority, customActionId)
        {
            State = state;
        }

        public SpellState State
        {
            get;
        }

        public bool IsDisabled
        {
            get;
            set;
        }

        public override void Apply()
        {
            base.Apply();

            Target.TriggerBuffs(Target, BuffTriggerType.OnStateAdded);
            Target.TriggerBuffs(Target, BuffTriggerType.OnSpecificStateAdded, State.Id);
        }

        public override void Dispell()
        {
            base.Dispell();

            Target.TriggerBuffs(Target, BuffTriggerType.OnStateRemoved);
            Target.TriggerBuffs(Target, BuffTriggerType.OnSpecificStateRemoved, State.Id);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            if (Delay == 0)
                return new FightTemporaryBoostStateEffect(Id, Target.Id, Duration, (sbyte)Dispellable, (short)Spell.Id, Effect.Id, 0, 1, (short)State.Id);

            var values = Effect.GetValues();

            return new FightTriggeredEffect(Id, Target.Id, Delay,
                (sbyte)Dispellable,
                (short)Spell.Id, Effect.Id, 0,
                (values.Length > 0 ? Convert.ToInt32(values[0]) : 0),
                (values.Length > 1 ? Convert.ToInt32(values[1]) : 0),
                (values.Length > 2 ? Convert.ToInt32(values[2]) : 0),
                Delay);
        }
    }
}