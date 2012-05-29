using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using NLog;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;
using Stump.Core.Attributes;
using Stump.Core.Cryptography;
using Stump.Core.Extensions;
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

        private readonly RSACryptoServiceProvider m_rsaProvider = new RSACryptoServiceProvider();

        public readonly string Salt = new Random().RandomString(32);
        public readonly sbyte[] RsaPublicKey;

        private AccountManager()
        {
            RsaPublicKey = GenerateRSAPublicKey();
        }

        public sbyte[] GetRSAPublicKey()
        {
            return RsaPublicKey;
        }

        public string GetSalt()
        {
            return Salt;
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

                if (!str.StartsWith(Salt))
                    return false;

                var givenPass = str.Remove(0, Salt.Length);

                return account.PasswordHash == givenPass.GetMD5();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Account FindAccount(string login)
        {
            Account account = Account.FindAccountByLogin(login);

            if (account == null)
                return null;

            lock (m_locker)
            {
                Account cachedAccount;
                if (m_accountsCache.TryGetValue(account.Id, out cachedAccount))
                {
                    m_accountsCache[account.Id] = account;
                }
                else
                {
                    m_accountsCache.Add(account.Id, account);
                }
            }

            return account;
        }

        public IpBan FindIpBan(string ip)
        {
            return IpBan.FindByIp(ip);
        }

        public Account FindRegisteredAccountByTicket(string ticket)
        {
            Account account;
            lock (m_locker)
            {
                var accounts = m_accountsCache.Values.Where(entry => entry.Ticket == ticket).ToArray();

                if (accounts.Count() > 1)
                {
                    m_accountsCache.Clear();

                    return null;
                }

                return accounts.SingleOrDefault();
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
            using (var scope = new SessionScope(FlushAction.Never))
            {
                WorldCharacter character = CreateAccountCharacter(account, world, characterId);

                if (account.Characters.Contains(character))
                    return false;

                account.Characters.Add(character);

                scope.Flush();
            }

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
            account.Update();

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
                bool disconnected = false;
                var server = WorldServerManager.Instance.GetServerById(account.LastConnection.World.Id);

                if (server != null && server.Connected && server.RemoteOperations != null)
                    if (server.RemoteOperations.DisconnectClient(account.Id))
                        disconnected = true;

                // diconnect clients from last game server
                if (disconnected)
                {
                    return true;
                }
            }

            return clients.Length > 0;
        }
    }
}