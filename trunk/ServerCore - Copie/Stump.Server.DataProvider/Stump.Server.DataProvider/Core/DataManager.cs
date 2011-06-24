using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NLog;
using Stump.Core.Cache;
using Stump.Core.Reflection;

namespace Stump.Server.DataProvider.Core
{
    public abstract class DataManager<T, T1> : Singleton<DataManager<T, T1>>, IEnumerable<T1> where T1 : class
    {
        private readonly ConcurrentQueue<DataModification<T, T1>> m_modificationsQueue =
            new ConcurrentQueue<DataModification<T, T1>>();

        private CacheDictionary<T, T1> m_cache;

        private int m_checkTime;
        private GetAllDelegate m_getAllMethod;
        private GetDelegate m_getOneMethod;
        private int m_lifeTime;
        private LoadingType m_loadingType;
        private ConcurrentDictionary<T, T1> m_preLoadedData;

        public T1 this[T index]
        {
            get { return GetOne(index); }
        }

        #region Initialization

        internal void Initialize(DataManagerParams @params)
        {
            m_loadingType = @params.LoadingType;
            m_lifeTime = @params.LifeTime;
            m_checkTime = @params.CheckTime;

            switch (m_loadingType)
            {
                case LoadingType.PreLoading:
                    PreLoadingInit();
                    break;
                case LoadingType.CacheLoading:
                    CachedLoadingInit();
                    break;
            }
        }

        private void PreLoadingInit()
        {
            m_preLoadedData = new ConcurrentDictionary<T, T1>(InternalGetAll()); // load datas

            // when we call GetOne/GetAll we load datas from the list
            m_getOneMethod = i => m_preLoadedData.ContainsKey(i) ? m_preLoadedData[i] : default(T1);
            m_getAllMethod = () => m_preLoadedData.Values;
        }

        private void CachedLoadingInit()
        {
            m_cache = new CacheDictionary<T, T1>(InternalGetOne, m_lifeTime, m_checkTime);
                // create a dictionary for the cache

            //when we call GetOne/GetAll we load first the data from the files then we store this datas
            m_getOneMethod = m_cache.Get;
            m_getAllMethod = () =>
                                 {
                                     m_cache.FillCache(InternalGetAll());

                                     return m_cache.GetAll().Values;
                                 };
        }

        #endregion

        #region Public Methods

        public T1 GetOne(T id)
        {
            return m_getOneMethod(id);
        }

        public IEnumerable<T1> GetAll()
        {
            return m_getAllMethod();
        }

        public void Add(T id, T1 element)
        {
            for (int i = 0; i < 10 && !m_preLoadedData.TryAdd(id, element); i++)
            {
                Thread.Sleep(0); // change current thread
            }

            m_modificationsQueue.Enqueue(new DataModification<T, T1>(FlushAction.Add, id, element));
        }

        public void AddAndFlush(T id, T1 element)
        {
            Add(id, element);
            Flush();
        }

        public void Remove(T id)
        {
            T1 dummy;
            for (int i = 0; i < 10 && !m_preLoadedData.TryRemove(id, out dummy); i++)
            {
                Thread.Sleep(0); // change current thread
            }

            m_modificationsQueue.Enqueue(new DataModification<T, T1>(FlushAction.Remove, id));
        }

        public void RemoveAndFlush(T id)
        {
            Remove(id);
            Flush();
        }

        public void Save(T id)
        {
            m_modificationsQueue.Enqueue(new DataModification<T, T1>(FlushAction.Modify, id, GetOne(id)));
        }

        public void SaveAndFlush(T index)
        {
            Save(index);
            Flush();
        }

        public void Flush()
        {
            var modifications = new List<DataModification<T, T1>>();

            while (!m_modificationsQueue.IsEmpty)
            {
                DataModification<T, T1> dataModification;

                if (m_modificationsQueue.TryDequeue(out dataModification))
                {
                    modifications.Add(dataModification);
                }
            }

            Flush(modifications.ToArray());
        }

        #endregion

        #region Abstract Methods

        protected abstract T1 InternalGetOne(T id);
        protected abstract Dictionary<T, T1> InternalGetAll();

        protected abstract void Flush(DataModification<T, T1>[] modifications);

        #endregion

        #region Nested type: DataModification

        protected class DataModification<TD, TD1>
        {
            public DataModification(FlushAction flushAction, TD index, TD1 element)
            {
                Action = flushAction;
                Index = index;
                Element = element;
            }

            public DataModification(FlushAction flushAction, TD index)
            {
                Action = flushAction;
                Index = index;
            }

            public FlushAction Action
            {
                get;
                set;
            }

            public TD Index
            {
                get;
                set;
            }

            public TD1 Element
            {
                get;
                set;
            }
        }

        #endregion

        #region Nested type: FlushAction

        protected enum FlushAction
        {
            Add,
            Remove,
            Modify,
            Nothing
        }

        #endregion

        #region Nested type: GetAllDelegate

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

        #region Nested type: GetDelegate

        private delegate T1 GetDelegate(T id);

        #endregion
    }


    public abstract class DataManager<T> : Singleton<DataManager<T>> where T : class
    {
        private List<T> m_preLoadData;

        internal void Initialize(DataManagerParams @params)
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