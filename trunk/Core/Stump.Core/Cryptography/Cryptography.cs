using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Stump.Core.Cryptography
{
    public static class Cryptography
    {
        #region MD5

        /// <summary>
        ///   Get the md5 from a string
        /// </summary>
        /// <param name = "input">String input</param>
        /// <returns>MD5 Hash</returns>
        public static string GetMD5Hash(string input)
        {
            var md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            var sBuilder = new StringBuilder();

            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2", CultureInfo.CurrentCulture));
            }

            return sBuilder.ToString();
        }

        public static string GetFileMD5Hash(string fileName)
        {
            String md5Result;
            var sb = new StringBuilder();
            var md5Hasher = MD5.Create();

            using (var fs = File.OpenRead(fileName))
            {
                foreach (var b in md5Hasher.ComputeHash(fs))
                    sb.Append(b.ToString("x2").ToLower());
            }

            md5Result = sb.ToString();

            return md5Result;
        }

        public static string GetFileMD5HashBase64(string fileName)
        {
            using (var md5Hasher = MD5.Create())
            {
                return Convert.ToBase64String(md5Hasher.ComputeHash(File.ReadAllBytes(fileName)));
            }
        }

        /// <summary>
        ///   Check if the given hash equals to the hash of the given string
        /// </summary>
        /// <param name = "chaine">String</param>
        /// <param name = "hash">MD5 hash to check</param>
        /// <returns></returns>
        public static bool VerifyMD5Hash(string chaine, string hash)
        {
            var hashOfInput = GetMD5Hash(chaine);

            var comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }

        #endregion

        #region RSA

        public static string EncryptRSA(string encryptValue, RSAParameters parameters)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(parameters);

            var bytesToEncrypt = Encoding.UTF8.GetBytes(encryptValue);
            var bytesEncrypted = rsa.Encrypt(bytesToEncrypt, false);

            var encryptedValue = Convert.ToBase64String(bytesEncrypted);


            return encryptedValue;
        }

        public static string DecryptRSA(byte[] encryptedValue, RSAParameters parameters)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(parameters);

            var decryptedValue = Encoding.UTF8.GetString(rsa.Decrypt(encryptedValue, false));

            return decryptedValue;
        }
        #endregion
    }
}