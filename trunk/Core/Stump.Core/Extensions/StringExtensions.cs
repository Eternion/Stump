using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Stump.Core.Collections;

namespace Stump.Core.Extensions
{
    public static class StringExtensions
    {
        public static string FirstLetterUpper(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            char[] letters = source.ToCharArray();
            letters[0] = char.ToUpper(letters[0]);

            return new string(letters);
        }

        public static string ConcatCopy(this string str, int times)
        {
            var builder = new StringBuilder(str.Length * times);

            for (int i = 0; i < times; i++)
            {
                builder.Append(str);
            }

            return builder.ToString();
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

        public static string[] SplitAdvanced(this string expression, string delimiter)
        {
            return SplitAdvanced(expression, delimiter, "", false);
        }

        public static string[] SplitAdvanced(this string expression, string delimiter,
                                     string qualifier)
        {
            return SplitAdvanced(expression, delimiter, qualifier, false);
        }

        public static string[] SplitAdvanced(this string expression, string delimiter,
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
                        qualifierState = !( qualifierState );
                    }
                    else if (!( qualifierState ) & ( delimiter != null )
                             & ( string.Compare(expression.Substring
                                                   (charIndex, delimiter.Length), delimiter, ignoreCase) == 0 ))
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

        public static string EscapeString(this string str)
        {
            return str == null ? null : Regex.Replace(str, @"[\r\n\x00\x1a\\'""]", @"\$0");
        }

        /// <summary>
        ///   Convert html chars to HTML entities
        /// </summary>
        /// <param name = "str"></param>
        /// <returns></returns>
        public static string HtmlEntities(this string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            return str;
        }

        public static int CountOccurences(this string str, char chr, int startIndex, int count)
        {
            int occurences = 0;

            for (int i = startIndex; i < startIndex + count; i++)
            {
                if (str[i] == chr)
                    occurences++;
            }

            return occurences;
        }

        public static string GetMD5(this string encryptString)
        {
            byte[] passByteCrypt = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(encryptString));

            return passByteCrypt.ByteArrayToString();
        }
    }
}