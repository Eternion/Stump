using System;
using Stump.Core.Threading;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Worlds.Effects.Instances
{
    [Serializable]
    public class EffectDice : EffectInteger
    {
        protected short m_diceface;
        protected short m_dicenum;

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
            /*if (context == EffectGenerationContext.Spell || EffectManager.Instance.IsEffectRandomable(EffectId))
            {
                var random = new AsyncRandom();
                short result = 0;

                for (int i = 0; i < m_dicenum; i++)
                {
                    result += (short)random.Next(1, m_diceface + 2);
                }

                return new EffectInteger(Id, result);
            }*/
            if (context == EffectGenerationContext.Spell || EffectManager.Instance.IsEffectRandomable(EffectId))
            {
                var rand = new AsyncRandom();

                return new EffectInteger(Id, (short)rand.Next(m_dicenum, m_diceface + 1));
            }

            return this;
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