using System;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Worlds.Effects.Instances
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
            : base(effect)
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

        public bool Equals(EffectInteger other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && other.m_value == m_value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as EffectInteger);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ m_value.GetHashCode();
            }
        }

        public static bool operator ==(EffectInteger left, EffectInteger right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EffectInteger left, EffectInteger right)
        {
            return !Equals(left, right);
        }
    }
}