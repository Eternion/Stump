using System;
using System.Threading;
using Stump.Core.Collections;

namespace Stump.Core.Pool.Task
{
    public class TaskPool
    {
        private readonly BlockingQueue<Action> m_tasks = new BlockingQueue<Action>();

        public void EnqueueTask(Action action)
        {
            m_tasks.Enqueue(action);
        }

        public void ProcessUpdate()
        {
            /* Execute Tasks */
            do
            {
                Action action = m_tasks.Dequeue();

                var executerThread = new Thread(() => action());
                executerThread.Start();

                while ((executerThread.ThreadState & ThreadState.Running) == ThreadState.Running)
                    // wait until thread end execution, or until thread enter a sleep state
                {
                    Thread.Yield(); // give priority to another thread
                }
            } while (m_tasks.Count > 0);
        }
    }
}