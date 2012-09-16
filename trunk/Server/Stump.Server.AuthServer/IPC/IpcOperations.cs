using System;
using System.Data.Objects;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.AuthServer.Database;
using Stump.Server.AuthServer.Managers;
using Stump.Server.BaseServer.IPC;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.AuthServer.IPC
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true)]
    public class IpcOperations : IRemoteAuthOperations
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly WorldServerManager Manager = WorldServerManager.Instance;

        public IpcOperations()
        {
            IContextChannel channel = OperationContext.Current.Channel;

            channel.Closed += OnDisconnected;
            channel.Faulted += OnDisconnected;

            var callbackCom = (ICommunicationObject) Callback;
            callbackCom.Closed += OnDisconnected;
            callbackCom.Faulted += OnDisconnected;
        }

        private DatabaseAccessor Database
        {
            get;
            set;
        }

        private AccountManager AccountManager
        {
            get;
            set;
        }

        public bool Connected
        {
            get;
            private set;
        }

        private IRemoteWorldOperations Callback
        {
            get { return OperationContext.Current.GetCallbackChannel<IRemoteWorldOperations>(); }
        }

        #region IRemoteAuthOperations Members

        public RegisterResultEnum RegisterWorld(WorldServerData serverData)
        {
            OperationContext context = OperationContext.Current;
            if (context == null)
            {
                return RegisterResultEnum.ContextNotFound;
            }

            IContextChannel channel = context.Channel;
            if (channel == null)
            {
                return RegisterResultEnum.ChannelNotFound;
            }

            string id = GetCurrentSessionId();
            RemoteEndpointMessageProperty endPoint = GetCurrentEndPoint();
            WorldServer server;

            RegisterResultEnum result;
            if (((result = Manager.RequestConnection(serverData, channel, endPoint, id)) != RegisterResultEnum.OK) ||
                (server = GetCurrentServer()) == null)
            {
                try
                {
                    channel.Abort();
                    channel.Close();
                }
                catch (Exception)
                {
                }

                return result != RegisterResultEnum.OK ? result : RegisterResultEnum.UnknownError;
            }

            Database = new DatabaseAccessor(AuthServer.DatabaseConfiguration.BuildConnection(), true);
            AccountManager = new AccountManager();
            AccountManager.ChangeDataSource(Database);
            AccountManager.Initialize();
            server.RemoteOperations = Callback;
            Connected = true;

            return RegisterResultEnum.OK;
        }

        public void UnRegisterWorld()
        {
            WorldServer server = GetCurrentServer();

            if (server != null)
                Manager.RemoveWorld(server);

            Connected = false;
        }

        public void ChangeState(ServerStatusEnum state)
        {
            Manager.ChangeWorldState(GetCurrentServer(), state);
        }

        public void UpdateConnectedChars(int value)
        {
            WorldServer server = GetCurrentServer();

            if (server.CharsCount == value)
                return;

            server.CharsCount = value;

            if (server.CharsCount >= server.CharCapacity &&
                server.Status == ServerStatusEnum.ONLINE)
            {
                Manager.ChangeWorldState(server, ServerStatusEnum.FULL);
            }

            if (server.CharsCount < server.CharCapacity &&
                server.Status == ServerStatusEnum.FULL)
            {
                Manager.ChangeWorldState(server, ServerStatusEnum.ONLINE);
            }

            AuthServer.Instance.SaveDatabaseChanges();
        }

        public AccountData GetAccountByTicket(string ticket)
        {
            Account account = AccountManager.Instance.FindCachedAccountByTicket(ticket);
            if (account == null)
                return null;

            return account.Serialize();
        }

        public AccountData GetAccountByNickname(string nickname)
        {
            Account account = AccountManager.FindAccountByNickname(nickname);
            return account != null ? account.Serialize() : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modifiedRecord"></param>
        /// <param name="wsi"></param>
        /// <returns></returns>
        /// <remarks>It only considers password, secret question & answer and role</remarks>
        public bool UpdateAccount(AccountData modifiedRecord)
        {
            Account account = AccountManager.FindAccountById(modifiedRecord.Id);
            if (account == null)
                return false;

            account.PasswordHash = modifiedRecord.PasswordHash;
            account.SecretQuestion = modifiedRecord.SecretQuestion;
            account.SecretAnswer = modifiedRecord.SecretAnswer;
            account.Role = modifiedRecord.Role;
            account.Tokens = modifiedRecord.Tokens;

            Database.SaveChanges();
            return true;
        }

        public bool CreateAccount(AccountData accountData)
        {
            var account = new Account
                              {
                                  Id = accountData.Id,
                                  Login = accountData.Login,
                                  PasswordHash = accountData.PasswordHash,
                                  Nickname = accountData.Nickname,
                                  Role = accountData.Role,
                                  AvailableBreeds = accountData.AvailableBreeds,
                                  Ticket = accountData.Ticket,
                                  SecretQuestion = accountData.SecretQuestion,
                                  SecretAnswer = accountData.SecretAnswer,
                                  Lang = accountData.Lang,
                                  Email = accountData.Email
                              };
            return AccountManager.CreateAccount(account);
        }

        public bool DeleteAccount(string accountname)
        {
            Account account = AccountManager.FindAccountByLogin(accountname);
            if (account == null)
                return false;

            AccountManager.Instance.DisconnectClientsUsingAccount(account);

            return AccountManager.DeleteAccount(account);
        }

        public bool AddAccountCharacter(int accountId, int characterId)
        {
            Account account = AccountManager.FindAccountById(accountId);
            WorldServer world = GetCurrentServer();

            if (account == null || world == null)
                return false;

            return AccountManager.AddAccountCharacter(account, world, characterId);
        }

        public bool DeleteAccountCharacter(int accountId, int characterId)
        {
            Account account = AccountManager.FindAccountById(accountId);
            WorldServer world = GetCurrentServer();

            if (account == null || world == null)
                return false;

            return AccountManager.Instance.DeleteAccountCharacter(account, world, characterId);
        }

        public bool UnBlamAccount(string victimAccountLogin)
        {
            Account victimAccount = Database.Accounts.FirstOrDefault(entry => entry.Login == victimAccountLogin);

            if (victimAccount == null)
                return false;

            victimAccount.Sanctions.Clear();

            Database.SaveChanges();

            return true;
        }

        public bool BlamAccount(string victimAccountLogin, int? bannerAccountId, TimeSpan duration, string reason)
        {
            Account victimAccount = Database.Accounts.FirstOrDefault(entry => entry.Login == victimAccountLogin);

            if (victimAccount == null)
                return false;

            var record = new Sanction
                             {
                                 Account = victimAccount,
                                 BannedBy = bannerAccountId,
                                 BanReason = reason,
                                 Duration = duration
                             };

            victimAccount.Sanctions.Add(record);

            Database.SaveChanges();
            return true;
        }

        public bool BlamAccount(int victimAccountId, int? bannerAccountId, TimeSpan duration, string reason)
        {
            Account victimAccount = AccountManager.Instance.FindAccountById(victimAccountId);

            if (victimAccount == null)
                return false;

            var record = new Sanction
                             {
                                 Account = victimAccount,
                                 BannedBy = bannerAccountId,
                                 BanReason = reason,
                                 Duration = duration
                             };

            victimAccount.Sanctions.Add(record);

            Database.SaveChanges();
            return true;
        }

        public bool BanIp(string ipToBan, int bannerAccountId, TimeSpan duration, string reason)
        {
            IpBan ipBan = Database.IpBans.FirstOrDefault(entry => entry.IPAsString == ipToBan);
            IPAddressRange ip = IPAddressRange.Parse(ipToBan);
            if (ipBan != null)
            {
                ipBan.BanReason = reason;
                ipBan.BannedBy = bannerAccountId;
                ipBan.Duration = duration;
            }
            else
            {
                var record = new IpBan
                                 {
                                     IP = ip,
                                     BannedBy = bannerAccountId,
                                     BanReason = reason,
                                     Duration = duration
                                 };
                Database.IpBans.Add(record);
            }
            Database.SaveChanges();
            return true;
        }

        #endregion

        private void OnDisconnected(object sender, EventArgs args)
        {
            lock (this)
            {
                if (!Connected)
                    return;

                WorldServer world = GetServerByChannel((IContextChannel) sender);
                logger.Warn("Channel closed or faulted");

                if (world != null)
                {
                    Manager.RemoveWorld(world);
                }
                else
                {
                    logger.Warn("A server has been disconnected but we cannot retrieve it");
                }

                Connected = false;
            }
        }

        private WorldServer GetServerByChannel(IContextChannel channel)
        {
            foreach (var server in Manager.Realmlist)
            {
                if (server.Value.Channel == channel)
                    return server.Value;
            }

            return null;
        }

        /// <summary>
        /// Returns the Id of the RealmServer that called the current method.
        /// Can only be used from remote IPC Channels.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentSessionId()
        {
            OperationContext context = OperationContext.Current;
            if (context == null)
            {
                return "";
            }
            IContextChannel channel = context.Channel;
            if (channel == null)
            {
                return "";
            }
            return channel.InputSession.Id;
        }

        /// <summary>
        /// Returns the RealmEntry that belongs to the Channel
        /// that is performing the current communication.
        /// Can only be used from remote IPC Channels.
        /// </summary>
        public WorldServer GetCurrentServer(bool _throw = true)
        {
            WorldServer server = Manager.GetServerBySessionId(GetCurrentSessionId());

            if (server == null)
            {
                server = Manager.Realmlist.FirstOrDefault(entry => entry.Value.RemoteOperations == Callback).Value;
            }

            if (server == null && _throw)
            {
                throw new Exception(string.Format("Server with id {0} cannot be found, it's certainly disconnected",
                                                  GetCurrentSessionId()));
            }

            return server;
        }

        public RemoteEndpointMessageProperty GetCurrentEndPoint()
        {
            return
                (RemoteEndpointMessageProperty)
                OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name];
        }
    }
}