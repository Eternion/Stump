using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stump.Core.Cache
{
    public class CacheObject<T>
    {
        private int m_maxDuration;
        private DateTime m_lastInsert;
        private T m_item;

        public CacheObject(int maxDuration)
        {
            m_maxDuration = maxDuration;
        }

        public void Insert(T item)
        {
            m_item = item;
            m_lastInsert = DateTime.Now;
        }

        public T Get(T item)
        {
            if ((DateTime.Now - m_lastInsert).TotalSeconds > m_maxDuration)
                Insert(item);

            return m_item;
        }
    }
}
