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
using System.Collections.Generic;
using System.Threading;

namespace Stump.BaseCore.Framework.Collections
{
    public sealed class BlockingQueue<T>
    {
        private readonly object m_lockObj = new object();
        private readonly Queue<T> m_queue;
        private bool m_isWaiting;

        public bool IsWaiting
        {
            get { return m_isWaiting; }
        }

        public int Count
        {
            get { return m_queue.Count; }
        }

        public BlockingQueue(int quantity)
        {
            m_queue = new Queue<T>();
        }

        public BlockingQueue()
        {
            m_queue = new Queue<T>();
        }

        public void Enqueue(T element)
        {
            lock (m_lockObj)
            {
                m_queue.Enqueue(element);
                if (m_queue.Count == 1)
                {
                    Monitor.Pulse(m_lockObj);
                }
            }
        }

        public T Dequeue()
        {
            lock (m_lockObj)
            {
                while (m_queue.Count == 0)
                {
                    m_isWaiting = true;
                    Monitor.Wait(m_lockObj);
                }
                m_isWaiting = false;
                return m_queue.Dequeue();
            }
        }
    }
}