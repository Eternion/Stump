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
using System.Collections.Generic;
using System.Timers;

namespace Stump.BaseCore.Framework.Collections
{
    /// <summary>
    ///   Dictionary that contains timed data using a lifecycle
    /// </summary>
    /// <typeparam name = "TKey">The type of the key.</typeparam>
    /// <typeparam name = "TData">The type of the data.</typeparam>
    public sealed class TimedDictionary<TKey, TData>
    {
        #region Properties

        private readonly Dictionary<Timer, KeyValuePair<TKey, TData>> m_dictionary =
            new Dictionary<Timer, KeyValuePair<TKey, TData>>();

        private readonly int m_time;

        #endregion

        #region Initialisation

        /// <summary>
        ///   Initializes a new instance of the <see cref = "TimedDictionary&lt;TKey, TData&gt;" /> class.
        /// </summary>
        /// <param name = "time">The living time.</param>
        public TimedDictionary(int time)
        {
            m_time = time;
        }

        #endregion

        #region Add

        /// <summary>
        ///   Adds the specified key.
        /// </summary>
        /// <param name = "key">The key.</param>
        /// <param name = "element">The element.</param>
        public void Add(TKey key, TData element)
        {
            var timer = new Timer(m_time)
                            {
                                AutoReset = false
                            };
            timer.Elapsed += Timer_Elapsed;
            m_dictionary.Add(timer, new KeyValuePair<TKey, TData>(key, element));
            timer.Start();
        }

        #endregion

        #region Get

        /// <summary>
        ///   Try to get a key from the dictionary
        /// </summary>
        /// <param name = "key">Element's key</param>
        /// <returns>
        ///   Return null if key is not present
        /// </returns>
        public TData TryGet(TKey key)
        {
            foreach (var kvp in m_dictionary.Values)
            {
                if (kvp.Key.Equals(key))
                {
                    return kvp.Value;
                }
            }

            return default(TData);
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
    }
}