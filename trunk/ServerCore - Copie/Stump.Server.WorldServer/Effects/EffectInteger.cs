
using System;
using Stump.DofusProtocol.Classes;
using EffectValueEx = Stump.DofusProtocol.D2oClasses.EffectInstanceInteger;


namespace Stump.Server.WorldServer.Effects
{
    [Serializable]
    public class EffectInteger : EffectBase
    {
        protected int m_value;

        public EffectInteger(uint id, int value)
            : base(id)
        {
            m_value = value;
        }

        public EffectInteger(EffectValueEx effect)
            : base(effect.effectId)
        {
            m_value = effect.value;
        }

        public override int ProtocoleId
        {
            get { return 70; }
        }

        public int Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public override object[] GetValues()
        {
            return new object[] {(short) Value};
        }

        public override ObjectEffect ToNetworkEffect()
        {
            return new ObjectEffectInteger((uint)EffectId, (uint) Value);
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