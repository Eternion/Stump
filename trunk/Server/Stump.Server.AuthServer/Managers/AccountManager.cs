using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using Stump.Server.BaseServer.IPC.Messages;

namespace Stump.Server.AuthServer.Managers
{
    public class AccountManager : DataManager<AccountManager>
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

        [Variable]
        public static int IpBanRefreshTime = 60;


        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<int, Tuple<DateTime, Account>> m_accountsCache = new Dictionary<int, Tuple<DateTime, Account>>();
        private List<IpBan> m_ipBans = new List<IpBan>(); 
        private SimpleTimerEntry m_timer;
        private SimpleTimerEntry m_bansTimer;

        public AccountManager()
        {
            
        }

        public override void Initialize()
        {
            base.Initialize();
            m_timer = AuthServer.Instance.IOTaskPool.CallPeriodically(CacheTimeout * 60 / 4, TimerTick);
            m_bansTimer = AuthServer.Instance.IOTaskPool.CallPeriodically(IpBanRefreshTime * 1000, RefreshIpBans);
            m_ipBans = Database.Fetch<IpBan>(IpBanRelator.FetchQuery);
        }

        public override void TearDown()
        {
            AuthServer.Instance.IOTaskPool.CancelSimpleTimer(m_timer);
            AuthServer.Instance.IOTaskPool.CancelSimpleTimer(m_bansTimer);
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

        private void RefreshIpBans()
        {
            lock (m_ipBans)
            {
                m_ipBans.Clear();
                m_ipBans.AddRange(Database.Query<IpBan>(IpBanRelator.FetchQuery));
            }
        }

        public void AddIPBan(IpBan ban)
        {
            lock (m_ipBans)
            {
                m_ipBans.Add(ban);
            }
        }

        public Account FindAccountById(int id)
        {
            return Database.Query<Account, WorldCharacter, Account>(new AccountRelator().Map,
                string.Format(AccountRelator.FindAccountById, id)).SingleOrDefault();
        }

        public Account FindAccountByLogin(string login)
        {
            return Database.Query<Account, WorldCharacter, Account>(new AccountRelator().Map,
                AccountRelator.FindAccountByLogin, login).SingleOrDefault();
        }

        public Account FindAccountByNickname(string nickname)
        {
            return Database.Query<Account, WorldCharacter, Account>(new AccountRelator().Map,
                AccountRelator.FindAccountByNickname, nickname).SingleOrDefault();
        }

        public IpBan FindIpBan(string ip)
        {
            lock (m_ipBans)
            {
                return m_ipBans.FirstOrDefault(x => x.IPAsString == ip);
            }
        }

        public IpBan FindMatchingIpBan(string ipStr)
        {
            lock (m_ipBans)
            {
                var ip = IPAddress.Parse(ipStr);
                var bans = m_ipBans.Where(entry => entry.Match(ip));

                return bans.OrderByDescending(entry => entry.GetRemainingTime()).FirstOrDefault();
            }
        }

        public void CacheAccount(Account account)
        {
            if (m_accountsCache.ContainsKey(account.Id))
                m_accountsCache[account.Id] = Tuple.Create(DateTime.Now + TimeSpan.FromSeconds(CacheTimeout), account);
            else
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
            return Database.ExecuteScalar<bool>("SELECT EXISTS(SELECT 1 FROM accounts WHERE Login=@0)", login);
        }

        public bool NicknameExists(string nickname)
        {
            return Database.ExecuteScalar<bool>("SELECT EXISTS(SELECT 1 FROM accounts WHERE Nickname=@0)", nickname);
        }

        public bool CreateAccount(Account account)
        {
            if (LoginExists(account.Login))
                return false;

            Database.Insert(account);

            return true;
        }

        public bool DeleteAccount(Account account)
        {
            Database.Delete(account);

            return true;
        }

        public WorldCharacter CreateAccountCharacter(Account account, WorldServer world, int characterId)
        {
            if (account.WorldCharacters.Any(entry => entry.CharacterId == characterId))
                return null;

            var character = new WorldCharacter
                                {
                                    AccountId = account.Id,
                                    WorldId = world.Id,
                                    CharacterId = characterId
                                };

            account.WorldCharacters.Add(character);
            Database.Insert(character);

            return character;
        }

        public bool DeleteAccountCharacter(Account account, WorldServer world, int characterId)
        {
            var success = Database.Execute(string.Format("DELETE FROM worlds_characters WHERE AccountId={0} AND CharacterId={1} AND WorldId={2}", account.Id, characterId, world.Id)) > 0;

            if (!success)
                return false;

            CreateDeletedCharacter(account, world, characterId);
            account.WorldCharacters.RemoveAll(x => x.CharacterId == characterId && x.WorldId == world.Id);

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
                                    AccountId = account.Id,
                                    WorldId = world.Id,
                                    CharacterId = characterId
                                };

            Database.Insert(character);

            return character;
        }

        public bool DeleteDeletedCharacter(WorldCharacterDeleted deletedCharacter)
        {
            if (deletedCharacter == null)
                return false;

            Database.Delete(deletedCharacter);

            return true;
        }

        // todo : callback to know if an account has been disconnected
        public void DisconnectClientsUsingAccount(Account account)
        {
            AuthClient[] clients = AuthServer.Instance.FindClients(entry => entry.Account != null &&
                                                                            entry.Account.Id == account.Id).ToArray();

            // disconnect clients from auth server
            foreach (AuthClient client in clients)
            {
                client.Disconnect();
            }

            if (account.LastConnectionWorld != null)
            {
                var server = WorldServerManager.Instance.GetServerById(account.LastConnectionWorld.Value);

                if (server != null && server.Connected && server.IPCClient != null)
                {
                    server.IPCClient.Send(new DisconnectClientMessage(account.Id));
                }
            }
        }
    }
}