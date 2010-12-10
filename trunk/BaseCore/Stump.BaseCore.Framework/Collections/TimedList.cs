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
using System.Collections;
using System.Collections.Generic;
using System.Timers;

namespace Stump.BaseCore.Framework.Collections
{
    public sealed class TimedList<TData> : IEnumerable<TData>
    {
        #region Properties

        private readonly Dictionary<Timer, TData> m_dictionary = new Dictionary<Timer, TData>();
        private readonly int m_time;

        #endregion

        #region Initialisation

        public TimedList(int time)
        {
            m_time = time;
        }

        #endregion

        #region Add

        public void Add(TData element)
        {
            var timer = new Timer(m_time) {AutoReset = false};
            timer.Elapsed += Timer_Elapsed;
            m_dictionary.Add(timer, element);
            timer.Start();
        }

        #endregion

        #region TimeElapsed

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var timer = sender as Timer;
            m_dictionary.Remove(timer);
            timer.Dispose();
            timer = null;
        }

        #endregion

        #region IEnumerable Implementation

        public IEnumerator<TData> GetEnumerator()
        {
            return m_dictionary.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}