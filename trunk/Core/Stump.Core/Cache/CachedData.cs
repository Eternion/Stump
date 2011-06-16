using System;

namespace Stump.Core.Cache
{
    class CachedData<T>
    {
        private readonly T m_data;
        private readonly DateTime m_cacheTime;

        public CachedData(T data)
        {
            m_data = data;
            m_cacheTime = DateTime.Now;
        }

        public T Data
        {
            get { return m_data; }
        }

        public TimeSpan CachedTime
        {
            get { return DateTime.Now.Subtract(m_cacheTime); }
        }
    }
}
