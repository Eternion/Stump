using System.Collections.Generic;
using System.Timers;

namespace Stump.Core.Collections
{
    /// <summary>
    ///   Dictionary that contains timed data using a lifecycle
    /// </summary>
    /// <typeparam name = "TKey">The type of the key.</typeparam>
    /// <typeparam name = "TData">The type of the data.</typeparam>
    public sealed class TimedDictionary<TKey, TData> : Dictionary<TKey, TData>
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