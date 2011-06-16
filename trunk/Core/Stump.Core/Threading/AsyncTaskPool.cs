using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Stump.Core.Collections;

namespace Stump.Core.Threading
{
    /// <summary>
    /// A task pool that processes messages asynchronously in a given application.
    /// </summary>
    public class AsyncTaskPool
    {
        #region Fields

        protected readonly Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Message queue containing messages to be executed.
        /// </summary>
        protected LockFreeQueue<IMessage> m_TaskQueue;
        /// <summary>
        /// Timer to measure how much time our processing has taken.
        /// </summary>
        protected Stopwatch m_taskTimer;
        protected long m_updateFrequency;
        /// <summary>
        /// Synchronization object.
        /// </summary>
        static readonly object obj = "";

        private bool m_Process;
        #endregion

        /// <summary>
        /// Creates a new task pool with an update frequency of 100ms.
        /// </summary>
        public AsyncTaskPool()
            : this(100)
        {
        }

        /// <summary>
        /// Creates a new task pool with the specified update frequency.
        /// </summary>
        /// <param name="updateFrequency">the update frequency of the task pool</param>
        public AsyncTaskPool(long updateFrequency)
        {
            m_TaskQueue = new LockFreeQueue<IMessage>();
            m_taskTimer = Stopwatch.StartNew();
            m_updateFrequency = updateFrequency;
            m_Process = true;
            Task.Factory.StartNewDelayed((int)m_updateFrequency, TaskUpdateCallback, this);
        }

        /// <summary>
        /// Enqueues a new task in the queue that will be executed during the next
        /// tick.
        /// </summary>
        public void EnqueueTask(IMessage task)
        {
            if (task == null)
                throw new ArgumentNullException("task", "task cannot be null");

            m_TaskQueue.Enqueue(task);
        }

        /// <summary>
        /// Queues a task for execution in the task pool.
        /// </summary>
        public void EnqueueTask(Action action)
        {
            m_TaskQueue.Enqueue(new Message(action));
        }


        /// <summary>
        /// Waits until all currently enqueued messages have been processed.
        /// </summary>
        public void WaitOneTick()
        {
            var msg = new Message(() =>
            {
                lock (obj)
                {
                    Monitor.PulseAll(obj);
                }
            });

            lock (obj)
            {
                m_TaskQueue.Enqueue(msg);
                Monitor.Wait(obj);
            }
        }

        public void StopProcessing()
        {
            m_Process = false;
        }

        public void ChangeUpdateFrequency(long frequency)
        {
            if (frequency < 0)
                throw new ArgumentException("frequency cannot be less than 0", "frequency");

            m_updateFrequency = frequency;
        }

        protected void TaskUpdateCallback(object state)
        {
            if (!m_Process)
                return;

            // get the time at the start of our task processing
            long timerStart = m_taskTimer.ElapsedMilliseconds;

            ProcessTasks(timerStart);

            // get the end time
            long timerStop = m_taskTimer.ElapsedMilliseconds;

            bool updateLagged = timerStop - timerStart > m_updateFrequency;
            long callbackTimeout = updateLagged ? 0 : ((timerStart + m_updateFrequency) - timerStop);

            if (!m_Process)
                return;

            // re-register the update to be called
            Task.Factory.StartNewDelayed((int)callbackTimeout, TaskUpdateCallback, this);
        }

        protected virtual void ProcessTasks(long startTime)
        {
            IMessage msg;

            // get and execute all the messages.
            while (m_TaskQueue.TryDequeue(out msg))
            {
                msg.Execute();
            }
        }
    }
}
