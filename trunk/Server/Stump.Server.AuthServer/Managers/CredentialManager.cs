using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.Server.AuthServer.Database;

namespace Stump.Server.AuthServer.Managers
{
    public class CredentialManager : Singleton<CredentialManager>
    {
        public CredentialManager()
        {
            m_rsaPublicKey = GenerateRSAPublicKey();
        }

        private readonly string m_salt = new Random().RandomString(32);
        private readonly sbyte[] m_rsaPublicKey;

        private readonly RSACryptoServiceProvider m_rsaProvider = new RSACryptoServiceProvider();

        public sbyte[] GetRSAPublicKey()
        {
            return m_rsaPublicKey;
        }

        public string GetSalt()
        {
            return m_salt;
        }

        private sbyte[] GenerateRSAPublicKey()
        {
            var exportParameters = m_rsaProvider.ExportParameters(false);
            var keyParameters = new RsaKeyParameters(false, new BigInteger(1, exportParameters.Modulus), new BigInteger(1, exportParameters.Exponent));

            var stringBuilder = new StringBuilder();
            var writer = new PemWriter(new StringWriter(stringBuilder));
            writer.WriteObject(keyParameters);

            string key = stringBuilder.ToString();

            string partial = key.Remove(key.IndexOf("-----END PUBLIC KEY-----")).Remove(0, "-----BEGIN PUBLIC KEY-----\n".Length);

            return Convert.FromBase64String(partial).Select(entry => (sbyte)entry).ToArray();
        }

        public bool CompareAccountPassword(Account account, IEnumerable<sbyte> credentials)
        {
            try
            {
                var data = m_rsaProvider.Decrypt(credentials.Select(entry => (byte)entry).ToArray(), false);
                var str = Encoding.ASCII.GetString(data);

                if (!str.StartsWith(m_salt))
                    return false;

                var givenPass = str.Remove(0, m_salt.Length);

                return account.PasswordHash == givenPass.GetMD5();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}