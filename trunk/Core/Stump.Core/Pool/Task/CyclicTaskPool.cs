using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Stump.Core.Attributes;
using Stump.Core.Reflection;

namespace Stump.Core.Pool.Task
{
    public class CyclicTaskPool
    {
        private readonly List<CyclicTask> m_cyclicTasks = new List<CyclicTask>();
        private readonly object m_sync = new object();

        public void Initialize(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        var attribute = method.GetCustomAttributes(typeof (Cyclic), false).SingleOrDefault() as Cyclic;
                        if (attribute != null)
                        {
                            RegisterCyclicTask(Delegate.CreateDelegate(method.GetActionType(), method) as Action, attribute.Time);
                        }
                    }
                }
            }
        }

        public void Clear()
        {
            foreach (var task in m_cyclicTasks.ToArray())
                UnregisterCyclicTask(task);
        }

        public void RegisterCyclicTask(Action method, int interval)
        {
            RegisterCyclicTask(new CyclicTask(method, TimeSpan.FromSeconds(interval)));
        }

        public void RegisterCyclicTask(Action method, int interval, int waitDelay = 0, int? callsLimit = null, CyclicTask.Predicate predicate = null)
        {
            RegisterCyclicTask(new CyclicTask(method, TimeSpan.FromSeconds(interval)));
        }

        public void RegisterCyclicTask(CyclicTask cyclicTask)
        {
            lock (m_sync)
            {
                m_cyclicTasks.Add(cyclicTask);
                cyclicTask.TaskEnded += UnregisterCyclicTask;
                cyclicTask.Start();
            }
        }

        public void UnregisterCyclicTask(Action method)
        {
            foreach (CyclicTask task in m_cyclicTasks.Where(entry => entry.Action == method).ToArray())
                UnregisterCyclicTask(task);
        }

        public void UnregisterCyclicTask(CyclicTask cyclicTask)
        {
            lock (m_sync)
            {
                cyclicTask.Stop();
                m_cyclicTasks.Remove(cyclicTask);
            }
        }
    }
}