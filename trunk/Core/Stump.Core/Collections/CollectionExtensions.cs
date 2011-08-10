using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Stump.Core.Collections
{
    public static class CollectionExtensions
    {
        public static T GetOrDefault<T>(this IList<T> list, int index)
        {
            return index >= list.Count ? default(T) : list[index];
        }

        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            TValue val;
            return dict.TryGetValue(key, out val) ? val : default(TValue);
        }
    }
}