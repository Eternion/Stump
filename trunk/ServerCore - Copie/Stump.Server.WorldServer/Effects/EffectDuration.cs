
using System;
using Stump.DofusProtocol.Classes;
using EffectDurationEx = Stump.DofusProtocol.D2oClasses.EffectInstanceDuration;


namespace Stump.Server.WorldServer.Effects
{
    [Serializable]
    public class EffectDuration : EffectBase
    {
        protected uint m_days;
        protected uint m_hours;
        protected uint m_minutes;

        public EffectDuration(uint id, uint days, uint hours, uint minutes)
            : base(id)
        {
            m_days = days;
            m_hours = hours;
            m_minutes = minutes;
        }

        public EffectDuration(EffectDurationEx effect)
            : base(effect.effectId)
        {
            m_days = effect.days;
            m_hours = effect.hours;
            m_minutes = effect.minutes;
        }


        public override int ProtocoleId
        {
            get { return 75; }
        }

        public override object[] GetValues()
        {
            return new object[] {(short) m_days, (short) m_hours, (short) m_minutes};
        }

        public override ObjectEffect ToNetworkEffect()
        {
            return new ObjectEffectDuration((uint) EffectId, m_days, m_hours, m_minutes);
        }

        public TimeSpan GetTimeSpan()
        {
            return new TimeSpan((int) m_days, (int) m_hours, (int) m_minutes, 0);
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