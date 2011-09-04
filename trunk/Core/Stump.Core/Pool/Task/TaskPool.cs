using System;
using System.Threading;
using NLog;
using Stump.Core.Collections;

namespace Stump.Core.Pool.Task
{
    public class TaskPool
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private BlockingQueue<Action> m_tasks = new BlockingQueue<Action>();

        public void Clear()
        {
            m_tasks = new BlockingQueue<Action>();
        }

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

                var executerThread = new Thread(
                    delegate()
                        {
                            try
                            {
                                action();
                            }
                            catch(Exception ex)
                            {
                                logger.Error("Exception occurs in the task Pool : {0}", ex);
                            }
                        });

                executerThread.Start();

                while (executerThread.ThreadState == ThreadState.Running)
                    // wait until thread end execution, or until thread enter a sleep state
                {
                    Thread.Yield(); // give priority to another thread
                }
            } while (m_tasks.Count > 0);
        }
    }
}