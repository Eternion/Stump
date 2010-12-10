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
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Stump.BaseCore.Framework.Utils
{
    public static class StringUtils
    {
        public static string EscapeString(string str)
        {
            return str == null ? null : Regex.Replace(str, @"[\r\n\x00\x1a\\'""]", @"\$0");
        }

        /// <summary>
        /// Convert html chars to HTML entities 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HtmlEntities(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            return str;
        }

        /// <summary>
        /// Return a string with the first letter upper
        /// </summary>
        /// <param name="str"></param>
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
    }
}