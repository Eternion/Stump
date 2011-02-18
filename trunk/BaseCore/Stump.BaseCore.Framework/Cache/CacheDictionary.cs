using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Pool;

namespace Stump.BaseCore.Framework.Cache
{
    public class CacheDictionary<T, T1>
    {
        private readonly Func<T, T1> m_provider;
        private readonly Dictionary<T, CachedData<T1>> m_cache;
        private readonly int m_lifeTime;

        public CacheDictionary(Func<T, T1> provider,int lifeTime = 5*3600 ,int clearTime = 5*3600, int capacity = 100)
        {
            m_provider = provider;
            m_lifeTime = lifeTime;
            m_cache = new Dictionary<T, CachedData<T1>>(capacity);
            TaskPool.Instance.RegisterCyclicTask((Action)RemoveDirtyData, clearTime, null, null);
        }

        public T1 Get(T key)
        {
            return m_cache.ContainsKey(key) ? LoadAndCache(key) : m_cache[key].Data;
        }

        private T1 LoadAndCache(T key)
        {
            var value = m_provider.Invoke(key);
            m_cache.Add(key, new CachedData<T1>(value));
            return value;
        }

        private void RemoveDirtyData()
        {
            foreach (var e in m_cache)
            {
                if (e.Value.CachedTime.TotalSeconds > m_lifeTime )
                    m_cache.Remove(e.Key);
            }
        }
    }
}
