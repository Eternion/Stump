using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Timers;
using Stump.DofusProtocol.Enums;
using Stump.Server.AuthServer.Database;
using Stump.Server.AuthServer.Network;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.AuthServer.Database.Accounts;

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
            PlayableBreedEnum.Steamer,
            PlayableBreedEnum.Eliotrope,
            PlayableBreedEnum.Huppermage
            };


        [Variable]
        public static int CacheTimeout = 300;

        [Variable]
        public static int BansRefreshTime = 60;

        static readonly Logger logger = LogManager.GetCurrentClassLogger();
        readonly Dictionary<string, Tuple<DateTime, Account>> m_accountsCache = new Dictionary<string, Tuple<DateTime, Account>>();
        List<IpBan> m_ipBans = new List<IpBan>();
        List<HardwareIdBan> m_hardwareIdBans = new List<HardwareIdBan>(); 
        TimedTimerEntry m_timer;
        TimedTimerEntry m_bansTimer;

        public AccountManager()
        {
            
        }

        public override void Initialize()
        {
            base.Initialize();
            m_timer = AuthServer.Instance.IOTaskPool.CallPeriodically(CacheTimeout * 60 / 4, TimerTick);
            m_bansTimer = AuthServer.Instance.IOTaskPool.CallPeriodically(BansRefreshTime * 1000, RefreshBans);
            m_ipBans = Database.Fetch<IpBan>(IpBanRelator.FetchQuery);
            m_hardwareIdBans = Database.Fetch<HardwareIdBan>(HardwareIdBanRelator.FetchQuery);
        }

        public override void TearDown()
        {
            AuthServer.Instance.IOTaskPool.RemoveTimer(m_timer);
            AuthServer.Instance.IOTaskPool.RemoveTimer(m_bansTimer);
        }

        void TimerTick()
        {
            var toRemove = (from keyPair in m_accountsCache where keyPair.Value.Item1 <= DateTime.Now select keyPair).ToList();

            foreach (var keyPair in toRemove)
            {
                m_accountsCache.Remove(keyPair.Key);
            }
        }

        void RefreshBans()
        {
            lock (m_ipBans)
            {
                m_ipBans.Clear();
                m_hardwareIdBans.Clear();

                m_ipBans.AddRange(Database.Query<IpBan>(IpBanRelator.FetchQuery));
                m_hardwareIdBans.AddRange(Database.Query<HardwareIdBan>(HardwareIdBanRelator.FetchQuery));
            }
        }

        public void AddIPBan(IpBan ban)
        {
            lock (m_ipBans)
            {
                m_ipBans.Add(ban);
            }
        }

        public void RemoveIPBan(IpBan ban)
        {
            lock (m_ipBans)
            {
                m_ipBans.Remove(ban);
            }
        }

        public void AddHardwareIdBan(HardwareIdBan ban)
        {
            lock (m_hardwareIdBans)
            {
                m_hardwareIdBans.Add(ban);
            }
        }

        public void RemoveHardwareIdBan(HardwareIdBan ban)
        {
            lock (m_hardwareIdBans)
            {
                m_hardwareIdBans.Remove(ban);
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

        public Account FindAccountByCharacterId(int characterId)
        {
            return Database.Query<Account, WorldCharacter, Account>(new AccountRelator().Map,
                string.Format(AccountRelator.FindAccountByCharacterId, characterId)).SingleOrDefault();
        }

        public IpBan FindIpBan(string ip)
        {
            return m_ipBans.FirstOrDefault(x => x.IPAsString == ip);
        }

        public HardwareIdBan FindHardwareIdBan(string hardwareId)
        {
            return m_hardwareIdBans.FirstOrDefault(x => x.HardwareId == hardwareId);
        }

        public IpBan FindMatchingIpBan(string ipStr)
        {
            var ip = IPAddress.Parse(ipStr);
            var bans = m_ipBans.Where(entry => entry.Match(ip));

            return bans.OrderByDescending(entry => entry.GetRemainingTime()).FirstOrDefault();
        }

        public UserGroupRecord FindUserGroup(int id)
            => Database.Query<UserGroupRecord>(string.Format(UserGroupRelator.FindUserById, id)).SingleOrDefault();

        public void CacheAccount(Account account)
        {
            if (m_accountsCache.ContainsKey(account.Ticket))
            {
                if (m_accountsCache[account.Ticket].Item2.Id != account.Id)
                {
                    throw new Exception("BE CAREFUL, two accounts have the same ticket");
                }

                m_accountsCache[account.Ticket] = Tuple.Create(DateTime.Now + TimeSpan.FromSeconds(CacheTimeout),
                    account);
            }
            else
            {
                var alreadyCachedAccounts = m_accountsCache.Where(x => x.Value.Item2.Id == account.Id).ToArray();

                foreach (var keyPair in alreadyCachedAccounts.ToArray())
                {
                    m_accountsCache.Remove(keyPair.Key);
                }

                m_accountsCache.Add(account.Ticket,
                    Tuple.Create(DateTime.Now + TimeSpan.FromSeconds(CacheTimeout), account));
            }
        }

        public void UnCacheAccount(Account account)
        {
            m_accountsCache.Remove(account.Ticket);
        }

        public Account FindCachedAccountByTicket(string ticket)
        {
            Tuple<DateTime, Account> tuple;
            return m_accountsCache.TryGetValue(ticket, out tuple) ? tuple.Item2 : null;
        }

        public bool LoginExists(string login) => Database.ExecuteScalar<bool>("SELECT EXISTS(SELECT 1 FROM accounts WHERE Login=@0)", login);

        public bool NicknameExists(string nickname) => Database.ExecuteScalar<bool>("SELECT EXISTS(SELECT 1 FROM accounts WHERE Nickname=@0)", nickname);

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

        public bool AddAccountCharacter(Account account, WorldServer world, int characterId)
            => CreateAccountCharacter(account, world, characterId) != null;

        public void DisconnectClientsUsingAccount(Account account, AuthClient except = null)
        {
            DisconnectClientsUsingAccount(account, except, result => { }, () => { }); // do nothing
        }

        public void DisconnectClientsUsingAccount(Account account, AuthClient except, Action<bool> callback, Action errorCallBack)
        {
            var clients = AuthServer.Instance.FindClients(entry => entry != except && entry.Account != null && entry.Account.Id == account.Id).ToArray();

            // disconnect clients from auth server
            foreach (var client in clients)
            {
                client.Disconnect();
            }

            if (account.LastConnectionWorld == null)
            {
                callback(false);
                return;
            }

            var server = WorldServerManager.Instance.GetServerById(account.LastConnectionWorld.Value);

            if (server != null && server.Connected && server.IPCClient != null)
            {
                server.IPCClient.SendRequest<DisconnectedClientMessage>(new DisconnectClientMessage(account.Id),
                    msg => callback(msg.Disconnected), msg => errorCallBack());
            }
            else
            {
                callback(false);
            }
        }
    }
}