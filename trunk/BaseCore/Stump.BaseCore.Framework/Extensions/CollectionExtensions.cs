using System.Collections.Generic;
using System.Linq;

namespace Stump.BaseCore.Framework.Extensions
{
    public static class CollectionExtensions
    {
        public static bool CompareEnumerable<T>(this IEnumerable<T> ie1, IEnumerable<T> ie2)
        {
            if (ie1.GetType() != ie2.GetType())
                return false;

            if (ie1.Count() != ie2.Count())
                return false;

            IEnumerator<T> enumerator1 = ie1.GetEnumerator();
            while (enumerator1.MoveNext())
            {
                if (ie2.Count(entry => Equals(entry, enumerator1.Current)) != ie1.Count(entry => Equals(entry, enumerator1.Current)))
                    return false;
            }

            IEnumerator<T> enumerator2 = ie2.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                if (ie2.Count(entry => Equals(entry, enumerator2.Current)) != ie1.Count(entry => Equals(entry, enumerator2.Current)))
                    return false;
            }

            return true;
        }
    }
}