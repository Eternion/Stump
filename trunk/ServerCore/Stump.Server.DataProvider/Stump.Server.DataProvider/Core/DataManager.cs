using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Stump.BaseCore.Framework.Cache;
using Stump.BaseCore.Framework.Utils;

namespace Stump.Server.DataProvider.Core
{
    public abstract class DataManager<T, T1> : Singleton<DataManager<T, T1>>, IEnumerable<T1> where T1 : class
    {
        private CacheDictionary<T, T1> m_cache;
        private Dictionary<T, T1> m_preLoadData;
        private List<T> m_updatedList = new List<T>();

        private int m_checkTime;
        private int m_lifeTime;
        private string m_loadingType;

        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();

        public T1 this[T index]
        {
            get { return Get(index); }
        }

        #region Initialization

        private void Init(DataManagerParams @params)
        {
            m_loadingType = @params.LoadingType;
            m_lifeTime = @params.LifeTime;
            m_checkTime = @params.CheckTime;

            switch (m_loadingType)
            {
                case "PreLoading":
                    PreLoadingInit();
                    m_logger.Info("PreLoaded {0} element(s)", m_preLoadData.Count);
                    break;
                case "CachedLoading":
                    CachedLoadingInit();
                    m_logger.Info("Init Cache with lifeTime of {0}second(s)", m_lifeTime);
                    break;
                case "LazyLoading":
                    LazyLoadingInit();
                    m_logger.Info("Init with LazyLoading");
                    break;
                default :
                    m_logger.Info("Wrong LoadingType, can't manage any elements");
                    break;
            }
        }

        private void PreLoadingInit()
        {
            m_preLoadData = GetAllData();
            m_getMethod = i => m_preLoadData.ContainsKey(i) ? m_preLoadData[i] : default(T1);
            m_getAllMethod = () => m_preLoadData.Select(k => k.Value);
        }

        private void CachedLoadingInit()
        {
            m_cache = new CacheDictionary<T, T1>(GetData, m_lifeTime, m_checkTime);
            m_getMethod = m_cache.Get;
            m_getAllMethod = () => GetAllData().Select(k => k.Value);
        }

        private void LazyLoadingInit()
        {
            m_getMethod = GetData;
            m_getAllMethod = () => GetAllData().Select(k => k.Value);
        }

        #endregion

        #region Public Methods

        public T1 Get(T id)
        {
            return m_getMethod(id);
        }

        public IEnumerable<T1> GetAll()
        {
            return m_getAllMethod();
        }

        public bool Contains(T id)
        {
            return Get(id) != null;
        }

        public void Add(T id, T1 element)
        {
            
        }

        public void Remove(T id)
        {
            
        }

        public void Save(T index)
        {

        }

        public void SaveAndFlush(T index)
        {

        }

        public void Flush()
        {
            
        }

        #endregion

        #region Abstract Methods

        protected abstract T1 GetData(T id);

        protected abstract Dictionary<T, T1> GetAllData();

        #endregion

        #region Delegates

        private GetDelegate m_getMethod;
        private delegate T1 GetDelegate(T id);

        private GetAllDelegate m_getAllMethod;
        private delegate IEnumerable<T1> GetAllDelegate();

        #endregion

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


    public abstract class DataManager<T> : Singleton<DataManager<T>> where T : class
    {
        private List<T> m_preLoadData;

        internal void Init(DataManagerParams @params)
        {
            m_preLoadData = GetAllData();
        }

        public bool Contains(Func<T, bool> predicate)
        {
            return m_preLoadData.FirstOrDefault(predicate) != null;
        }

        public IEnumerable<T> Get(Func<T, bool> predicate)
        {
            return m_preLoadData.Where(predicate);
        }

        protected abstract List<T> GetAllData();
    }
}