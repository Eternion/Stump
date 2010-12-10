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
using EffectDateEx = Stump.DofusProtocol.D2oClasses.EffectInstanceDate;


namespace Stump.Server.WorldServer.Effects
{
    [Serializable]
    public class EffectDate : EffectBase
    {
        protected uint m_day;
        protected uint m_hour;
        protected uint m_minute;
        protected uint m_month;
        protected uint m_year;

        public EffectDate(int id, uint year, uint month, uint day, uint hour, uint minute)
            : base(id)
        {
            m_year = year;
            m_month = month;
            m_day = day;
            m_hour = hour;
            m_minute = minute;
        }

        public EffectDate(EffectDateEx effect)
            : base(effect.effectId)
        {
            m_year = effect.year;
            m_month = effect.month;
            m_day = effect.day;
            m_hour = effect.hour;
            m_minute = effect.minute;
        }


        public override int ProtocoleId
        {
            get { return 72; }
        }

        public override object[] GetValues()
        {
            return new object[]
                {
                    m_year.ToString(), m_month.ToString("00") + m_day.ToString("00"),
                    m_hour.ToString("00") + m_minute.ToString("00")
                };
        }

        public DateTime GetDate()
        {
            return new DateTime((int) m_year, (int) m_month, (int) m_day, (int) m_hour, (int) m_minute, 0);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is EffectDate))
                return false;
            return base.Equals(obj) && GetDate().Equals((obj as EffectDate).GetDate());
        }

        public static bool operator ==(EffectDate a, EffectDate b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (((object) a == null) || ((object) b == null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(EffectDate a, EffectDate b)
        {
            return !(a == b);
        }

        public bool Equals(EffectDate other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && other.m_day == m_day && other.m_hour == m_hour && other.m_minute == m_minute &&
                   other.m_month == m_month && other.m_year == m_year;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ m_day.GetHashCode();
                result = (result*397) ^ m_hour.GetHashCode();
                result = (result*397) ^ m_minute.GetHashCode();
                result = (result*397) ^ m_month.GetHashCode();
                result = (result*397) ^ m_year.GetHashCode();
                return result;
            }
        }
    }
}