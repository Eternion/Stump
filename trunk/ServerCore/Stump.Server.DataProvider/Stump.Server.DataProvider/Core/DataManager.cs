using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stump.BaseCore.Framework.Cache;
using Stump.BaseCore.Framework.Utils;

namespace Stump.Server.DataProvider.Core
{
    public abstract class DataManager<T, T1> : Singleton<DataManager<T, T1>>, IEnumerable<T1>
    {
        protected CacheDictionary<T, T1> Cache;
        protected Dictionary<T, T1> PreLoadData;

        private int m_checkTime;
        private GetDelegate m_getMethod;
        private GetAllDelegate m_getAllMethod;

        private int m_lifeTime;
        private LoadingType m_loadingType;

        public T1 this[T index]
        {
            get { return Get(index); }
        }

        internal void Init(DataManagerParams @params)
        {
            m_loadingType = @params.LoadingType;
            m_lifeTime = @params.LifeTime;
            m_checkTime = @params.CheckTime;

            switch (m_loadingType)
            {
                case LoadingType.PreLoading:
                    PreLoadData = GetAllData();
                    m_getMethod = i => PreLoadData.ContainsKey(i) ? PreLoadData[i] : default(T1);
                    m_getAllMethod = () => PreLoadData.Select(k => k.Value);
                    break;

                case LoadingType.Cached:
                    Cache = new CacheDictionary<T, T1>(GetData, m_lifeTime, m_checkTime);
                    m_getMethod = Cache.Get;
                    m_getAllMethod = () => GetAllData().Select(k => k.Value);
                    break;
                    
                case LoadingType.LazyLoading:
                    m_getMethod = GetData;
                    m_getAllMethod = () => GetAllData().Select(k => k.Value);
                    break;
            }
        }

        public T1 Get(T id)
        {
            return m_getMethod(id);
        }

        public IEnumerable<T1> GetAll()
        {
            return m_getAllMethod();
        }

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

        #region Nested type: GetDelegate

        private delegate T1 GetDelegate(T id);

        #endregion

        #region Nested type: GetAllDelegate

        private delegate IEnumerable<T1> GetAllDelegate();

        #endregion
    }


    public abstract class DataManager<T> : Singleton<DataManager<T>>
    {
        protected List<T> PreLoadData;

        internal void Init(DataManagerParams @params)
        {
            PreLoadData = GetAllData();
        }

        public bool Contains(Func<T, bool> predicate)
        {
            return PreLoadData.Count(predicate) != 0;
        }

        public IEnumerable<T> Get(Func<T, bool> predicate)
        {
            return PreLoadData.Where(predicate);
        }

        protected abstract List<T> GetAllData();
    }
}