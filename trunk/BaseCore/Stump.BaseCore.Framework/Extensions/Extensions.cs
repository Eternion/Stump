// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Stump.BaseCore.Framework.Extensions
{
    public static class Extensions
    {
        public static Type GetActionType(this MethodInfo method)
        {
            return Expression.GetActionType(method.GetParameters().Select(entry => entry.ParameterType).ToArray());
        }

        public static Delegate ToDelegate(this MethodInfo mi, object target)
        {
            if (mi == null) throw new ArgumentNullException("mi");

            Type delegateType;

            var typeArgs = mi.GetParameters()
                .Select(p => p.ParameterType)
                .ToList();

            // builds a delegate type
            if (mi.ReturnType == typeof(void))
            {
                delegateType = Expression.GetActionType(typeArgs.ToArray());

            }
            else
            {
                typeArgs.Add(mi.ReturnType);
                delegateType = Expression.GetFuncType(typeArgs.ToArray());
            }

            // creates a binded delegate if target is supplied
            var result = ( target == null )
                ? Delegate.CreateDelegate(delegateType, mi)
                : Delegate.CreateDelegate(delegateType, target, mi);

            return result;
        }

        public static string RandomString(this Random random, int size)
        {
            var builder = new StringBuilder();

            for (int i = 0; i < size; i++)
            {
                //26 letters in the alphabet, ascii + 65 for the capital letters
                builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26*random.NextDouble() + 65))));
            }
            return builder.ToString();
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
    }
}