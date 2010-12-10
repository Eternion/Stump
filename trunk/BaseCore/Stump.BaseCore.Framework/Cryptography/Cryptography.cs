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
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Stump.BaseCore.Framework.Cryptography
{
    public static class Crypt
    {
        #region MD5

        /// <summary>
        ///   Get the md5 from a string
        /// </summary>
        /// <param name = "input">String input</param>
        /// <returns>MD5 Hash</returns>
        public static string GetMD5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2", CultureInfo.CurrentCulture));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        ///   Check if the given hash equals to the hash of the given string
        /// </summary>
        /// <param name = "chaine">String</param>
        /// <param name = "hash">MD5 hash to check</param>
        /// <returns></returns>
        public static bool VerifyMD5Hash(string chaine, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMD5Hash(chaine);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}