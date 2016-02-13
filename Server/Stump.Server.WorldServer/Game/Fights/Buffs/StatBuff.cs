using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    public class StatBuff : Buff
    {
        public StatBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, short value, PlayerFields caracteristic, bool critical, bool dispelable)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
            Value = value;
            Caracteristic = caracteristic;
        }

        public StatBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, short value, PlayerFields caracteristic, bool critical, bool dispelable, int priority, short? customActionId)
            : base(id, target, caster, effect, spell, critical, dispelable, priority, customActionId)
        {
            Value = value;
            Caracteristic = caracteristic;
        }

        public short Value
        {
            get;
            set;
        }

        public PlayerFields Caracteristic
        {
            get;
            set;
        }

        public override void Apply()
        {
            base.Apply();
            Target.Stats[Caracteristic].Context += Value;
        }

        public override void Dispell()
        {
            base.Dispell();

            if (!Target.IsAlive())
                return;

            Target.Stats[Caracteristic].Context -= Value;

            Target.CheckDead(Target);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            if (Delay == 0)
                return new FightTemporaryBoostEffect(Id, Target.Id, Duration, (sbyte)(Dispellable ? FightDispellableEnum.DISPELLABLE : FightDispellableEnum.DISPELLABLE_BY_DEATH), (short)Spell.Id, Effect.Id, 0, Math.Abs(Value));

            var values = Effect.GetValues();
            return new FightTriggeredEffect(Id, Target.Id, (short)(Duration + Delay), (sbyte)(Dispellable ? FightDispellableEnum.DISPELLABLE : FightDispellableEnum.DISPELLABLE_BY_DEATH), (short)Spell.Id, Effect.Id, 0, (short)values[0], (short)values[1], (short)values[2], Delay);
        }
    }
}