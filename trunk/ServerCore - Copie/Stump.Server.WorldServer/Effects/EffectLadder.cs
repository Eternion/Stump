
using System;
using Stump.DofusProtocol.Classes;
using EffectLadderEx = Stump.DofusProtocol.D2oClasses.EffectInstanceLadder;


namespace Stump.Server.WorldServer.Effects
{
    [Serializable]
    public class EffectLadder : EffectCreature
    {
        protected uint m_monsterCount;

        public uint MonsterCount
        {
            get { return m_monsterCount; }
            set { m_monsterCount = value; }
        }

        public EffectLadder(uint id, uint monsterfamily, uint monstercount)
            : base(id, monsterfamily)
        {
            m_monsterCount = monstercount;
        }

        public EffectLadder(EffectLadderEx effect)
            : base(effect.effectId, effect.monsterFamilyId)
        {
            m_monsterCount = effect.monsterCount;
        }

        public override int ProtocoleId
        {
            get { return 81; }
        }

        public override object[] GetValues()
        {
            return new object[] {(short) m_monsterCount};
        }

        public override ObjectEffect ToNetworkEffect()
        {
            return new ObjectEffectLadder((uint)EffectId, MonsterFamily, MonsterCount);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EffectLadder))
                return false;
            return base.Equals(obj) && m_monsterCount == (obj as EffectLadder).m_monsterCount;
        }

        public static bool operator ==(EffectLadder a, EffectLadder b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object) a == null) || ((object) b == null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EffectLadder a, EffectLadder b)
        {
            return !(a == b);
        }

        public bool Equals(EffectLadder other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && other.m_monsterCount == m_monsterCount;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode()*397) ^ m_monsterCount.GetHashCode();
            }
        }
    }
}