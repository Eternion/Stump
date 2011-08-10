using System;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Handlers.Effects
{
    [Serializable]
    public class EffectInteger : EffectBase
    {
        protected short m_value;

        public EffectInteger(short id, short value)
            : base(id)
        {
            m_value = value;
        }

        public EffectInteger(EffectInstanceInteger effect)
            : base((short)effect.effectId)
        {
            m_value = (short) effect.value;
        }

        public override int ProtocoleId
        {
            get { return 70; }
        }

        public short Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public override object[] GetValues()
        {
            return new object[] {Value};
        }

        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectInteger(Id, Value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EffectInteger))
                return false;
            return base.Equals(obj) && m_value == (obj as EffectInteger).m_value;
        }

        public static bool operator ==(EffectInteger a, EffectInteger b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object) a == null) || ((object) b == null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EffectInteger a, EffectInteger b)
        {
            return !(a == b);
        }

        public bool Equals(EffectInteger other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && other.m_value == m_value;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ m_value;
            }
        }
    }
}