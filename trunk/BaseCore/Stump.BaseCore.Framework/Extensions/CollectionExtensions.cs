using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stump.BaseCore.Framework.Extensions
{
    public static class CollectionExtensions
    {
        public static string ByteArrayToString(this byte[] byteCryptedMd5)
        {
            var output = new StringBuilder(byteCryptedMd5.Length);

            for (int i = 0; i < byteCryptedMd5.Length; i++)
            {
                output.Append(byteCryptedMd5[i].ToString("X2"));
            }

            return output.ToString().ToLower();
        }

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

        public static T MaxOf<T, T1>(this IList<T> collection, Func<T, T1> selector) where T1 : IComparable<T1>
        {
            if (collection.Count == 0) return default(T);
            
            var maxT = collection[0];
            var maxT1 = selector(maxT);
            
            for (var i = 1; i < collection.Count - 1; i++)
            {
                var currentT1 = selector(collection[i]);
                if (currentT1.CompareTo( maxT1) > 0)
                {
                    maxT = collection[i];
                    maxT1 = currentT1;
                }
            }
            return maxT;
        }

        public static T MinOf<T, T1>(this IList<T> collection, Func<T, T1> selector) where T1 : IComparable<T1>
        {
            if (collection.Count == 0) return default(T);

            var maxT = collection[0];
            var maxT1 = selector(maxT);

            for (var i = 1; i < collection.Count - 1; i++)
            {
                var currentT1 = selector(collection[i]);
                if (currentT1.CompareTo(maxT1) < 0)
                {
                    maxT = collection[i];
                    maxT1 = currentT1;
                }
            }
            return maxT;
        }

        /// <summary>
        /// Returns the string representation of an IEnumerable (all elements, joined by comma)
        /// </summary>
        /// <param name="conj">The conjunction to be used between each elements of the collection</param>
        public static string ToStringCol(this ICollection collection, string conj)
        {
            return collection != null ? string.Join(conj, ToStringArr(collection)) : "(null)";
        }

        public static string ToString(this IEnumerable collection, string conj)
        {
            return collection != null ? string.Join(conj, ToStringArr(collection)) : "(null)";
        }

        public static string[] ToStringArr(this IEnumerable collection)
        {
            var strs = new List<string>();
            var colEnum = collection.GetEnumerator();
            while (colEnum.MoveNext())
            {
                var cur = colEnum.Current;
                if (cur != null)
                {
                    strs.Add(cur.ToString());
                }
            }
            return strs.ToArray();
        }
    }
}