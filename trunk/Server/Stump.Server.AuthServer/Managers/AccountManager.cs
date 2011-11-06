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