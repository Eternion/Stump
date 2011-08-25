using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Stump.Core.Pool
{
    public class UniqueIdProvider
    {
        private readonly ConcurrentQueue<int> m_freeIds = new ConcurrentQueue<int>();
        private int m_nextId;

        public UniqueIdProvider()
        {
            
        }

        public UniqueIdProvider(int lastId)
        {
            m_nextId = lastId + 1;
        }

        public UniqueIdProvider(IEnumerable<int> freeIds)
        {
            foreach (var freeId in freeIds)
            {
                m_freeIds.Enqueue(freeId);
            }
        }

        public int Pop()
        {
            int id;

            if (!m_freeIds.IsEmpty)
            {
                if (!m_freeIds.TryDequeue(out id))
                {
                    id = m_nextId;
                    Interlocked.Increment(ref m_nextId);
                }
                else
                {
                    id = m_nextId;
                    Interlocked.Increment(ref m_nextId);
                }
            }
            else
            {
                id = m_nextId;
                Interlocked.Increment(ref m_nextId);
            }

            return id;
        }

        /// <summary>
        /// Indicate that the given id is free
        /// </summary>
        /// <param name="freeId"></param>
        public void Push(int freeId)
        {
            m_freeIds.Enqueue(freeId);
        }
    }
}