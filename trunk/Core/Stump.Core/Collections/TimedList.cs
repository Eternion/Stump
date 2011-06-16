using System.Collections;
using System.Collections.Generic;
using System.Timers;

namespace Stump.Core.Collections
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