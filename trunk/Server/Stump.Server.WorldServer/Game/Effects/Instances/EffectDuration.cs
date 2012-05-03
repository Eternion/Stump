using System;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Game.Effects.Instances
{
    [Serializable]
    public class EffectDuration : EffectBase
    {
        protected short m_days;
        protected short m_hours;
        protected short m_minutes;

        public EffectDuration()
        {
            
        }

        public EffectDuration(EffectDuration copy)
            : this(copy.Id, copy.m_days, copy.m_hours, copy.m_minutes, copy)
        {
            
        }

        public EffectDuration(short id, short days, short hours, short minutes, EffectBase effect)
            : base(id, effect)
        {
            m_days = days;
            m_hours = hours;
            m_minutes = minutes;
        }

        public EffectDuration(EffectInstanceDuration effect)
            : base(effect)
        {
            m_days = (short) effect.days;
            m_hours = (short) effect.hours;
            m_minutes = (short) effect.minutes;
        }

        public override int ProtocoleId
        {
            get { return 75; }
        }

        public override byte SerializationIdenfitier
        {
            get
            {
                return 5;
            }
        }

        public override object[] GetValues()
        {
            return new object[] {m_days,  m_hours,  m_minutes};
        }

        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectDuration(Id, m_days, m_hours, m_minutes);
        }
        public override EffectInstance GetEffectInstance()
        {
            return new EffectInstanceDuration()
            {
                effectId = (uint)Id,
                targetId = (int)Targets,
                delay = Delay,
                duration = Duration,
                group = Group,
                random = Random,
                modificator = Modificator,
                trigger = Trigger,
                hidden = Hidden,
                rawZone = RawZone,
                days = (uint) m_days,
                hours = (uint) m_hours,
                minutes = (uint) m_minutes
            };
        }

        public override EffectBase GenerateEffect(EffectGenerationContext context, EffectGenerationType type = EffectGenerationType.Normal)
        {
            return new EffectDuration(this);
        }

        protected override void InternalSerialize(ref System.IO.BinaryWriter writer)
        {
            base.InternalSerialize(ref writer);

            writer.Write(m_days);
            writer.Write(m_hours);
            writer.Write(m_minutes);
        }

        protected override void InternalDeserialize(ref System.IO.BinaryReader reader)
        {
            base.InternalDeserialize(ref reader);

            m_days = reader.ReadInt16();
            m_hours = reader.ReadInt16();
            m_minutes = reader.ReadInt16();
        }

        public TimeSpan GetTimeSpan()
        {
            return new TimeSpan( m_days,  m_hours,  m_minutes, 0);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EffectDuration))
                return false;
            return base.Equals(obj) && GetTimeSpan().Equals((obj as EffectDuration).GetTimeSpan());
        }

        public static bool operator ==(EffectDuration a, EffectDuration b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object) a == null) || ((object) b == null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EffectDuration a, EffectDuration b)
        {
            return !(a == b);
        }

        public bool Equals(EffectDuration other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && other.m_days == m_days && other.m_hours == m_hours &&
                   other.m_minutes == m_minutes;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ m_days.GetHashCode();
                result = (result*397) ^ m_hours.GetHashCode();
                result = (result*397) ^ m_minutes.GetHashCode();
                return result;
            }
        }
    }
}