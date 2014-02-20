using System;
using NLog;
using Stump.Core.Threading;

namespace Stump.Core.Pool.Task
{
    public class CyclicTask
    {
        #region Delegates

        public delegate bool Predicate();

        #endregion

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public event Action<CyclicTask> TaskEnded;

        private readonly uint? m_callsLimit;
        private readonly Predicate m_condition;
        private readonly TimeSpan m_intervalDelay;
        private readonly TimeSpan m_firstCallDelay;

        private uint m_callsCount;
        private DateTime m_lastCall;

        private bool m_cancel;

        public CyclicTask(Action action, TimeSpan intervalDelay)

        {
            Action = action;
            m_intervalDelay = intervalDelay;
            m_lastCall = DateTime.Now;
        }

        public CyclicTask(Action action, TimeSpan intervalDelay, TimeSpan firstCallDelay)

        {
            Action = action;
            m_intervalDelay = intervalDelay;
            m_firstCallDelay = firstCallDelay;
            m_lastCall = intervalDelay > firstCallDelay ? DateTime.Now - (intervalDelay - firstCallDelay) : DateTime.Now;
        }

        public CyclicTask(Action action, TimeSpan intervalDelay, Predicate condition, uint? callsLimit)

        {
            Action = action;
            m_intervalDelay = intervalDelay;
            m_condition = condition;
            m_callsLimit = callsLimit;
            m_lastCall = DateTime.Now;
        }

        public CyclicTask(Action action, TimeSpan intervalDelay, TimeSpan firstCallDelay, Predicate condition, uint? callsLimit)

        {
            Action = action;
            m_intervalDelay = intervalDelay;
            m_firstCallDelay = firstCallDelay;
            m_condition = condition;
            m_callsLimit = callsLimit;
            m_lastCall = intervalDelay > firstCallDelay ? DateTime.Now - (intervalDelay - firstCallDelay) : DateTime.Now;
        }

        public Action Action
        {
            get;
            private set;
        }

        public bool RequireExecution
        {
            get { return ReachDelay && SuitCondition && Alive; }
        }

        private bool SuitCondition
        {
            get { return m_condition == null || m_condition(); }
        }

        private bool ReachDelay
        {
            get { return DateTime.Now.Subtract(m_lastCall) >= m_intervalDelay; }
        }

        public bool Alive
        {
            get
            {
                return ( !m_callsLimit.HasValue || m_callsCount < m_callsLimit ) && !m_cancel;
            }
        }

        internal void Start()
        {
            Compute();
        }

        internal void Stop()
        {
            m_cancel = true;
        }

        private void Compute()
        {
            var delay = (int) (m_callsCount == 0 && m_firstCallDelay != default(TimeSpan) ?
                m_firstCallDelay.TotalMilliseconds : m_intervalDelay.TotalMilliseconds);

            System.Threading.Tasks.Task.Factory.StartNewDelayed(delay,
            delegate
            {
                if (RequireExecution)
                {
                    try
                    {
                        Action();
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Exception occurs on cylictask {0} : {1}", Action.Method.Name, ex);
                    }

                    m_lastCall = DateTime.Now;
                    m_callsCount++;
                }

                if (Alive)
                    Compute();
                else
                {
                    if (TaskEnded != null)
                        TaskEnded(this);
                }
            });
        }
    }
}