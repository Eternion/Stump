using System.Collections.Generic;
using System.Threading;

namespace Stump.Core.Pool
{
    public class ReversedUniqueIdProvider : UniqueIdProvider
    {
        public ReversedUniqueIdProvider()
        {
        }

        public ReversedUniqueIdProvider(int lastId)
        {
            m_nextId = lastId - 1;
        }

        public ReversedUniqueIdProvider(IEnumerable<int> freeIds)
            : base(freeIds)
        {
        }

        public override int Pop()
        {
            int id;

            if (!m_freeIds.IsEmpty)
            {
                if (!m_freeIds.TryDequeue(out id))
                {
                    id = m_nextId;
                    Interlocked.Decrement(ref m_nextId);
                }
            }
            else
            {
                id = m_nextId;
                Interlocked.Decrement(ref m_nextId);
            }

            return id;
        }

    }
}