using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class ResistancesDebuff : Buff
    {
        public ResistancesDebuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, short value, bool critical, bool dispelable)
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
            Target.Stats[CaracteristicsEnum.AirResistPercent].Context -= Value;
            Target.Stats[CaracteristicsEnum.FireResistPercent].Context -= Value;
            Target.Stats[CaracteristicsEnum.EarthResistPercent].Context -= Value;
            Target.Stats[CaracteristicsEnum.NeutralResistPercent].Context -= Value;
            Target.Stats[CaracteristicsEnum.WaterResistPercent].Context -= Value;
        }

        public override void Dispell()
        {
            Target.Stats[CaracteristicsEnum.AirResistPercent].Context += Value;
            Target.Stats[CaracteristicsEnum.FireResistPercent].Context += Value;
            Target.Stats[CaracteristicsEnum.EarthResistPercent].Context += Value;
            Target.Stats[CaracteristicsEnum.NeutralResistPercent].Context += Value;
            Target.Stats[CaracteristicsEnum.WaterResistPercent].Context += Value;
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            return new FightTemporaryBoostEffect(Id, Target.Id, Duration, (sbyte) (Dispelable ? 0 : 1), (short) Spell.Id, 0, Value);
        }    
    }
}