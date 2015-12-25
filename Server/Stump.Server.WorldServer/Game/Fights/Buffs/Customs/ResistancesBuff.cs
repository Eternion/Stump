using System;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class ResistancesBuff : Buff
    {
        public ResistancesBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, short value, bool critical, bool dispelable)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
            Value = value;
        }

        public short Value
        {
            get;
            private set;
        }

        public override void Apply(BuffTriggerType type, object token)
        {
            Target.Stats[PlayerFields.AirResistPercent].Context += Value;
            Target.Stats[PlayerFields.FireResistPercent].Context += Value;
            Target.Stats[PlayerFields.EarthResistPercent].Context += Value;
            Target.Stats[PlayerFields.NeutralResistPercent].Context += Value;
            Target.Stats[PlayerFields.WaterResistPercent].Context += Value;
        }

        public override void Dispell()
        {
            Target.Stats[PlayerFields.AirResistPercent].Context -= Value;
            Target.Stats[PlayerFields.FireResistPercent].Context -= Value;
            Target.Stats[PlayerFields.EarthResistPercent].Context -= Value;
            Target.Stats[PlayerFields.NeutralResistPercent].Context -= Value;
            Target.Stats[PlayerFields.WaterResistPercent].Context -= Value;
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            return new FightTemporaryBoostEffect(Id, Target.Id, Duration, (sbyte) (Dispellable ? 0 : 1), (short) Spell.Id, Effect.Id, 0, Math.Abs(Value));
        }    
    }
}