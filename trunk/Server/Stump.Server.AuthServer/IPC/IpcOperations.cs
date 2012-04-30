using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.AuthServer.Database.Account;
using Stump.Server.AuthServer.Database.World;
using Stump.Server.AuthServer.Managers;
using Stump.Server.BaseServer.IPC;
using Stump.Server.BaseServer.IPC.Objects;

namespace Stump.Server.AuthServer.IPC
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, IncludeExceptionDetailInFaults = true)]
    public class IpcOperations : IRemoteAuthOperations
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public WorldServerManager Manager = WorldServerManager.Instance;

        public IpcOperations()
        {
            IContextChannel channel = OperationContext.Current.Channel;

            channel.Closed += OnDisconnected;
            channel.Faulted += OnDisconnected;
        }

        private void OnDisconnected(object sender, EventArgs args)
        {
            WorldServer world = GetServerByChannel((IContextChannel) sender);

            if (world != null)
            {
                Manager.RemoveWorld(world);
            }
            else
            {
                logger.Warn("A server has been disconnected but we cannot retrieve it");
            }
        }

        private void OnOperationError(Exception ex)
        {
            WorldServer world = GetCurrentServer();

            if (ex is CommunicationException)
            {
                // Connection got interrupted
                logger.Warn("Lost connection to WorldServer {0}. Scheduling reconnection attempt...", world);
            }
            else
            {
                logger.Error("Exception occurs on IPC method access on WorldServer {0} : {1} \nScheduling reconnection attempt...", world, ex);
            }

            if (world != null)
                Manager.RemoveWorld(world);
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
        public WorldServer GetCurrentServer()
        {
            var server = Manager.GetServerBySessionId(GetCurrentSessionId());

            if (server == null)
            {
                throw new Exception(string.Format("Server with id {0} cannot be found, it's certainly disconnected", GetCurrentSessionId()));
            }

            return server;
        }

        public RemoteEndpointMessageProperty GetCurrentEndPoint()
        {
            return (RemoteEndpointMessageProperty) OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name];
        }

        public RegisterResultEnum RegisterWorld(WorldServerData serverData, string remoteIpcAddress)
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
                    channel.Close();
                }
                catch (Exception)
                {
                }

                return result != RegisterResultEnum.OK ? result : RegisterResultEnum.UnknownError;
            }

            try
            {
                var binding = new NetTcpBinding {Security = {Mode = SecurityMode.None}};
                var remoteClient = new WorldClientAdapter(binding, new EndpointAddress(remoteIpcAddress));
                remoteClient.Open();
                server.RemoteOperations = remoteClient;
                remoteClient.Error += OnOperationError;
            }
            catch (Exception)
            {
                logger.Error("Cannot retrieve remote object from server {0} localized at {1}", serverData.Name, remoteIpcAddress);
                Manager.RemoveWorld(server);

                return RegisterResultEnum.IpcConnectionFailed;
            }

            return RegisterResultEnum.OK;
        }

        public void UnRegisterWorld()
        {
            var server = GetCurrentServer();

            if (server != null)
                Manager.RemoveWorld(server);
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

            server.Update();
        }

        public AccountData GetAccountByTicket(string ticket)
        {
            Account account = AccountManager.Instance.FindRegisteredAccountByTicket(ticket);

            if (account == null)
                return null;

            return account.Serialize();
        }

        public AccountData GetAccountByNickname(string nickname)
        {
            return Account.FindAccountByNickname(nickname.ToLower()).Serialize();
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
            Account account = Account.FindAccountByNickname(modifiedRecord.Nickname);

            if (account == null)
                return false;

            account.PasswordHash = modifiedRecord.PasswordHash;
            account.SecretQuestion = modifiedRecord.SecretQuestion;
            account.SecretAnswer = modifiedRecord.SecretAnswer;
            account.Role = modifiedRecord.Role;
            account.Tokens = modifiedRecord.Tokens;

            account.Update();

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

            return AccountManager.Instance.CreateAccount(account);
        }

        public bool DeleteAccount(string accountname)
        {
            Account account = Account.FindAccountByLogin(accountname);

            if (account == null)
                return false;

            AccountManager.Instance.DisconnectClientsUsingAccount(account);

            return AccountManager.Instance.DeleteAccount(account);
        }

        public bool AddAccountCharacter(uint accountId, uint characterId)
        {
            Account account = Account.FindAccountById(accountId);
            WorldServer world = GetCurrentServer();

            if (account == null || world == null)
                return false;

            return AccountManager.Instance.AddAccountCharacter(account, world, characterId);
        }

        public bool DeleteAccountCharacter(uint accountId, uint characterId)
        {
            Account account = Account.FindAccountById(accountId);
            WorldServer world = GetCurrentServer();

            if (account == null || world == null)
                return false;

            return AccountManager.Instance.DeleteAccountCharacter(account, world, characterId);
        }

        public bool BlamAccountFrom(uint victimAccountId, uint bannerAccountId, TimeSpan duration, string reason)
        {
            Account victimAccount = Account.FindAccountById(victimAccountId);
            Account bannerAccount = Account.FindAccountById(bannerAccountId);

            if (victimAccount == null || bannerAccount == null)
                return false;

            var record = new Sanction
                             {
                                 Account = victimAccount,
                                 BannedBy = bannerAccount,
                                 BanReason = reason,
                                 Duration = duration
                             };
            record.Create();

            // todo : check if it is necessary
            victimAccount.Sanctions.Add(record);
            victimAccount.Update();

            return true;
        }


        public bool BlamAccount(uint victimAccountId, TimeSpan duration, string reason)
        {
            Account victimAccount = Account.FindAccountById(victimAccountId);

            if (victimAccount == null)
                return false;

            var record = new Sanction
                             {
                                 Account = victimAccount,
                                 BanReason = reason,
                                 Duration = duration
                             };
            record.Create();

            // todo : check if it is necessary
            victimAccount.Sanctions.Add(record);
            victimAccount.Update();

            return true;
        }

        public bool BanIp(string ipToBan, uint bannerAccountId, TimeSpan duration, string reason)
        {
            Account bannerAccount = Account.FindAccountById(bannerAccountId);

            if (bannerAccount == null)
                return false;

            var record = new IpBan
                             {
                                 Ip = ipToBan,
                                 BannedBy = bannerAccount,
                                 BanReason = reason,
                                 Duration = duration
                             };
            record.Create();

            return true;
        }
    }
}