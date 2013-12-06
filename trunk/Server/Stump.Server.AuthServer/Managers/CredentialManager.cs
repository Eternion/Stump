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
using Stump.Core.IO;
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
            //return m_rsaPublicKey;
            // copy of the server public key
            return new sbyte[0];
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

        public bool DecryptCredentials(out Account account, IEnumerable<sbyte> credentials)
        {
            try
            {
                // old one
                /*account = null;
                var data = m_rsaProvider.Decrypt(credentials.Select(entry => (byte)entry).ToArray(), false);
                var reader = new FastBigEndianReader(data);

                if (reader.ReadUTFBytes((ushort) m_salt.Length) != m_salt)
                    return false;

                var userLength = reader.ReadByte();
                var username = reader.ReadUTFBytes(userLength);
                account = AccountManager.Instance.FindAccountByLogin(username);

                if (account == null)
                    return false;

                var password = reader.ReadUTFBytes((ushort) reader.BytesAvailable);

                return account.PasswordHash == password.GetMD5();*/

                account = null;
                var reader = new FastBigEndianReader(credentials.Select(x => (byte)x).ToArray());
                var username = reader.ReadUTF();
                var password = reader.ReadUTF();

                account = AccountManager.Instance.FindAccountByLogin(username);

                if (account == null)
                    return false;

                return account.PasswordHash == password.GetMD5();
            }
            catch (Exception)
            {
                account = null;
                return false;
            }
        }
    }
}