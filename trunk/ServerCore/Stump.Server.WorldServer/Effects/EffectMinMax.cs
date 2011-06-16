
using System;
using Stump.Core.Threading;
using Stump.DofusProtocol.Classes;
using EffectMaxEx = Stump.DofusProtocol.D2oClasses.EffectInstanceMinMax;


namespace Stump.Server.WorldServer.Effects
{
    [Serializable]
    public class EffectMinMax : EffectBase
    {
        protected uint m_maxvalue;
        protected uint m_minvalue;

        public EffectMinMax(uint id, uint valuemin, uint valuemax)
            : base(id)
        {
            m_minvalue = valuemin;
            m_maxvalue = valuemax;
        }

        public EffectMinMax(EffectMaxEx effect)
            : base(effect.effectId)
        {
            m_maxvalue = effect.max;
            m_minvalue = effect.min;
        }

        public override int ProtocoleId
        {
            get { return 82; }
        }

        public uint ValueMin
        {
            get { return m_minvalue; }
            set { m_minvalue = value; }
        }

        public uint ValueMax
        {
            get { return m_maxvalue; }
            set { m_maxvalue = value; }
        }

        public override object[] GetValues()
        {
            return new object[] {(short) ValueMin, (short) ValueMax};
        }

        public override ObjectEffect ToNetworkEffect()
        {
            return new ObjectEffectMinMax((uint)EffectId, ValueMin, ValueMax);
        }

        public override EffectBase GenerateEffect(EffectGenerationContext context)
        {
            if (context == EffectGenerationContext.Spell || EffectManager.IsEffectRandomable(EffectId))
            {
                var rand = new AsyncRandom();

                return new EffectInteger(m_id, rand.NextInt((int) ValueMin, (int) ValueMax + 1));
            }

            return this;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EffectMinMax))
                return false;
            var b = obj as EffectMinMax;
            return base.Equals(obj) && m_minvalue == b.m_minvalue && m_maxvalue == b.m_maxvalue;
        }

        public static bool operator ==(EffectMinMax a, EffectMinMax b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object) a == null) || ((object) b == null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EffectMinMax a, EffectMinMax b)
        {
            return !(a == b);
        }

        public bool Equals(EffectMinMax other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && other.m_maxvalue == m_maxvalue && other.m_minvalue == m_minvalue;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ m_maxvalue.GetHashCode();
                result = (result*397) ^ m_minvalue.GetHashCode();
                return result;
            }
        }
    }
}