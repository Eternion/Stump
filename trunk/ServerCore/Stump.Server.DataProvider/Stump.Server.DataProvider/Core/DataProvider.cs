using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Cache;
using Stump.BaseCore.Framework.Utils;
using Stump.Server.DataProvider.Core2;

namespace Stump.Server.DataProvider.Core
{
    public abstract class DataProvider<T, T1> : Singleton<DataProvider<T, T1>>, IEnumerable<T1>
    {
        private LoadingType m_loadingType;

        private int m_lifeTime;

        private int m_checkTime;

        protected CacheDictionary<T, T1> m_cacheDico;

        protected Dictionary<T, T1> m_dico;

        public T1 this[T index]
        {
            get { return Get(index); }
        }

        internal void Init(ProviderParams @params)
        {
            m_loadingType = @params.LoadingType;
            m_lifeTime = @params.LifeTime;
            m_checkTime = @params.CheckTime;

            switch (m_loadingType)
            {
                case LoadingType.PreLoading:
                    m_dico = GetAllData();
                    m_dGet = i => m_dico.ContainsKey(i) ? m_dico[i] : default(T1);
                    m_dGetAll = () => m_dico.Select(k => k.Value);
                    break;
                case LoadingType.Cached:
                    m_cacheDico = new CacheDictionary<T, T1>(GetData, m_lifeTime, m_checkTime);
                    m_dGet = m_cacheDico.Get;
                    m_dGetAll = () => GetAllData().Select(k => k.Value);
                    break;
                case LoadingType.LazyLoading:
                    m_dGet = GetData;
                    m_dGetAll = () => GetAllData().Select(k=>k.Value);
                    break;
            }
        }

        public T1 Get(T id)
        {
            return m_dGet(id);
        }

        public IEnumerable<T1> GetAll()
        {
            return m_dGetAll();
        }

        private DProvider m_dGet;
        private delegate T1 DProvider(T id);

        private DProviderAll m_dGetAll;
        private delegate IEnumerable<T1> DProviderAll();

        protected abstract T1 GetData(T id);

        protected abstract Dictionary<T, T1> GetAllData();      

        #region Implementation of IEnumerable

        public IEnumerator<T1> GetEnumerator()
        {
            return GetAll().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
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
