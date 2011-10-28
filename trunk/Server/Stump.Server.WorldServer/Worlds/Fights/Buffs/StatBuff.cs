using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Fights.Buffs
{
    public class StatBuff : Buff
    {
        public StatBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, short value, CaracteristicsEnum caracteristic, bool critical, bool dispelable)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
            Value = value;
            Caracteristic = caracteristic;
        }

        public StatBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, short value, CaracteristicsEnum caracteristic, bool critical, bool dispelable, short customActionId)
            : base(id, target, caster, effect, spell, critical, dispelable, customActionId)
        {
            Value = value;
            Caracteristic = caracteristic;
        }

        public short Value
        {
            get;
            private set;
        }

        public CaracteristicsEnum Caracteristic
        {
            get;
            set;
        }

        public override void Apply()
        {
            Target.Stats[Caracteristic].Context += Value;
        }

        public override void Remove()
        {
            Target.Stats[Caracteristic].Context -= Value;
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            return new FightTemporaryBoostEffect(Id, Target.Id, Duration, (sbyte) (Dispelable ? 0 : 1), (short) Spell.Id, 0, (short)System.Math.Abs(Value));
        }
    }
}