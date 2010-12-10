// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using Stump.BaseCore.Framework.Utils;
using EffectDiceEx = Stump.DofusProtocol.D2oClasses.EffectInstanceDice;


namespace Stump.Server.WorldServer.Effects
{
    [Serializable]
    public class EffectDice : EffectValue
    {
        protected int m_diceface;
        protected int m_dicenum;

        public EffectDice(int id, int value, int dicenum, int diceface)
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

        public int DiceNum
        {
            get { return m_dicenum; }
        }

        public int DiceFace
        {
            get { return m_diceface; }
        }

        public override object[] GetValues()
        {
            return new object[] {(short) DiceNum, (short) DiceFace, (short) Value};
        }

        public override EffectBase GenerateEffect()
        {
            var random = new AsyncRandom();
            int result = 0;

            for (int i = 0; i < m_dicenum; i++)
            {
                result += random.NextInt(1, m_diceface + 2);
            }

            return new EffectValue(m_id, result);
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