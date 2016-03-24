using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class ResistancesBuff : Buff
    {
        public ResistancesBuff(int id, FightActor target, FightActor caster, SpellEffectHandler effect, Spell spell, short value, bool critical, FightDispellableEnum dispelable)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
            Value = value;
        }

        public short Value
        {
            get;
            private set;
        }

        public override void Apply()
        {
            base.Apply();
            Target.Stats[PlayerFields.AirResistPercent].Context += Value;
            Target.Stats[PlayerFields.FireResistPercent].Context += Value;
            Target.Stats[PlayerFields.EarthResistPercent].Context += Value;
            Target.Stats[PlayerFields.NeutralResistPercent].Context += Value;
            Target.Stats[PlayerFields.WaterResistPercent].Context += Value;
        }

        public override void Dispell()
        {
            base.Dispell();
            Target.Stats[PlayerFields.AirResistPercent].Context -= Value;
            Target.Stats[PlayerFields.FireResistPercent].Context -= Value;
            Target.Stats[PlayerFields.EarthResistPercent].Context -= Value;
            Target.Stats[PlayerFields.NeutralResistPercent].Context -= Value;
            Target.Stats[PlayerFields.WaterResistPercent].Context -= Value;
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            if (Delay == 0)
                return new FightTemporaryBoostEffect(Id, Target.Id, Duration, (sbyte)Dispellable, (short)Spell.Id, Effect.Id, 0, Math.Abs(Value));

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