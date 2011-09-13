using System;
using Stump.Core.Threading;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Worlds.Effects.Instances
{
    [Serializable]
    public class EffectMinMax : EffectBase
    {
        protected short m_maxvalue;
        protected short m_minvalue;

        public EffectMinMax(short id, short valuemin, short valuemax)
            : base(id)
        {
            m_minvalue = valuemin;
            m_maxvalue = valuemax;
        }

        public EffectMinMax(EffectInstanceMinMax effect)
            : base(effect)
        {
            m_maxvalue = (short) effect.max;
            m_minvalue = (short) effect.min;
        }

        public override int ProtocoleId
        {
            get { return 82; }
        }

        public short ValueMin
        {
            get { return m_minvalue; }
            set { m_minvalue = value; }
        }

        public short ValueMax
        {
            get { return m_maxvalue; }
            set { m_maxvalue = value; }
        }

        public override object[] GetValues()
        {
            return new object[] {ValueMin, ValueMax};
        }

        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectMinMax(Id, ValueMin, ValueMax);
        }

        public override EffectBase GenerateEffect(EffectGenerationContext context)
        {
            if (context == EffectGenerationContext.Spell || EffectManager.Instance.IsEffectRandomable(EffectId))
            {
                var rand = new AsyncRandom();

                return new EffectInteger(Id, (short) rand.Next(ValueMin,  ValueMax + 1));
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