using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NHibernate.Criterion;
using NLog;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Stump.Core.Attributes;
using Stump.Core.Cryptography;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.AuthServer.Database.Account;
using Stump.Server.AuthServer.Database.World;
using Stump.Server.AuthServer.IPC;
using Stump.Server.AuthServer.Network;
using Stump.Server.BaseServer.IPC;

namespace Stump.Server.AuthServer.Managers
{
    public class AccountManager : Singleton<AccountManager>
    {
        public const string LoginPublicKey =
            "-----BEGIN PUBLIC KEY-----\n" +
            "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA2B4zCXNIlO/u2i06zyxX\n" +
            "HFXiDMioMqYVty8xEi/ZWB1vaMdnroW6DcujRQfDoycNAtv5vmkb/x5DIgkRv/3S\n" +
            "m5yYn3rj/xyozSb5GAymhg80F0bFwGLtHj95mDWZX9pyegCqpSSijYGoImEhdoYT\n" +
            "ThM8HyC2yPlsVYJEj3Me6eu9DK+zF+ozR5Wrw0nzt3GjqS1bEOJi5ORT1OeqHACo\n" +
            "NYgA3fS27jgH75rsZ39H8NMsGaIHr3zZfyOiY2aflsee08/QOoIu/myMIBucHsHp\n" +
            "Qtzp6kHQP7uolwpyXZZlCQXzx1fJ0aqek0EgFR3P9AdpluNqNdlDUEDKimXBOCMU\n" +
            "GwIDAQAB\n" +
            "-----END PUBLIC KEY-----";

        public const string a = 
            "-----BEGIN PUBLIC KEY-----\n" +
            "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA2B4zCXNIlO/u2i06zyxX\n" +
            "HFXiDMioMqYVty8xEi/ZWB1vaMdnroW6DcujRQfDoycNAtv5vmkb/x5DIgkRv/3S\n" +
            "m5yYn3rj/xyozSb5GAymhg80F0bFwGLtHj95mDWZX9pyegCqpSSijYGoImEhdoYT\n" +
            "ThM8HyC2yPlsVYJEj3Me6eu9DK+zF+ozR5Wrw0nzt3GjqS1bEOJi5ORT1OeqHACo\n" +
            "NYgA3fS27jgH75rsZ39H8NMsGaIHr3zZfyOiY2aflsee08/QOoIu/myMIBucHsHp\n" +
            "Qtzp6kHQP7uolwpyXZZlCQXzx1fJ0aqek0EgFR3P9AdpluNqNdlDUEDKimXBOCMU\n" +
            "GwIDAQAB\n" +
            "-----END PUBLIC KEY-----";



        public const string LoginPrivateKey =
            "-----BEGIN RSA PRIVATE KEY-----\n" +
            "MIIEpgIBAAKCAQEA2B4zCXNIlO/u2i06zyxXHFXiDMioMqYVty8xEi/ZWB1vaMdn\n" +
            "roW6DcujRQfDoycNAtv5vmkb/x5DIgkRv/3Sm5yYn3rj/xyozSb5GAymhg80F0bF\n" +
            "wGLtHj95mDWZX9pyegCqpSSijYGoImEhdoYTThM8HyC2yPlsVYJEj3Me6eu9DK+z\n" +
            "F+ozR5Wrw0nzt3GjqS1bEOJi5ORT1OeqHACoNYgA3fS27jgH75rsZ39H8NMsGaIH\n" +
            "r3zZfyOiY2aflsee08/QOoIu/myMIBucHsHpQtzp6kHQP7uolwpyXZZlCQXzx1fJ\n" +
            "0aqek0EgFR3P9AdpluNqNdlDUEDKimXBOCMUGwIDAQABAoIBAQCd28Evv/NeRrtS\n" +
            "xgLU3PBsFBdfexnQlRv7iA7x192LzURZZn3HLoquFPaXL4qpG5cXZZJQkPoQKQyk\n" +
            "SbebY014uLOKmfr7lvV9lGASPvtLpMNJ1ToQwrGsjHTGxy/WoftIPkBnDBFoBzLG\n" +
            "megVaO2k88vQeNbKidWlx9dIaLBF78C+UtV4W8EGfbe5q2IQQPijo3duB59l686l\n" +
            "2ycauqgj3WKh/pjg1l/G4vOVKpK4c1JaglnzEV5Earqsleu8jnfVy8cOo1R/uPH4\n" +
            "5CpN3zYDTrmQ/7D4R4zURjgi4YYh9PUSxNCO30qJ7lXb76ExGspBnfcQt2CFE0aE\n" +
            "sn1qyJIxAoGBAP1lI4LsyZbR8t7AMJ7Xp7CD5KBQYQ2vfQhPizOxf4OqSq5HhDsx\n" +
            "Na8mBLvfuDdq2w4UzAWCuWR1J+8NRUoFEUGFcYaLshbQwqEnkyMd+qhc9HMELLWo\n" +
            "t53THXR+Q+fPL1vWPjRBPlKLyOw2HhWKru7pS1pIkbfXkbHxapxNqZHXAoGBANpW\n" +
            "9VEP1hZYfIJhmYJrCl1kjxRlEAa/dM+tkS/YzJMMZFD0XKNml6pQQx/w2cWVkC74\n" +
            "7Y7N3WMPj52FYV4G4calYR4bS+7gfK1M+XG61NvlfKtPEGXgasoBE+kxGlTn+o6z\n" +
            "xZLZYPZ34EA5HMV/u09u+EmE63hd2YcgXGjhPo9dAoGBALDOARk5Xu99TpleQI6U\n" +
            "qszfOochjpad+//wgJBxKSgVikZQYFNs4qhzPppYX5FLXc1VdYXi0LjnhhWmjNI4\n" +
            "9vFgyvW2Q2zn/OW1V1UJdfxD38zg/NFEB9p3k/XUpEz6o3DQ7FZJr9Ko9mja2eLu\n" +
            "AWFyJsG4IPTF1ULz0A9/oPHBAoGBALS9ZUecL2HCEBeyCWxfhW34L0T3wAOF+4Fr\n" +
            "MZOFCRv1Fxm4nvMYmxX+aQKI0wzvmTJ5F9Wt7sTw/bas0gQO+FkDT9inSf1NUYf3\n" +
            "/0m5GjmJx+Dbizx6QIxFxiC0aBK/EbeNc1Dzp4N/imA/puSKrxi7SMc7Q90Y+1gT\n" +
            "XsvnIW11AoGBAOKUZ90FY0Zb64Qf0zc7l91GpdAyPs0/npL0bwfqMV+PEJ2QgqhG\n" +
            "G/TwP63GQFo0/xHP1pxrq2V4OPdPcmkSXA5qx+JaKLkTImffPJEzofINejSQvYu2\n" +
            "AeSysP1GiHMz8x4Nrj50Z819LOvfiu1uJmpT5jbx5k+PJ5j9MOTqknSz\n" +
            "-----END RSA PRIVATE KEY-----";

        /// <summary>
        /// List of available breeds
        /// </summary>
        [Variable]
        public static List<PlayableBreedEnum> AvailableBreeds = new List<PlayableBreedEnum>
                                                                    {
                                                                        PlayableBreedEnum.Feca,
                                                                        PlayableBreedEnum.Osamodas,
                                                                        PlayableBreedEnum.Enutrof,
                                                                        PlayableBreedEnum.Sram,
                                                                        PlayableBreedEnum.Xelor,
                                                                        PlayableBreedEnum.Ecaflip,
                                                                        PlayableBreedEnum.Eniripsa,
                                                                        PlayableBreedEnum.Iop,
                                                                        PlayableBreedEnum.Cra,
                                                                        PlayableBreedEnum.Sadida,
                                                                        PlayableBreedEnum.Sacrieur,
                                                                        PlayableBreedEnum.Pandawa,
                                                                        PlayableBreedEnum.Roublard,
                                                                        PlayableBreedEnum.Zobal,
                                                                    };


        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<uint, Account> m_accountsCache = new Dictionary<uint, Account>();
        private readonly object m_locker = new object();


        public Account FindAccount(string login)
        {
            Account account = Account.FindAccountByLogin(login);

            if (account == null)
                return null;

            Account cachedAccount;
            if (m_accountsCache.TryGetValue(account.Id, out cachedAccount))
            {
                m_accountsCache[account.Id] = account;
            }
            else
            {
                lock (m_locker)
                    m_accountsCache.Add(account.Id, account);
            }

            return account;
        }

        public bool CompareAccountPassword(Account account, string encryptedPassword, string loginKey)
        {
            return encryptedPassword == account.Password;
            /*var pemReader = new PemReader(new StringReader(LoginPrivateKey));
            var privateKey = ( (AsymmetricCipherKeyPair)pemReader.ReadObject() ).Private as RsaPrivateCrtKeyParameters;

            var parameters = new RSAParameters()
            {
                DP = privateKey.DP.ToByteArray(),
                DQ = privateKey.DQ.ToByteArray(),
                Q = privateKey.Q.ToByteArray(),
                InverseQ = privateKey.QInv.ToByteArray(),
                Modulus = privateKey.Modulus.ToByteArray(),
                P = privateKey.P.ToByteArray(),
                Exponent = privateKey.PublicExponent.ToByteArray(),
                D = privateKey.Exponent.ToByteArray(),
            };

            var pemReader2 = new PemReader(new StringReader(LoginPublicKey));
            var publicKey = ( (RsaKeyParameters)pemReader2.ReadObject() );


            var parameters2 = new RSAParameters()
            {
                Exponent = publicKey.Exponent.ToByteArray(),
                Modulus = publicKey.Modulus.ToByteArray(),
            };

            string realPassword = Cryptography.DecryptRSA(Convert.FromBase64String(encryptedPassword), parameters);

            return string.Concat(loginKey, account.Password) == realPassword;*/
        }

        public Account FindRegisteredAccountByTicket(string ticket)
        {
            Account account;
            lock (m_locker)
            {
                account = m_accountsCache.Values.Where(entry => entry.Ticket == ticket).SingleOrDefault();
            }

            return account;
        }

        public bool LoginExist(string login)
        {
            return Account.Exists(Restrictions.Eq("Login", login.ToLower()));
        }

        public bool NicknameExist(string nickname)
        {
            return Account.Exists(Restrictions.Eq("Nickname", nickname));
        }

        public bool CreateAccount(Account account)
        {
            if (LoginExist(account.Login.ToLower()))
                return false;

            account.Save();

            return true;
        }

        public bool DeleteAccount(Account account)
        {
            account.Delete();

            return true;
        }

        public WorldCharacter CreateAccountCharacter(Account account, WorldServer world, uint characterId)
        {
            var character = new WorldCharacter
                                {
                                    Account = account,
                                    World = world,
                                    CharacterId = characterId
                                };

            character.Create();

            return character;
        }

        public bool AddAccountCharacter(Account account, WorldServer world, uint characterId)
        {
            WorldCharacter character = CreateAccountCharacter(account, world, characterId);

            if (account.Characters.Contains(character))
                return false;

            account.Characters.Add(character);
            account.Update();

            return true;
        }

        public DeletedWorldCharacter AddDeletedCharacter(Account account, WorldServer world, uint characterId)
        {
            var character = new DeletedWorldCharacter
                                {
                                    Account = account,
                                    World = world,
                                    CharacterId = characterId
                                };

            character.Create();

            return character;
        }

        public bool DeleteAccountCharacter(Account account, WorldServer world, uint characterId)
        {
            WorldCharacter character = account.Characters.FirstOrDefault(c => c.CharacterId == characterId);

            if (character == null)
                return false;

            account.Characters.Remove(character);
            character.Delete();

            account.DeletedCharacters.Add(AddDeletedCharacter(account, world, characterId));
            account.Save();

            return true;
        }


        public bool DisconnectClientsUsingAccount(Account account)
        {
            AuthClient[] clients = AuthServer.Instance.FindClients(entry => entry.Account != null &&
                                                                            entry.Account.Id == account.Id).ToArray();

            // disconnect clients from auth server
            foreach (AuthClient client in clients)
            {
                client.Disconnect();
            }

            if (account.LastConnection != null)
            {
                WorldServer lastWorld = account.LastConnection.World;
                IRemoteOperationsWorld client = IpcServer.Instance.GetIpcClient(lastWorld.Id);

                // diconnect clients from last game server
                if (client != null && client.DisconnectConnectedAccount(account.Id))
                {
                    return true;
                }
            }

            return clients.Length > 0;
        }
    }
}