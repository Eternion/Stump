using System;
using System.Collections.Generic;
using System.Linq;
using Stump.BaseCore.Framework.Cache;
using Stump.BaseCore.Framework.Utils;

namespace Stump.Server.DataProvider.Core
{
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
