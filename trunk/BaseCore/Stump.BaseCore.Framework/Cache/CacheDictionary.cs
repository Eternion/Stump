using System;
using System.Collections.Generic;
using System.Linq;
using Stump.BaseCore.Framework.Pool;

namespace Stump.BaseCore.Framework.Cache
{
    public class CacheDictionary<T, T1>
    {
        private Dictionary<T, CachedData<T1>> m_cache;
        private readonly int m_lifeTime;
        private readonly Func<T, T1> m_provider;

        private object m_syncCache = new object();

        public CacheDictionary(Func<T, T1> provider, int lifeTime = 5*3600, int clearTime = 5*3600, int capacity = 100)
        {
            m_provider = provider;
            m_lifeTime = lifeTime;
            m_cache = new Dictionary<T, CachedData<T1>>(capacity);

            TaskPool.Instance.RegisterCyclicTask(RemoveDirtyData, clearTime, null, null);
        }

        public T1 Get(T key)
        {
            return m_cache.ContainsKey(key) ? LoadAndCache(key) : m_cache[key].Data;
        }

        public Dictionary<T, T1> GetAll()
        {
            return m_cache.ToDictionary(entry => entry.Key, entry => entry.Value.Data);
        }

        public void FillCache(IDictionary<T, T1> elements)
        {
            m_cache = new Dictionary<T, CachedData<T1>>(elements.ToDictionary(entry => entry.Key, entry => new CachedData<T1>(entry.Value)));
        }

        private T1 LoadAndCache(T key)
        {
            T1 value = m_provider(key);

            lock (m_syncCache)
            {
                m_cache.Add(key, new CachedData<T1>(value));
            }

            return value;
        }

        private void RemoveDirtyData()
        {
            var outdatedElements = m_cache.Where(entry => entry.Value.CachedTime.TotalSeconds > m_lifeTime);

            foreach (var outdatedElement in outdatedElements)
            {
                m_cache.Remove(outdatedElement.Key);
            }
        }
    }
}