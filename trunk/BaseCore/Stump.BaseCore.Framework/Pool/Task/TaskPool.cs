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
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;

using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.Extensions;

namespace Stump.BaseCore.Framework.Xml
{
    public static class TaskPool
    {
        private static List<CyclicTask> m_cyclicTasks = new List<CyclicTask>();
        private static readonly object sync = new object();
        private static ConcurrentQueue<Action> m_tasks = new ConcurrentQueue<Action>();

        public static void Initialize(Assembly asm)
        {
            foreach (var type in asm.GetTypes())
            {
                var m = type.GetMethods();
                foreach (var method in type.GetMethods())
                {
                    var attributes = method.GetCustomAttributes(typeof(Cyclic), false);
                    if (attributes.Length == 1)
                    {
                        m_cyclicTasks.Add(new CyclicTask(Delegate.CreateDelegate(method.GetActionType(), method), (attributes[0] as Cyclic).Time, null, null, null));
                    }
                }
            }
        }

        public static void RegisterCyclicTask(Delegate method, uint time, DateTime? startDate, DateTime? endDate, uint? maxExecution)
        {
            lock (sync)
                m_cyclicTasks.Add(new CyclicTask(method, time, startDate, endDate, maxExecution));
        }

        public static void RegisterCyclicTask(CyclicTask cyclicTask)
        {
            lock (sync)
                m_cyclicTasks.Add(cyclicTask);
        }

        public static void UnregisterCyclicTask(Delegate method)
        {
            lock (sync)
                m_cyclicTasks.RemoveAll(m => m.Delegate == method);
        }

        public static void UnregisterCyclicTask(CyclicTask cyclicTask)
        {
            lock (sync)
                m_cyclicTasks.Remove(cyclicTask);
        }

        public static void EnqueueTask(Action action)
        {
            m_tasks.Enqueue(action);
        }

        private static Action m_action;
        public static void ProcessUpdate(int sleepTime)
        {
            /* Execute Tasks */
            while (m_tasks.TryDequeue(out m_action))
                m_action.DynamicInvoke();

            lock (sync)
            {
                /* Execute Cyclic Tasks */
                foreach (var cyclicMethod in m_cyclicTasks.Where(m => m.RequireExecution))
                    cyclicMethod.Execute();
                /* Delete Obsolete Tasks */
                m_cyclicTasks.RemoveAll(m => m.IsObsolete);
            }
        }
    }
}