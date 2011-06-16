using System.Collections.Generic;
using System.Threading;

namespace Stump.Core.Collections
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