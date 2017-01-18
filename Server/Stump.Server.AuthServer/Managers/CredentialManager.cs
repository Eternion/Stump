using System;
using System.Linq;
using NLog;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.Server.AuthServer.Database;
using Stump.DofusProtocol.Messages.Custom;

namespace Stump.Server.AuthServer.Managers
{
    public class CredentialManager : Singleton<CredentialManager>
    {
         static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public CredentialManager()
        {
            m_rsaPublicKey = GenerateRSAPublicKey();
        }

        readonly string m_salt = new Random().RandomString(32);
        readonly sbyte[] m_rsaPublicKey;

        public sbyte[] GetRSAPublicKey() => m_rsaPublicKey;

        public string GetSalt() => m_salt;

        static sbyte[] GenerateRSAPublicKey()
        {
            var publicKeyBase64 = "Vphs/8DzshWGwQ8ydffFVi8YtPqFGOfd3KWydc8ZdUFXhg8Npols4zwIT++s8z+/0Jqn6OI5i68uXgldDGB6zUX5a5RP9r7qgLFh4jYyywtkHeDv3CcPk2vekZkY9eaL+0AO50DUMsW6tyghFebWFyhkEck9CW7oqWVap99uRe/qXwk39LdrkNeFADdAPkO4infbVDQTy8EtozzBro5b9TuZSKiBUvfgUxR3kJ1u66N8IV5dB0guKmord1ZOYzhMokOMezkZ3ISBPltysSLwLFmYdpLfm/TvHaWcsfSmZjlWvtnXWowTssgqkmryVuVYrkB9ezcGIjXuQ7AYbnXXVIQb68VH02DfmPE15cBzqrUskcScH+lwIbuV0Yiy1XGXr4D0HERr7q5h87U5HLkedl8=";

            return Convert.FromBase64String(publicKeyBase64).Select(entry => (sbyte)entry).ToArray();
        }

        public bool DecryptCredentials(out Account account, ClearIdentificationMessage message)
        {
            try
            {
                account = null;

                var username = message.username;
                var password = message.password;

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