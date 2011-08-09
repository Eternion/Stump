using System;
using Stump.Core.Attributes;
using Stump.Core.Cryptography;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.AuthServer.Database.Account;
using Stump.Server.AuthServer.Database.World;
using Stump.Server.AuthServer.Managers;
using Stump.Server.AuthServer.Network;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.AuthServer.Handlers.Connection
{
    public partial class ConnectionHandler : AuthHandlerContainer
    {
        public ConnectionHandler()
        {
            Predicate(ServerSelectionMessage.Id, PredicatesDefinitions.IsLookingOfServers);
        }

        /// <summary>
        /// Max Number of connection to logs in the database
        /// </summary>
        [Variable]
        public static uint MaxConnectionLogs = 5;

        #region Identification

        [AuthHandler(IdentificationMessage.Id)]
        public static void HandleIdentificationMessage(AuthClient client, IdentificationMessage message)
        {
            /* Handle common identification */
            if (!HandleIndentification(client, message))
                return;

            /* If autoconnect, send to the lastServer */
            var lastConnection = client.Account.LastConnection;
            if (message.autoconnect && lastConnection != null && WorldServerManager.Instance.CanAccessToWorld(client, lastConnection.World))
            {
                SendSelectServerData(client, lastConnection.World);
            }
            else
            {
                SendServersListMessage(client);
            }
        }

        [AuthHandler(IdentificationWithServerIdMessage.Id)]
        public static void HandleIdentificationMessageWithServerIdMessage(AuthClient client, IdentificationWithServerIdMessage message)
        {
            /* Handle common identification */
            HandleIndentification(client, message);

            /* If world exist and connected */
            if (WorldServerManager.Instance.CanAccessToWorld(client, message.serverId))
            {
                SendSelectServerData(client, WorldServerManager.Instance.GetWorldServer(message.serverId));
            }
            else
            {
                SendServersListMessage(client);
            }

        }

        private static bool HandleIndentification(AuthClient client, IdentificationMessage message)
        {
            /* Wrong Version */
            if (!message.version.IsUpToDate())
            {
                SendIdentificationFailedForBadVersionMessage(client, VersionExtension.ExpectedVersion);
                client.DisconnectLater(1000);
                return false;
            }

            /* Bind Login and Pass to Client */
            client.Login = message.login.EscapeString();
            client.Password = message.password.EscapeString();

            /* Get corresponding account */
            var account = AccountManager.Instance.FindAccount(client.Login);

            /* Invalid password */
            if (account == null || Cryptography.EncryptPassword(account.Password, client.Key) != client.Password)
            {
                SendIdentificationFailedMessage(client, IdentificationFailureReasonEnum.WRONG_CREDENTIALS);
                client.DisconnectLater(1000);
                return false;
            }

            /* Check Sanctions */
            var banRemainingTime = account.BanRemainingTime;
            if (banRemainingTime > 0)
            {
                SendIdentificationFailedBannedMessage(client, banRemainingTime);
                client.DisconnectLater(1000);
                return false;
            }

            /* Already connected on this account */
            bool wasAlreadyConnected = AccountManager.Instance.DisconnectClientsUsingAccount(account);
            if (wasAlreadyConnected)
            {
                client.Send(new AlreadyConnectedMessage());
            }

            /* Bind Account to Client */
            client.Account = account;

            /* Propose at client to give a nickname */
            if (client.Account.Nickname == string.Empty)
            {
                client.Send(new NicknameRegistrationMessage());
                return false;
            }

            SendIdentificationSuccessMessage(client, wasAlreadyConnected);

            return true;
        }

        public static void SendIdentificationSuccessMessage(AuthClient client, bool wasAlreadyConnected)
        {
            client.Send(new IdentificationSuccessMessage(
                            client.Account.Role >= RoleEnum.Moderator,             
                            wasAlreadyConnected,
                            client.Account.Nickname,
                            (int)client.Account.Id,
                            0, // community ID ? ( se trouve dans le d2p, utilisé pour trouver les serveurs de la communauté )
                            client.Account.SecretQuestion,
                            client.Account.SubscriptionRemainingTime));
            client.LookingOfServers = true;
        }

        public static void SendIdentificationFailedMessage(AuthClient client, IdentificationFailureReasonEnum reason)
        {
            client.Send(new IdentificationFailedMessage((byte)reason));
        }

        public static void SendIdentificationFailedForBadVersionMessage(AuthClient client, DofusProtocol.Types.Version version)
        {
            client.Send(new IdentificationFailedForBadVersionMessage((byte)IdentificationFailureReasonEnum.BAD_VERSION, version));
        }

        public static void SendIdentificationFailedBannedMessage(AuthClient client, uint time)
        {
            client.Send(new IdentificationFailedBannedMessage((byte)IdentificationFailureReasonEnum.BANNED, (int) time));
        }

        #endregion

        #region Server Selection

        [AuthHandler(ServerSelectionMessage.Id)]
        public static void HandleServerSelectionMessage(AuthClient client, ServerSelectionMessage message)
        {
            var world = WorldServerManager.Instance.GetWorldServer(message.serverId);

            /* World not exist */
            if (world == null)
            {
                SendSelectServerRefusedMessage(client, world, ServerConnectionErrorEnum.SERVER_CONNECTION_ERROR_NO_REASON);
                return;
            }

            /* Wrong state */
            if (world.Status != ServerStatusEnum.ONLINE)
            {
                SendSelectServerRefusedMessage(client, world, ServerConnectionErrorEnum.SERVER_CONNECTION_ERROR_DUE_TO_STATUS);
                return;
            }

            /* not suscribe */
            if (world.RequireSubscription && client.Account.SubscriptionRemainingTime <= 0)
            {
                SendSelectServerRefusedMessage(client, world, ServerConnectionErrorEnum.SERVER_CONNECTION_ERROR_SUBSCRIBERS_ONLY);
                return;
            }

            /* not the rights */
            if (world.RequiredRole > client.Account.Role)
            {
                SendSelectServerRefusedMessage(client, world, ServerConnectionErrorEnum.SERVER_CONNECTION_ERROR_ACCOUNT_RESTRICTED);
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
            client.Account.Ticket = client.Key;

            /* Insert connection info */
            var connectionRecord = new ConnectionLog
                                       {
                                           World = world,
                                           Date = DateTime.Now,
                                           Account = client.Account,
                                           Ip = client.IP
                                       };
            connectionRecord.Create();
            client.Account.Connections.Add(connectionRecord);

            /* Remove the oldest Connection */
            if (client.Account.Connections.Count > MaxConnectionLogs)
                client.Account.RemoveOldestConnection();

            client.Save();

            client.Send(new SelectedServerDataMessage(
                            (short) world.Id,
                            world.Ip,
                            world.Port,
                            (client.Account.Role >= world.RequiredRole),
                            client.Key));

            client.Disconnect();
        }

        public static void SendSelectServerRefusedMessage(AuthClient client, WorldServer world, ServerConnectionErrorEnum reason)
        {
            client.Send(new SelectedServerRefusedMessage((short) world.Id, (byte) reason, (byte) world.Status));
        }

        public static void SendServersListMessage(AuthClient client)
        {
            client.Send(new ServersListMessage(WorldServerManager.Instance.GetServersInformationArray(client)));
        }

        public static void SendServerStatusUpdateMessage(AuthClient client, WorldServer world)
        {
            if (world != null)
                client.Send(new ServerStatusUpdateMessage(WorldServerManager.Instance.GetServerInformation(client, world)));
        }

        #endregion

    }
}