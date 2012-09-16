using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using Stump.Core.Collections;
using Stump.Core.Cryptography;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.Core.Threading;
using Stump.Core.Timers;
using Stump.DofusProtocol.Enums;
using Stump.Server.AuthServer.Database;
using Stump.Server.AuthServer.Network;
using Stump.Server.BaseServer.Database;
using DatabaseAccessor = Stump.Server.AuthServer.Database.DatabaseAccessor;

namespace Stump.Server.AuthServer.Managers
{
    public class AccountManager : DataManager<DatabaseAccessor, AccountManager>
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


        [Variable]
        public static int CacheTimeout = 300;


        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<int, Tuple<DateTime, Account>> m_accountsCache = new Dictionary<int, Tuple<DateTime, Account>>(CacheTimeout);
        private SimpleTimerEntry m_timer;

        public AccountManager()
        {
            
        }

        public override void Initialize()
        {
            m_timer = AuthServer.Instance.IOTaskPool.CallPeriodically(CacheTimeout * 60 / 4, TimerTick);
        }

        public override void TearDown()
        {
            AuthServer.Instance.IOTaskPool.CancelSimpleTimer(m_timer);
        }

        private void TimerTick()
        {
            var toRemove = new List<int>();

            foreach (var tuple in m_accountsCache.Values)
            {
                if (tuple.Item1 >= DateTime.Now)
                    toRemove.Add(tuple.Item2.Id);
            }

            foreach (var id in toRemove)
            {
                m_accountsCache.Remove(id);
            }
        }

        public Account FindAccountById(int id)
        {
            return Database.Accounts.Find(id);
        }

        public Account FindAccountByLogin(string login)
        {
            return Database.Accounts.FirstOrDefault(entry => entry.Login == login);
        }

        public Account FindAccountByNickname(string nickname)
        {
            return Database.Accounts.FirstOrDefault(entry => entry.Nickname == nickname);
        }

        public IpBan FindIpBan(string ip)
        {
            return Database.IpBans.FirstOrDefault(entry => entry.IPAsString == ip);
        }

        public IpBan FindMatchingIpBan(string ipStr)
        {
            var ip = IPAddress.Parse(ipStr);
            var bans = Database.IpBans.Where(entry => entry.Match(ip));

            return bans.OrderByDescending(entry => entry.GetRemainingTime()).FirstOrDefault();
        }

        public void CacheAccount(Account account)
        {
            m_accountsCache.Add(account.Id, Tuple.Create(DateTime.Now + TimeSpan.FromSeconds(CacheTimeout), account));
        }

        public void UnCacheAccount(Account account)
        {
            m_accountsCache.Remove(account.Id);
        }

        public Account FindCachedAccountByTicket(string ticket)
        {
            var accounts = m_accountsCache.Values.Where(entry => entry.Item2.Ticket == ticket).ToArray();

            if (accounts.Count() > 1)
            {
                foreach (var conflictedAccount in accounts)
                {
                    conflictedAccount.Item2.Ticket = string.Empty;
                    UnCacheAccount(conflictedAccount.Item2);
                }

                return null;
            }

            var result = accounts.SingleOrDefault();

            return result != null ? result.Item2 : null;
        }

        public bool LoginExists(string login)
        {
            return Database.Accounts.Any(entry => entry.Login == login);
        }

        public bool NicknameExists(string nickname)
        {
            return Database.Accounts.Any(entry => entry.Nickname == nickname);
        }

        public bool CreateAccount(Account account)
        {
            if (LoginExists(account.Login))
                return false;

            Database.Accounts.Add(account);
            Database.SaveChanges();

            return true;
        }

        public bool DeleteAccount(Account account)
        {
            Database.Accounts.Remove(account);
            Database.SaveChanges();

            return true;
        }

        public WorldCharacter CreateAccountCharacter(Account account, WorldServer world, int characterId)
        {
            if (account.WorldCharacters.Any(entry => entry.CharacterId == characterId))
                return null;

            var character = new WorldCharacter
                                {
                                    Account = account,
                                    WorldId = world.Id,
                                    CharacterId = characterId
                                };

            account.WorldCharacters.Add(character);
            Database.SaveChanges();

            return character;
        }

        public bool DeleteAccountCharacter(Account account, WorldServer world, int characterId)
        {
            WorldCharacter character = Database.WorldCharacters.FirstOrDefault(c => c.CharacterId == characterId);

            if (character == null)
                return false;

            CreateDeletedCharacter(account, world, characterId);

            account.WorldCharacters.Remove(character);
            Database.WorldCharacters.Remove(character);


            return true;
        }

        public bool AddAccountCharacter(Account account, WorldServer world, int characterId)
        {
            WorldCharacter character = CreateAccountCharacter(account, world, characterId);


            return true;
        }

        public WorldCharacterDeleted CreateDeletedCharacter(Account account, WorldServer world, int characterId)
        {
            var character = new WorldCharacterDeleted
                                {
                                    Account = account,
                                    WorldId = world.Id,
                                    CharacterId = characterId
                                };

            account.WorldDeletedCharacters.Add(character);
            Database.SaveChanges();

            return character;
        }

        public bool DeleteDeletedCharacter(WorldCharacterDeleted deletedCharacter)
        {
            if (deletedCharacter == null)
                return false;

            deletedCharacter.Account.WorldDeletedCharacters.Remove(deletedCharacter);
            Database.SaveChanges();

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

            var lastConnection = account.GetLastConnection();
            if (lastConnection != null && lastConnection.WorldId.HasValue)
            {
                bool disconnected = false;
                var server = WorldServerManager.Instance.GetServerById(lastConnection.WorldId.Value);

                if (server != null && server.Connected && server.RemoteOperations != null)
                    if (server.RemoteOperations.DisconnectClient(account.Id))
                        disconnected = true;

                // diconnect clients from last game server
                if (disconnected)
                {
                }
                    return true;
            }

            return clients.Length > 0;
        }
    }
}