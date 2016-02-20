using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using System;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class InvisibilityBuff : Buff
    {
        public InvisibilityBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, FightDispellableEnum dispelable)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
        }

        public InvisibilityBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, int priority, FightDispellableEnum dispelable, short customActionId)
            : base(id, target, caster, effect, spell, critical, dispelable, priority, customActionId)
        {
        }

        public override void Apply()
        {
            Target.SetInvisibilityState(GameActionFightInvisibilityStateEnum.INVISIBLE);
            base.Apply();
        }

        public override void Dispell()
        {
            base.Dispell();
            Target.SetInvisibilityState(GameActionFightInvisibilityStateEnum.VISIBLE);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            if (Delay == 0)
                return new FightTemporaryBoostEffect(Id, Target.Id, Duration, (sbyte)Dispellable, (short)Spell.Id, Effect.Id, 0, 1);

            var values = Effect.GetValues();

            return new FightTriggeredEffect(Id, Target.Id, Duration,
                (sbyte)Dispellable,
                (short)Spell.Id, Effect.Id, 0,
                (values.Length > 0 ? Convert.ToInt32(values[0]) : 0),
                (values.Length > 1 ? Convert.ToInt32(values[1]) : 0),
                (values.Length > 2 ? Convert.ToInt32(values[2]) : 0),
                Delay);
        }
    }
}