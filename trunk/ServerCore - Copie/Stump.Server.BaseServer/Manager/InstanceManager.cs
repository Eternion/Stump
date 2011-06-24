
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Stump.Server.BaseServer.Manager
{
    public abstract class InstanceManager<T> where T : IInstance
    {
        public delegate void InstanceEventHandler(T instance);

        public static event InstanceEventHandler InstanceAdded;

        private static void NotifyInstanceAdded(T instance)
        {
            InstanceEventHandler handler = InstanceAdded;
            if (handler != null) handler(instance);
        }

        public static event InstanceEventHandler InstanceRemoved;

        private static void NotifyInstanceRemoved(T instance)
        {
            InstanceEventHandler handler = InstanceRemoved;
            if (handler != null) handler(instance);
        }

        private static readonly ConcurrentStack<int> m_freeIds = new ConcurrentStack<int>();

        protected static ConcurrentDictionary<int, T> m_instances= new ConcurrentDictionary<int, T>();

        protected static int NextId;

        protected static int CreateInstance(T instance)
        {
            int id;

            if (!m_freeIds.IsEmpty)
            {
                if (!m_freeIds.TryPop(out id))
                {
                    id = NextId;
                    Interlocked.Increment(ref NextId);
                }
            }
            else
            {
                id = NextId;
                Interlocked.Increment(ref NextId);
            }

            if (m_instances.TryAdd(id, instance))
            {
                instance.Id = id;

                NotifyInstanceAdded(instance);
                return id;
            }

            return -1;
        }

        protected static bool RemoveInstance(T instance)
        {
            int id = instance.Id;

            T outvalue;
            if (m_instances.TryRemove(id, out outvalue))
            {
                instance.Id = -1;
                m_freeIds.Push(id);

                NotifyInstanceRemoved(outvalue);
                return true;
            }

            return false;
        }

        protected static T GetInstanceById(int id)
        {
            if (m_instances.ContainsKey(id))
            {
                T outvalue;
                return m_instances.TryGetValue(id, out outvalue) ? outvalue : default(T);
            }

            return default(T);
        }

        public T this[int id]
        {
            get { return GetInstanceById(id); }
        }
    }
}