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
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Stump.BaseCore.Framework.Utils
{
    public static class StringUtils
    {
        public static string[] Split(string expression, string delimiter)
        {
            return Split(expression, delimiter, "", false);
        }

        public static string[] Split(string expression, string delimiter,
                                     string qualifier)
        {
            return Split(expression, delimiter, qualifier, false);
        }

        public static string[] Split(string expression, string delimiter,
                                     string qualifier, bool ignoreCase)
        {
            bool qualifierState = false;
            int startIndex = 0;
            var values = new ArrayList();

            for (int charIndex = 0; charIndex < expression.Length - 1; charIndex++)
            {
                if (qualifier != null)
                    if (string.Compare(expression.Substring
                                           (charIndex, qualifier.Length), qualifier, ignoreCase) == 0)
                    {
                        qualifierState = !(qualifierState);
                    }
                    else if (!(qualifierState) & (delimiter != null)
                             & (string.Compare(expression.Substring
                                                   (charIndex, delimiter.Length), delimiter, ignoreCase) == 0))
                    {
                        values.Add(expression.Substring
                                       (startIndex, charIndex - startIndex));
                        startIndex = charIndex + 1;
                    }
            }

            if (startIndex < expression.Length)
                values.Add(expression.Substring
                               (startIndex, expression.Length - startIndex));

            var returnValues = new string[values.Count];
            values.CopyTo(returnValues);
            return returnValues;
        }

        public static string EscapeString(string str)
        {
            return str == null ? null : Regex.Replace(str, @"[\r\n\x00\x1a\\'""]", @"\$0");
        }

        /// <summary>
        ///   Convert html chars to HTML entities
        /// </summary>
        /// <param name = "str"></param>
        /// <returns></returns>
        public static string HtmlEntities(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            return str;
        }

        /// <summary>
        ///   Return a string with the first letter upper
        /// </summary>
        /// <param name = "str"></param>
        /// <returns></returns>
        public static string FirstLetterUpper(string str)
        {
            char[] letters = str.ToCharArray();
            letters[0] = char.ToUpper(letters[0]);

            return new string(letters);
        }

        public static string EncryptPassword(string password, string key)
        {
            return GetMd5(GetMd5(password) + key);
        }

        public static string GetMd5(string encryptString)
        {
            byte[] passByteCrypt = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(encryptString));

            return ByteArrayToString(passByteCrypt);
        }

        public static string ByteArrayToString(byte[] byteCryptedMd5)
        {
            var output = new StringBuilder(byteCryptedMd5.Length);

            for (int i = 0; i < byteCryptedMd5.Length; i++)
            {
                output.Append(byteCryptedMd5[i].ToString("X2"));
            }

            return output.ToString().ToLower();
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

        public static string[] ToStringArr(IEnumerable collection)
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