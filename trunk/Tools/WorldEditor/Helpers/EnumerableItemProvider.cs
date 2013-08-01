using System;
using System.Collections.Generic;
using WorldEditor.Helpers.Collections;

namespace WorldEditor.Helpers
{
    // do not clean up pages with it !!
    public class EnumerableItemProvider<T> : IItemsProvider<T>
    {
        private readonly IEnumerator<T> m_enumerator;
        private bool m_isEnded;

        private int m_count = 0;

        public EnumerableItemProvider(IEnumerable<T> enumerable)
        {
            m_enumerator = enumerable.GetEnumerator();
        }

        public bool IsEnded
        {
            get { return m_isEnded; }
        }

        public int FetchCount()
        {
            return m_isEnded ? m_count : m_count + 1; // adds one to force him to fetch another item
        }

        public IList<T> FetchRange(int startIndex, int count)
        {
            if (m_isEnded && m_count == 0)
                return new List<T>();

            if (m_isEnded || startIndex > m_count) // may not work
            {
                m_enumerator.Reset();
                m_count = 0;
            }

            var list = new List<T>();
            bool movenext = true;
            while (startIndex + count > m_count && (movenext = m_enumerator.MoveNext()))
            {
                if (startIndex <= m_count)
                    list.Add(m_enumerator.Current);

                m_count++;
            }

            if (!movenext) // ended, no more item !
                m_isEnded = true;

            return list;
        }
    }
}