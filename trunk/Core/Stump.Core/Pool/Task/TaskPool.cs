using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.Core.Reflection;

namespace Stump.Core.Pool.Task
{
    public delegate bool Condition();

    public class TaskPool
    {
        private readonly List<CyclicTask> m_cyclicTasks = new List<CyclicTask>();

        private readonly object m_sync = new object();
        private readonly BlockingQueue<Action> m_tasks = new BlockingQueue<Action>();
        private static TaskPool m_instance;

        public static TaskPool Instance
        {
            get { return m_instance ?? (m_instance = new TaskPool()); }
        }

        public void Initialize(Assembly asm)
        {
            foreach (var type in asm.GetTypes())
            {
                foreach (var method in type.GetMethods())
                {
                    var attribute = method.GetCustomAttributes(typeof (Cyclic), false).FirstOrDefault() as Cyclic;
                    if (attribute != null)
                    {
                        m_cyclicTasks.Add(new CyclicTask(Delegate.CreateDelegate(method.GetActionType(), method) as Action, attribute.Time, null, null));
                    }
                }
            }
        }

        public void RegisterCyclicTask(Action method, int time, Condition condition, uint? maxExecution)
        {
            lock (m_sync)
                m_cyclicTasks.Add(new CyclicTask(method, time, condition, maxExecution));
        }

        public void RegisterCyclicTask(CyclicTask cyclicTask)
        {
            lock (m_sync)
                m_cyclicTasks.Add(cyclicTask);
        }

        public void UnregisterCyclicTask(Action method)
        {
            lock (m_sync)
                m_cyclicTasks.RemoveAll(m => m.Action == method);
        }

        public void UnregisterCyclicTask(CyclicTask cyclicTask)
        {
            lock (m_sync)
                m_cyclicTasks.Remove(cyclicTask);
        }

        public void EnqueueTask(Action action)
        {
            m_tasks.Enqueue(action);
        }

        public void ProcessUpdate()
        {
            /* Execute Tasks */
            while (m_tasks.Count > 0)
            {
                Action action = m_tasks.Dequeue();

                var executerThread = new Thread(() => action());
                executerThread.Start();

                while ((executerThread.ThreadState & ThreadState.Running) == ThreadState.Running)
                    // wait until thread end execution, or until thread enter a sleep state
                {
                    Thread.Yield(); // give priority to another thread
                }
            }

            // todo : we maybe should separate TaskPool and CyclicTasks

            /* Execute Cyclic Tasks */
            foreach (var cyclicMethod in m_cyclicTasks.Where(m => m.RequireExecution))
                cyclicMethod.Execute();

            /* Delete Obsolete Tasks */
            m_cyclicTasks.RemoveAll(m => m.ReachMaxExecutionNbr);
        }
    }
}