using System;

namespace Stump.Core.Timers
{
    /// <summary>
    /// Lightweight timer object that supports one-shot or repeated firing.
    /// </summary>
    /// <remarks>This timer is not standalone, and must be driven via the Update method.
    /// Source : WCell</remarks>
    public class TimerEntry : IDisposable
    {
        private int m_millisSinceLastTick;
        private int m_millisBeforeInit;

        private int m_initialDelay;

        public int InitialDelay
        {
            get { return m_initialDelay; }
            set
            {
                m_initialDelay = value; 
                m_millisBeforeInit = value;
            }
        }

        public int IntervalDelay
        {
            get;
            set;
        }

        public Action<int> Action;

        public TimerEntry()
        {
        }

        /// <summary>
        /// Creates a new timer with the given start delay, interval, and callback.
        /// </summary>
        /// <param name="delay">the delay before firing initially</param>
        /// <param name="intervalDelay">the interval between firing</param>
        /// <param name="callback">the callback to fire</param>
        public TimerEntry(int delay, int intervalDelay, Action<int> callback)
        {
            m_millisSinceLastTick = -1;
            Action = callback;
            InitialDelay = delay;
            IntervalDelay = intervalDelay;
        }

        public TimerEntry(Action<int> callback)
            : this(0, 0, callback)
        {
        }

        /// <summary>
        /// The amount of time in milliseconds that elapsed between the last timer tick and the last update.
        /// </summary>
        public int MillisSinceLastTick
        {
            get
            {
                return m_millisSinceLastTick;
            }
        }

        public int RemainingTime
        {
            get
            {
                return m_millisBeforeInit > 0 ? m_millisBeforeInit : IntervalDelay - m_millisSinceLastTick;
            }
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            m_millisSinceLastTick = 0;
        }

        /// <summary>
        /// Starts the timer with the given delay.
        /// </summary>
        /// <param name="initialDelay">the delay before firing initially</param>
        public void Start(int initialDelay)
        {
            InitialDelay = initialDelay;
            m_millisSinceLastTick = 0;
        }

        /// <summary>
        /// Starts the time with the given delay and interval.
        /// </summary>
        /// <param name="initialDelay">the delay before firing initially</param>
        /// <param name="interval">the interval between firing</param>
        public void Start(int initialDelay, int interval)
        {
            InitialDelay = initialDelay;
            IntervalDelay = interval;
            m_millisSinceLastTick = 0;
        }

        /// <summary>
        /// Whether or not the timer is running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return m_millisSinceLastTick >= 0;
            }
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop()
        {
            m_millisSinceLastTick = -1;
        }

        /// <summary>
        /// Updates the timer, firing the callback if enough time has elapsed.
        /// </summary>
        /// <param name="dtMillis">the time change since the last update</param>
        public void Update(int dtMillis)
        {
            // means this timer is not running.
            if (m_millisSinceLastTick == -1)
                return;

            if (m_millisBeforeInit > 0)
            {
                m_millisBeforeInit -= dtMillis;

                if (m_millisBeforeInit <= 0)
                {
                    if (IntervalDelay == 0)
                    {
                        // we need to stop the timer if it's only
                        // supposed to fire once.
                        var millis = m_millisSinceLastTick;
                        Stop();
                        Action(millis);
                    }
                    else
                    {
                        Action(m_millisSinceLastTick);
                        m_millisSinceLastTick = 0;
                    }
                }
            }
            else
            {
                // update our idle time
                m_millisSinceLastTick += dtMillis;

                if (m_millisSinceLastTick >= IntervalDelay)
                {
                    // time to tick
                    Action(m_millisSinceLastTick);
                    if (m_millisSinceLastTick != -1)
                    {
                        m_millisSinceLastTick -= IntervalDelay;
                    }
                }
            }
        }

        /// <summary>
        /// Stops and cleans up the timer.
        /// </summary>
        public void Dispose()
        {
            Stop();
            Action = null;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(TimerEntry)) return false;
            return Equals((TimerEntry)obj);
        }

        public bool Equals(TimerEntry obj)
        {
            // needs to be improved
            return obj.IntervalDelay == IntervalDelay && Equals(obj.Action, Action);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var result = IntervalDelay * 397 ^ ( Action != null ? Action.GetHashCode() : 0 );
                return result;
            }
        }
    }
}