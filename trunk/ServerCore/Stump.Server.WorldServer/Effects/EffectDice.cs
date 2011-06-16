
using System;
using Stump.Core.Threading;
using Stump.DofusProtocol.Classes;
using EffectDiceEx = Stump.DofusProtocol.D2oClasses.EffectInstanceDice;


namespace Stump.Server.WorldServer.Effects
{
    [Serializable]
    public class EffectDice : EffectInteger
    {
        protected uint m_diceface;
        protected uint m_dicenum;

        public EffectDice(uint id, int value, uint dicenum, uint diceface)
            : base(id, value)
        {
            m_dicenum = dicenum;
            m_diceface = diceface;
        }

        public EffectDice(EffectDiceEx effect)
            : base(effect.effectId, effect.value)
        {
            m_dicenum = effect.diceNum;
            m_diceface = effect.diceSide;
        }

        public override int ProtocoleId
        {
            get { return 73; }
        }

        public uint DiceNum
        {
            get { return m_dicenum; }
        }

        public uint DiceFace
        {
            get { return m_diceface; }
        }

        public override object[] GetValues()
        {
            return new object[] {(short) DiceNum, (short) DiceFace, (short) Value};
        }

        public override ObjectEffect ToNetworkEffect()
        {
            return new ObjectEffectDice((uint)EffectId, DiceNum, DiceFace, (uint) Value);
        }

        public override EffectBase GenerateEffect(EffectGenerationContext context)
        {
            if (context == EffectGenerationContext.Spell || EffectManager.IsEffectRandomable(EffectId))
            {
                var random = new AsyncRandom();
                int result = 0;

                for (int i = 0; i < m_dicenum; i++)
                {
                    result += random.NextInt(1, (int) (m_diceface + 2));
                }

                return new EffectInteger(m_id, result);
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
                result = (int) ((result*397) ^ m_diceface);
                result = (int) ((result*397) ^ m_dicenum);
                return result;
            }
        }
    }
}