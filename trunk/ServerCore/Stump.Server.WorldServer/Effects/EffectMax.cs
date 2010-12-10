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
using EffectMaxEx = Stump.DofusProtocol.D2oClasses.EffectInstanceMinMax;


namespace Stump.Server.WorldServer.Effects
{
    [Serializable]
    public class EffectMax : EffectBase
    {
        protected uint m_maxvalue;
        protected uint m_minvalue;

        public EffectMax(int id, uint valuemin, uint valuemax)
            : base(id)
        {
            m_minvalue = valuemin;
            m_maxvalue = valuemax;
        }

        public EffectMax(EffectMaxEx effect)
            : base(effect.effectId)
        {
            m_maxvalue = effect.max;
            m_minvalue = effect.min;
        }

        public override int ProtocoleId
        {
            get { return 82; }
        }

        public uint ValueMin
        {
            get { return m_minvalue; }
            set { m_minvalue = value; }
        }

        public uint ValueMax
        {
            get { return m_maxvalue; }
            set { m_maxvalue = value; }
        }

        public override object[] GetValues()
        {
            return new object[] {(short) ValueMin, (short) ValueMax};
        }

        public override EffectBase GenerateEffect()
        {
            if (EffectManager.IsEffectRandomable(EffectId))
            {
                var rand = new AsyncRandom();

                return new EffectValue(m_id, rand.NextInt((int) ValueMin, (int) ValueMax + 1));
            }

            return this;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EffectMax))
                return false;
            var b = obj as EffectMax;
            return base.Equals(obj) && m_minvalue == b.m_minvalue && m_maxvalue == b.m_maxvalue;
        }

        public static bool operator ==(EffectMax a, EffectMax b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object) a == null) || ((object) b == null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EffectMax a, EffectMax b)
        {
            return !(a == b);
        }

        public bool Equals(EffectMax other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && other.m_maxvalue == m_maxvalue && other.m_minvalue == m_minvalue;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ m_maxvalue.GetHashCode();
                result = (result*397) ^ m_minvalue.GetHashCode();
                return result;
            }
        }
    }
}