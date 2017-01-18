using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.AuthServer.Database;
using Stump.Server.AuthServer.Managers;
using Stump.Server.AuthServer.Network;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.Network;
using Version = Stump.DofusProtocol.Types.Version;
using Stump.DofusProtocol.Messages.Custom;

namespace Stump.Server.AuthServer.Handlers.Connection
{
    public partial class ConnectionHandler : AuthHandlerContainer
    {
        public static SynchronizedCollection<AuthClient> ConnectionQueue = new SynchronizedCollection<AuthClient>();
        private static Task m_queueRefresherTask;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Initialization(InitializationPass.First)]
        private static void Initialize()
        {
            m_queueRefresherTask = Task.Factory.StartNewDelayed(3000, RefreshQueue);
        }
        
        // still thread safe theorically
        private static void RefreshQueue()
        {
            try
            {
                lock (ConnectionQueue.SyncRoot)
                {
                    var toRemove = new List<AuthClient>();
                    var count = 0;
                    foreach (var authClient in ConnectionQueue)
                    {
                        count++;

                        if (!authClient.Connected)
                        {
                            toRemove.Add(authClient);
                        }

                        if (DateTime.Now - authClient.InQueueUntil <= TimeSpan.FromSeconds(3))
                            continue;

                        SendQueueStatusMessage(authClient, (ushort) count, (ushort) ConnectionQueue.Count);
                        authClient.QueueShowed = true;
                    }

                    foreach (var authClient in toRemove)
                    {
                        ConnectionQueue.Remove(authClient);
                    }
                }
            }
            finally
            {
                m_queueRefresherTask = Task.Factory.StartNewDelayed(3000, RefreshQueue);
            }
        }

        /// <summary>
        /// Max Number of connection to logs in the database
        /// </summary>
        [Variable]
        public static uint MaxConnectionLogs = 5;

        #region Identification

        [AuthHandler(ClearIdentificationMessage.Id)]
        public static void HandleClearIdentificationMessage(AuthClient client, ClearIdentificationMessage message)
        {
            lock (ConnectionQueue.SyncRoot)
                ConnectionQueue.Remove(client);

            if (client.QueueShowed)
                SendQueueStatusMessage(client, 0, 0); // close the popup

            /* Invalid password */
            if (!CredentialManager.Instance.DecryptCredentials(out var account, message))
            {
                SendIdentificationFailedMessage(client, IdentificationFailureReasonEnum.WRONG_CREDENTIALS);
                client.DisconnectLater(1000);
                return;
            }

            /* Check ServerIP - AntiBot Mesure */
            if (!message.serverIp.Contains(AuthServer.CustomHost)) //Will cause problem with NAT as CustomHost will not be the public server IP
            {
                SendIdentificationFailedMessage(client, IdentificationFailureReasonEnum.BANNED);
                client.DisconnectLater(1000);
                return;
            }

            SendCredentialsAcknowledgementMessage(client);

            client.Account = account;
            /* Check Sanctions */
            if (account.IsBanned && account.BanEndDate > DateTime.Now)
            {
                SendIdentificationFailedBannedMessage(client, account.BanEndDate.Value);
                client.DisconnectLater(1000);
                return;
            }

            if (account.IsLifeBanned)
            {
                SendIdentificationFailedBannedMessage(client);
                client.DisconnectLater(1000);
                return;
            }

            if (account.BanEndDate < DateTime.Now)
            {
                account.IsBanned = false;
                account.IsJailed = false;
                account.BanEndDate = null;
            }

            var ipBan = AccountManager.Instance.FindMatchingIpBan(client.IP);
            if (ipBan != null && ipBan.GetRemainingTime() > TimeSpan.Zero)
            {
                SendIdentificationFailedBannedMessage(client, ipBan.GetEndDate());
                client.DisconnectLater(1000);
                return;
            }

            var hardwareIdBan = AccountManager.Instance.FindHardwareIdBan(message.hardwareId);
            if (hardwareIdBan != null)
            {
                SendIdentificationFailedBannedMessage(client);
                client.DisconnectLater(1000);
                return;
            }

            AccountManager.Instance.DisconnectClientsUsingAccount(account, client, success => AuthServer.Instance.IOTaskPool.AddMessage(() =>
            {
                // we must reload the record since it may have been modified
                if (success)
                    account = AccountManager.Instance.FindAccountById(account.Id);

                /* Bind Account to Client */
                client.Account = account;
                client.UserGroup = AccountManager.Instance.FindUserGroup(account.UserGroupId);
                client.Account.LastHardwareId = message.hardwareId;

                if (client.UserGroup == null)
                {
                    SendIdentificationFailedMessage(client, IdentificationFailureReasonEnum.UNKNOWN_AUTH_ERROR);
                    logger.Error("User group {0} doesn't exist !", client.Account.UserGroupId);
                    return;
                }

                /* Propose at client to give a nickname */
                if (client.Account.Nickname == string.Empty)
                {
                    client.Send(new NicknameRegistrationMessage());
                    return;
                }

                SendIdentificationSuccessMessage(client, false);

                /* If autoconnect, send to the lastServer */
                if (message.autoconnect && client.Account.LastConnectionWorld != null && WorldServerManager.Instance.CanAccessToWorld(client, client.Account.LastConnectionWorld.Value))
                    SendSelectServerData(client, WorldServerManager.Instance.GetServerById(client.Account.LastConnectionWorld.Value));
                else
                    SendServersListMessage(client, 0, true);
            }), () =>
            {
                client.Disconnect();
                logger.Error("Error while joining last used world server, connection aborted");
            });
        }

        [AuthHandler(IdentificationMessage.Id)]
        public static void HandleIdentificationMessage(AuthClient client, IdentificationMessage message)
        {
            /* Wrong Version */
            if (!message.version.IsUpToDate())
            {
                SendIdentificationFailedForBadVersionMessage(client, VersionExtension.ExpectedVersion);
                client.DisconnectLater(1000);
                return;
            }

            var patch = AuthServer.Instance.GetConnectionSwfPatch();
            if (patch != null)
                client.Send(new RawDataMessageFixed(patch));
        }

        public static void SendIdentificationSuccessMessage(AuthClient client, bool wasAlreadyConnected)
        {
            var creationDate = (DateTime.Now - client.Account.CreationDate).TotalMilliseconds;

            client.Send(new IdentificationSuccessMessage(
                client.UserGroup.IsGameMaster,
                wasAlreadyConnected,
                client.Account.Login,
                client.Account.Nickname,
                client.Account.Id,
                0,
                client.Account.SecretQuestion,
                client.Account.SubscriptionEnd > DateTime.Now
                    ? client.Account.SubscriptionEnd.GetUnixTimeStampLong()
                    : 0,
                0d,
                creationDate < 0 ? 0 : creationDate, 
                0));

            client.LookingOfServers = true;
        }

        public static void SendIdentificationFailedMessage(AuthClient client, IdentificationFailureReasonEnum reason)
        {
            client.Send(new IdentificationFailedMessage((sbyte) reason));
        }

        public static void SendIdentificationFailedForBadVersionMessage(AuthClient client, Version version)
        {
            client.Send(new IdentificationFailedForBadVersionMessage(
                (sbyte) IdentificationFailureReasonEnum.BAD_VERSION, version));
        }

        public static void SendIdentificationFailedBannedMessage(AuthClient client)
        {
            client.Send(new IdentificationFailedMessage((sbyte) IdentificationFailureReasonEnum.BANNED));
        }

        public static void SendIdentificationFailedBannedMessage(AuthClient client, DateTime date)
        {
            client.Send(new IdentificationFailedBannedMessage((sbyte)IdentificationFailureReasonEnum.BANNED,
                date.GetUnixTimeStampLong()));
        }

        public static void SendQueueStatusMessage(IPacketReceiver client, ushort position, ushort total)
        {
            client.Send(new LoginQueueStatusMessage(position, total));
        }

        #endregion

        #region Server Selection

        [AuthHandler(ServerSelectionMessage.Id)]
        public static void HandleServerSelectionMessage(AuthClient client, ServerSelectionMessage message)
        {
            var world = WorldServerManager.Instance.GetServerById(message.serverId);

            /* World not exist */
            if (world == null)
            {
                SendSelectServerRefusedMessage(client, message.serverId,
                    ServerConnectionErrorEnum.SERVER_CONNECTION_ERROR_NO_REASON);
                return;
            }

            /* Wrong state */
            if (world.Status != ServerStatusEnum.ONLINE)
            {
                SendSelectServerRefusedMessage(client, world,
                    ServerConnectionErrorEnum.SERVER_CONNECTION_ERROR_DUE_TO_STATUS);
                return;
            }

            /* not suscribe */
            if (world.RequireSubscription && client.Account.SubscriptionEnd <= DateTime.Now)
            {
                SendSelectServerRefusedMessage(client, world,
                    ServerConnectionErrorEnum.SERVER_CONNECTION_ERROR_SUBSCRIBERS_ONLY);
                return;
            }

            /* not the rights */
            if (world.RequiredRole > client.UserGroup.Role && !client.UserGroup.AvailableServers.Contains(world.Id))
            {
                SendSelectServerRefusedMessage(client, world,
                    ServerConnectionErrorEnum.SERVER_CONNECTION_ERROR_ACCOUNT_RESTRICTED);
                return;
            }

            /* Send client to the server */
            SendSelectServerData(client, world);
        }

        public static void SendSelectServerData(AuthClient client, WorldServer world)
        {
            /* Check if is null */
            if (world == null)
                return;

            client.LookingOfServers = false;

            /* Bind Ticket */
            client.Account.Ticket = new AsyncRandom().RandomString(32);
            AccountManager.Instance.CacheAccount(client.Account);

            client.Account.LastConnection = DateTime.Now;
            client.Account.LastConnectedIp = client.UserGroup.Role == RoleEnum.Administrator ? "127.0.0.1" : client.IP;
            client.Account.LastConnectionWorld = world.Id;
            client.SaveNow();

            client.Send(new SelectedServerDataMessage(
                (short) world.Id,
                world.Address,
                world.Port,
                (client.UserGroup.Role >= world.RequiredRole || client.UserGroup.AvailableServers.Contains(world.Id)),
                Encoding.ASCII.GetBytes(client.Account.Ticket).Select(x => (sbyte)x)));

            client.Disconnect();
        }

        public static void SendSelectServerRefusedMessage(AuthClient client, WorldServer world,
            ServerConnectionErrorEnum reason)
        {
            client.Send(new SelectedServerRefusedMessage((short) world.Id, (sbyte) reason, (sbyte) world.Status));
        }

        public static void SendSelectServerRefusedMessage(AuthClient client, short worldId,
            ServerConnectionErrorEnum reason)
        {
            client.Send(new SelectedServerRefusedMessage(worldId, (sbyte) reason,
                (sbyte) ServerStatusEnum.STATUS_UNKNOWN));
        }

        public static void SendServersListMessage(AuthClient client, short alreadyConnectedToServer, bool canCreateCharacter)
        {
            client.Send(new ServersListMessage(WorldServerManager.Instance.GetServersInformationArray(client), alreadyConnectedToServer, canCreateCharacter));
        }

        public static void SendServerStatusUpdateMessage(AuthClient client, WorldServer world)
        {
            if (world != null)
                client.Send(new ServerStatusUpdateMessage(WorldServerManager.Instance.GetServerInformation(client, world)));
        }

        public static void SendCredentialsAcknowledgementMessage(AuthClient client)
        {
            client.Send(new CredentialsAcknowledgementMessage());
        }

        #endregion
    }
}