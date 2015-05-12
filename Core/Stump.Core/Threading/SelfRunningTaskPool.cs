using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stump.Core.Collections;
using Stump.Core.Timers;

namespace Stump.Core.Threading
{
    /// <summary>
    ///     Thank's to WCell project
    /// </summary>
    public class SelfRunningTaskPool : IContextHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly LockFreeQueue<IMessage> m_messageQueue;
        private readonly Stopwatch m_queueTimer;
        private readonly ManualResetEvent m_stoppedAsync = new ManualResetEvent(false);
        private readonly PriorityQueueB<TimedTimerEntry> m_timers = new PriorityQueueB<TimedTimerEntry>(new TimedTimerComparer());
        private readonly List<TimedTimerEntry> m_pausedTimers = new List<TimedTimerEntry>();
        private readonly TimedTimerComparer m_timerComparer = new TimedTimerComparer();

        public readonly TimeSpan TimerTimeout = TimeSpan.FromMinutes(5);

        private int m_currentThreadId;
        private int m_lastUpdate;
        private Task m_updateTask;

        public SelfRunningTaskPool(int interval, string name)
        {
            m_messageQueue = new LockFreeQueue<IMessage>();
            m_queueTimer = Stopwatch.StartNew();
            UpdateInterval = interval;
            Name = name;
        }

        public string Name
        {
            get;
            set;
        }

        public int UpdateInterval
        {
            get;
            set;
        }

        public long LastUpdateTime
        {
            get { return m_lastUpdate; }
        }

        public bool IsRunning
        {
            get;
            protected set;
        }

        public bool IsInContext
        {
            get { return Thread.CurrentThread.ManagedThreadId == m_currentThreadId; }
        }

        public void AddMessage(IMessage message)
        {
            m_messageQueue.Enqueue(message);
        }

        public void AddMessage(Action action)
        {
            m_messageQueue.Enqueue(new Message(action));
        }

        public bool ExecuteInContext(Action action)
        {
            if (IsInContext)
            {
                action();
                return true;
            }

            AddMessage(action);
            return false;
        }

        public void EnsureContext()
        {
            if (!IsInContext)
            {
                throw new InvalidOperationException("Not in context");
            }
        }

        public void Start()
        {
            IsRunning = true;

            m_updateTask = Task.Factory.StartNewDelayed(UpdateInterval, ProcessCallback, this);
        }

        public void Stop()
        {
            Stop(false);
        }

        public void Stop(bool async)
        {
            IsRunning = false;

            if (async && m_currentThreadId != 0)
                m_stoppedAsync.WaitOne();
        }

        public void EnsureNotContext()
        {
            if (IsInContext)
            {
                throw new InvalidOperationException("Forbidden context");
            }
        }

        public void AddTimer(TimedTimerEntry timer)
        {
            ExecuteInContext(() =>
            {
                if (!timer.Enabled)
                    timer.Start();

                m_timers.Push(timer);
            });
        }

        public void RemoveTimer(TimedTimerEntry timer)
        {
            ExecuteInContext(() =>
            {
                m_timers.Remove(timer);
                timer.Dispose();
            });
        }

        public TimedTimerEntry CallPeriodically(int delayMillis, Action callback)
        {
            var timer = new TimedTimerEntry(delayMillis, callback);
            AddTimer(timer);
            return timer;
        }

        public TimedTimerEntry CallDelayed(int delayMillis, Action callback)
        {
            var timer = new TimedTimerEntry(delayMillis, -1, callback);
            AddTimer(timer);
            return timer;
        }

        protected void ProcessCallback(object state)
        {
            if (!IsRunning)
            {
                return;
            }

            if (Interlocked.CompareExchange(ref m_currentThreadId, Thread.CurrentThread.ManagedThreadId, 0) != 0)
                return;
            long timerStart = 0;
                // get the time at the start of our task processing
                timerStart = m_queueTimer.ElapsedMilliseconds;
                var updateDt = (int) (timerStart - m_lastUpdate);
                m_lastUpdate = (int) timerStart;
            try
            {
                // process messages
                IMessage msg;
                while (m_messageQueue.TryDequeue(out msg))
                {
                    try
                    {
                        msg.Execute();
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Failed to execute message {0} : {1}", msg, ex);
                    }

                    if (!IsRunning)
                    {
                        return;
                    }
                }
                
                foreach (var timer in m_pausedTimers.Where(timer => timer.Enabled))
                {
                    m_timers.Push(timer);
                }

                TimedTimerEntry peek;
                while (( peek = m_timers.Peek() ) != null && peek.NextTick <= DateTime.Now)
                {
                    var timer = m_timers.Pop();

                    if (!timer.Enabled)
                    {
                        if (!timer.IsDisposed)
                            m_pausedTimers.Add(timer);
                    }
                    else
                    {
                        try
                        {
                            timer.Trigger();

                            if (timer.Enabled)
                                m_timers.Push(timer);
                        }
                        catch (Exception ex)
                        {
                            logger.Error("Exception raised when processing TimerEntry {2} in {0} : {1}.", this, ex, timer);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                logger.Error("Failed to run TaskQueue callback for \"{0}\" : {1}", Name, ex);
            }
            finally
            {
                // get the end time
                var timerStop = m_queueTimer.ElapsedMilliseconds;

                var updateLagged = timerStop - timerStart > UpdateInterval;
                var callbackTimeout = updateLagged ? 0 : ((timerStart + UpdateInterval) - timerStop);

                Interlocked.Exchange(ref m_currentThreadId, 0);

                if (updateLagged)
                    logger.Debug("TaskPool '{0}' update lagged ({1}ms)", Name, timerStop - timerStart);

                if (IsRunning)
                {
                    // re-register the Update-callback
                    m_updateTask = Task.Factory.StartNewDelayed((int) callbackTimeout, ProcessCallback, this);
                }
                else
                {
                    m_stoppedAsync.Set();
                }
            }
        }
    }
}