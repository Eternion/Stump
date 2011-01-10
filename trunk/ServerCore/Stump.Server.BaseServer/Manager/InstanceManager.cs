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
using System.Collections.Concurrent;
using System.Linq;
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