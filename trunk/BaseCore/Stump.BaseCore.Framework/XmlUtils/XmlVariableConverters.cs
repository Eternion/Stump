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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.Exceptions;
using Stump.BaseCore.Framework.Utils;

namespace Stump.BaseCore.Framework.XmlUtils
{
    // todo : maybe can we found a way to add any method to convert to any type we wan't in any assembly
    /// <summary>
    ///   Provides some methods to convert string to a given type
    /// </summary>
    public class XmlVariableConverters
    {
        /// <summary>
        /// Allow to pack parsed array's element with quotes
        /// </summary>
        /// <example>
        /// Parsed string : John, Harry, "Thomas, Moore"
        /// 
        /// When it's set to true the array will contains 3 elements :
        ///     - John
        ///     - Harry
        ///     - Thomas, Moore
        /// 
        /// When it's false the array will contains 4 elements :
        ///     - John
        ///     - Harry
        ///     - "Thomas
        ///     - Moore"
        /// </example>
        public const bool UseModifiers = true;

        // note : very ugly way ...
        public static IList ToList(string str, Type elementType)
        {
            Type listType = typeof (List<>).MakeGenericType(new[]
                {
                    elementType
                });

            var list = Activator.CreateInstance(listType) as IList;
            foreach (object element in ToArray(str, elementType))
            {
                list.Add(element);
            }

            return list;
        }

        public static Array ToArray(string str, Type elementType)
        {
            string[] strAsArray = UseModifiers ? StringUtils.Split(str.Trim(), ",", "\"") : str.Trim().Split(',');

            if (UseModifiers)
            {
                for (int i = 0; i < strAsArray.Length; i++ )
                {
                    strAsArray[i] = strAsArray[i].Trim();

                    if (strAsArray[i].Length > 2 && strAsArray[i].StartsWith("\"") && strAsArray[i].EndsWith("\""))
                        strAsArray[i] = strAsArray[i].Remove(strAsArray[i].Length - 1).Remove(0, 1);
                }
            }

            Array array = Array.CreateInstance(elementType, strAsArray.Length);

            for (int i = 0; i < strAsArray.Length; i++)
            {
                array.SetValue(FoundConverter(strAsArray[i], elementType), i);
            }

            return array;
        }


        public static object ToEnum(string str, Type enumType)
        {
            return Enum.IsDefined(enumType, str) ? Enum.Parse(enumType, str) : Enum.ToObject(enumType, str);
        }

        public static T FoundConverter<T>(string value)
        {
            return (T) FoundConverter(value, typeof (T));
        }

        public static object FoundConverter(string value, Type toType)
        {
            // ugly code but attributes isn't an easy way

            if (toType.IsEnum)
                return ToEnum(value, toType);

            if (toType.GetInterfaces().Contains(typeof (IConvertible)))
                return Convert.ChangeType(value, toType, CultureInfo.InvariantCulture);

            if (toType.IsArray)
                return ToArray(value, toType.GetElementType());

            if (toType.GetInterfaces().Contains(typeof (IList)) && toType.IsGenericType)
                return ToList(value, toType.GetGenericArguments()[0]);

            throw new NotFoundException("Converter of " + toType + " doesn't exists");
        }
    }
}