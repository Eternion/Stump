using System;
using System.Linq;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Cache;
using Stump.BaseCore.Framework.Utils;

namespace Stump.Server.DataProvider.Core
{
    public abstract class DataProvider<T, T1> : Singleton<DataProvider<T,T1>>
    {
        private DataLoadingType m_loadingType;
    
        private int m_lifeTime;

        private int m_checkTime;
    
        protected CacheDictionary<T, T1> m_cacheDico;

        protected Dictionary<T, T1> m_dico;

        internal void Init(ProviderParams @params)
        {
            m_loadingType = @params.LoadingType;
            m_lifeTime = @params.LifeTime;
            m_checkTime = @params.CheckTime;

            if (m_loadingType == DataLoadingType.PreLoading)
            {
                m_dico = GetAllData();
                m_dGet = i => m_dico.ContainsKey(i) ? m_dico[i] : default(T1);
            }
            else if (m_loadingType == DataLoadingType.Cached)
            {
                m_cacheDico = new CacheDictionary<T, T1>(GetData, m_lifeTime, m_checkTime);
                m_dGet = m_cacheDico.Get;
            }
            else if(m_loadingType == DataLoadingType.LazyLoading)
            {
                m_dGet = GetData;
            }
        }

        public T1 Get(T id)
        {
            return m_dGet(id);
        }

        private DProvider m_dGet;

        private delegate T1 DProvider(T id);

        protected abstract T1 GetData(T id);

        protected abstract Dictionary<T, T1> GetAllData();
    }


    public abstract class DataProvider<T> : Singleton<DataProvider<T>>
    {
        protected List<T> m_list;

        internal void Init(ProviderParams @params)
        {
            m_list = GetAllData();
        }

        public bool Contains(Func<T, bool> predicate)
        {
            return m_list.Count(predicate) != 0;
        }

        public IEnumerable<T> Get(Func<T, bool> predicate)
        {
            return m_list.Where(predicate);
        }

        protected abstract List<T> GetAllData();
    }
}
