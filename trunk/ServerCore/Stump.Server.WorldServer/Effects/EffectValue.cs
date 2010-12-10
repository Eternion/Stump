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
using EffectValueEx = Stump.DofusProtocol.D2oClasses.EffectInstanceInteger;


namespace Stump.Server.WorldServer.Effects
{
    [Serializable]
    public class EffectValue : EffectBase
    {
        protected int m_value;

        public EffectValue(int id, int value)
            : base(id)
        {
            m_value = value;
        }

        public EffectValue(EffectValueEx effect)
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

        public override bool Equals(object obj)
        {
            if (!(obj is EffectValue))
                return false;
            return base.Equals(obj) && m_value == (obj as EffectValue).m_value;
        }

        public static bool operator ==(EffectValue a, EffectValue b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object) a == null) || ((object) b == null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EffectValue a, EffectValue b)
        {
            return !(a == b);
        }

        public bool Equals(EffectValue other)
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