using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.Reflection;

namespace Stump.BaseCore.Framework.Pool.Task
{
    public delegate bool Condition();

    public class TaskPool
    {
        private readonly List<CyclicTask> m_cyclicTasks = new List<CyclicTask>();
        private readonly object m_sync = new object();
        private readonly ConcurrentQueue<Action> m_tasks = new ConcurrentQueue<Action>();
        private static TaskPool m_instance;

        public static TaskPool Instance
        {
            get { return m_instance ?? (m_instance = new TaskPool()); }
        }


        public void Initialize(Assembly asm)
        {
            foreach (var type in asm.GetTypes())
            {
                var m = type.GetMethods();
                foreach (var method in type.GetMethods())
                {
                    var attribute = method.GetCustomAttributes(typeof(Cyclic), false).FirstOrDefault() as Cyclic;
                    if (attribute != null)
                    {
                        m_cyclicTasks.Add(new CyclicTask(Delegate.CreateDelegate(method.GetActionType(), method) as Action, attribute.Time,null, null));
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

        private Action m_action;
        public void ProcessUpdate()
        {
            /* Execute Tasks */
            while (m_tasks.TryDequeue(out m_action))
                m_action.Invoke();

            lock (m_sync)
            {
                /* Execute Cyclic Tasks */
                foreach (var cyclicMethod in m_cyclicTasks.Where(m => m.RequireExecution))
                    cyclicMethod.Execute();
                /* Delete Obsolete Tasks */
                m_cyclicTasks.RemoveAll(m => m.ReachMaxExecutionNbr);
            }
        }
    }
}