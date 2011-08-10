using System;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Handlers.Effects
{
    [Serializable]
    public class EffectString : EffectBase
    {
        protected string m_value;

        public EffectString(short id, string value)
            : base(id)
        {
            m_value = value;
        }

        public EffectString(EffectInstanceString effect)
            : base((short)effect.effectId)
        {
            m_value = effect.text;
        }

        public override int ProtocoleId
        {
            get { return 74; }
        }

        public override object[] GetValues()
        {
            return new object[] {m_value};
        }

        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectString(Id, m_value);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EffectString))
                return false;
            return base.Equals(obj) && m_value == (obj as EffectString).m_value;
        }

        public static bool operator ==(EffectString a, EffectString b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object) a == null) || ((object) b == null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EffectString a, EffectString b)
        {
            return !(a == b);
        }

        public bool Equals(EffectString other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && Equals(other.m_value, m_value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ (m_value != null ? m_value.GetHashCode() : 0);
            }
        }
    }
}