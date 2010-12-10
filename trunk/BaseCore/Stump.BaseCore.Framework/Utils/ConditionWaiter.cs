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
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Stump.BaseCore.Framework.Utils
{
    public class ConditionWaiter
    {
        private readonly Func<bool> m_predicate;
        private readonly int m_timeout;
        private readonly Timer m_timer;
        private DateTime m_startTime;

        public ConditionWaiter(Func<bool> predicate, int timeout)
            : this(predicate, timeout, 100)
        {
        }

        public ConditionWaiter(Func<bool> predicate, int timeout, int interval)
        {
            m_predicate = predicate;
            m_timeout = timeout;
            Interval = interval;

            m_timer = new Timer(interval);
            m_timer.Elapsed += timer_Elapsed;
        }

        public Func<bool> Predicate
        {
            get { return m_predicate; }
        }

        public int Interval
        {
            get;
            set;
        }

        public int Timeout
        {
            get { return m_timeout; }
        }

        public event EventHandler Success;
        public event EventHandler Fail;

        public void Start()
        {
            if (m_startTime != default(DateTime))
                m_startTime = DateTime.Now;

            m_timer.Start();
        }

        public void Stop()
        {
            m_timer.Stop();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (m_timeout != System.Threading.Timeout.Infinite && (DateTime.Now - m_startTime).TotalMilliseconds > m_timeout)
            {
                m_timer.Stop();

                if (Fail != null)
                {
                    Fail(this, new EventArgs());
                }
            }

            if (m_predicate())
            {
                m_timer.Stop();

                if (Success != null)
                {
                    Success(this, new EventArgs());
                }
            }
        }

        public static bool WaitFor(Func<bool> predicate, int timeout)
        {
            DateTime startTime = DateTime.Now;

            do
            {
                if (predicate())
                    return true;

                Thread.Sleep(100);
            } while ((DateTime.Now - startTime).TotalMilliseconds < timeout);

            return false;
        }

        public static bool WaitFor(Func<bool> predicate, int timeout, int interval)
        {
            DateTime startTime = DateTime.Now;

            do
            {
                if (predicate())
                    return true;

                Thread.Sleep(interval);
            } while (( DateTime.Now - startTime ).TotalMilliseconds < timeout || timeout == System.Threading.Timeout.Infinite);

            return false;
        }
    }

    public class ConditionWaiter<T> where T : class
    {
        private readonly object[] m_delegateArgs;
        private readonly Func<bool> m_predicate;

        private readonly int m_timeout;
        private readonly Timer m_timer;

        private T m_delegateAction;
        private DateTime m_startTime;

        static ConditionWaiter()
        {
            if (!typeof (T).IsSubclassOf(typeof (Delegate)))
            {
                throw new InvalidOperationException(typeof (T).Name + " is not a delegate type");
            }
        }

        public ConditionWaiter(Func<bool> predicate, int timeout)
            : this(predicate, timeout, 100, null)
        {
        }

        public ConditionWaiter(Func<bool> predicate, int timeout, int interval)
            : this(predicate, timeout, interval, null)
        {
        }

        public ConditionWaiter(Func<bool> predicate, int timeout, int interval, T action, params object[] args)
        {
            m_predicate = predicate;
            m_timeout = timeout;
            Interval = interval;
            m_delegateAction = action;
            m_delegateArgs = args;

            m_timer = new Timer(interval);
            m_timer.Elapsed += timer_Elapsed;
        }

        public Func<bool> Predicate
        {
            get { return m_predicate; }
        }

        public int Interval
        {
            get;
            set;
        }

        public int Timeout
        {
            get { return m_timeout; }
        }

        public T DelegateAction
        {
            get { return m_delegateAction; }
            set { m_delegateAction = value; }
        }

        public event EventHandler Success;
        public event EventHandler Fail;

        public void Start()
        {
            if (m_startTime != default(DateTime))
                m_startTime = DateTime.Now;

            m_timer.Start();
        }

        public void Stop()
        {
            m_timer.Stop();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (m_timeout != System.Threading.Timeout.Infinite && ( DateTime.Now - m_startTime ).TotalMilliseconds > m_timeout)
            {
                m_timer.Stop();

                if (Fail != null)
                {
                    Fail(this, new EventArgs());
                }
            }

            if (m_predicate())
            {
                m_timer.Stop();

                if (Success != null)
                {
                    Success(this, new EventArgs());
                }

                if (m_delegateAction != null)
                    (m_delegateAction as Delegate).DynamicInvoke(m_delegateArgs);
            }
        }
    }
}