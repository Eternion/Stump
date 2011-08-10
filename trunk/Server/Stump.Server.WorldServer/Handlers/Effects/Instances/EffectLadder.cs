using System;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Handlers.Effects
{
    [Serializable]
    public class EffectLadder : EffectCreature
    {
        protected short m_monsterCount;

        public short MonsterCount
        {
            get { return m_monsterCount; }
            set { m_monsterCount = value; }
        }

        public EffectLadder(short id, short monsterfamily, short monstercount)
            : base(id, monsterfamily)
        {
            m_monsterCount = monstercount;
        }

        public EffectLadder(EffectInstanceLadder effect)
            : base((short)effect.effectId, (short)effect.monsterFamilyId)
        {
            m_monsterCount = (short) effect.monsterCount;
        }

        public override int ProtocoleId
        {
            get { return 81; }
        }

        public override object[] GetValues()
        {
            return new object[] {m_monsterCount};
        }

        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectLadder(Id, MonsterFamily, MonsterCount);
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