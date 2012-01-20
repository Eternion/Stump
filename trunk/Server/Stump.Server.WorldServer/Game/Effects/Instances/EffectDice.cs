using System;
using Stump.Core.Threading;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Game.Effects.Instances
{
    [Serializable]
    public class EffectDice : EffectInteger
    {
        protected short m_diceface;
        protected short m_dicenum;

        public EffectDice()
        {
            
        }

        public EffectDice(short id, short value, short dicenum, short diceface)
            : base(id, value)
        {
            m_dicenum = dicenum;
            m_diceface = diceface;
        }

        public EffectDice(EffectInstanceDice effect)
            : base(effect)
        {
            m_dicenum = (short) effect.diceNum;
            m_diceface = (short) effect.diceSide;
        }

        public override int ProtocoleId
        {
            get { return 73; }
        }

        public override byte SerializationIdenfitier
        {
            get
            {
                return 4;
            }
        }

        public short DiceNum
        {
            get { return m_dicenum; }
        }

        public short DiceFace
        {
            get { return m_diceface; }
        }

        public override object[] GetValues()
        {
            return new object[] {DiceNum, DiceFace, Value};
        }

        public override ObjectEffect GetObjectEffect()
        {
            return new ObjectEffectDice(Id, DiceNum, DiceFace, Value);
        }

        public override EffectBase GenerateEffect(EffectGenerationContext context)
        {
            if (context == EffectGenerationContext.Spell || EffectManager.Instance.IsEffectRandomable(EffectId))
            {
                var rand = new AsyncRandom();

                var max = m_dicenum >= m_diceface ? m_dicenum : m_diceface;
                var min = m_dicenum <= m_diceface ? m_dicenum : m_diceface;

                if (min == 0)
                    return new EffectInteger(Id, max);

                return new EffectInteger(Id, (short)rand.Next(min, max + 1));
            }

            return this;
        }

        protected override void InternalSerialize(ref System.IO.BinaryWriter writer)
        {
            base.InternalSerialize(ref writer);

            writer.Write(DiceNum);
            writer.Write(DiceFace);
        }

        protected override void InternalDeserialize(ref System.IO.BinaryReader reader)
        {
            base.InternalDeserialize(ref reader);

            m_dicenum = reader.ReadInt16();
            m_diceface = reader.ReadInt16();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EffectDice))
                return false;
            var b = obj as EffectDice;
            return base.Equals(obj) && m_diceface == b.m_diceface && m_dicenum == b.m_dicenum;
        }

        public static bool operator ==(EffectDice a, EffectDice b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object) a == null) || ((object) b == null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EffectDice a, EffectDice b)
        {
            return !(a == b);
        }

        public bool Equals(EffectDice other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && other.m_diceface == m_diceface && other.m_dicenum == m_dicenum;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ m_diceface;
                result = (result*397) ^ m_dicenum;
                return result;
            }
        }
    }
}