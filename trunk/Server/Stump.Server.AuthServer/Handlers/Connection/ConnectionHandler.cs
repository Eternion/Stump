using System;
using Stump.Core.Attributes;
using Stump.Core.Cryptography;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.AuthServer.Database;
using Stump.Server.AuthServer.Managers;
using Stump.Server.AuthServer.Network;
using Stump.Server.BaseServer.Network;
using Version = Stump.DofusProtocol.Types.Version;

namespace Stump.Server.AuthServer.Handlers.Connection
{
    public partial class ConnectionHandler : AuthHandlerContainer
    {
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
            if (message.autoconnect && client.Account.LastConnectionWorld != null &&
                WorldServerManager.Instance.CanAccessToWorld(client, client.Account.LastConnectionWorld.Value))
            {
                SendSelectServerData(client, WorldServerManager.Instance.GetServerById(client.Account.LastConnectionWorld.Value));
            }
            else
            {
                SendServersListMessage(client);
            }
        }

        /*[AuthHandler(IdentificationWithServerIdMessage.Id)]
        public static void HandleIdentificationMessageWithServerIdMessage(AuthClient client, IdentificationWithServerIdMessage message)
        {
            // Handle common identification
            HandleIndentification(client, message);

            // If world exist and connected
            if (WorldServerManager.Instance.CanAccessToWorld(client, message.serverId))
            {
                SendSelectServerData(client, WorldServerManager.Instance.GetServerById(message.serverId));
            }
            else
            {
                SendServersListMessage(client);
            }
        }*/

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
            //client.Login = message.login.EscapeString();

            /* Get corresponding account */
            Account account = AccountManager.Instance.FindAccountByLogin(client.Login);

            /* Invalid password */
            if (account == null || !CredentialManager.Instance.CompareAccountPassword(account, message.credentials))
            {
                SendIdentificationFailedMessage(client, IdentificationFailureReasonEnum.WRONG_CREDENTIALS);
                client.DisconnectLater(1000);
                return false;
            }

            /* Check Sanctions */
            if (account.IsBanned && account.BanEndDate > DateTime.Now)
            {
                SendIdentificationFailedBannedMessage(client, account.BanEndDate.Value);
                client.DisconnectLater(1000);
                return false;
            }
            else if (account.IsBanned)
            {
                account.IsBanned = false;
                account.BanEndDate = null;
            }

            var ipBan = AccountManager.Instance.FindMatchingIpBan(client.IP);
            if (ipBan != null)
            {
                SendIdentificationFailedBannedMessage(client, ipBan.GetEndDate());
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
                            client.Account.Login,
                            client.Account.Nickname,
                            (int) client.Account.Id,
                            0, // community ID ? ( se trouve dans le d2p, utilisé pour trouver les serveurs de la communauté )
                            client.Account.SecretQuestion,
                            client.Account.SubscriptionEnd > DateTime.Now ? client.Account.SubscriptionEnd.GetUnixTimeStamp() : 0, 
                            (DateTime.Now - client.Account.CreationDate).TotalMilliseconds));

            client.LookingOfServers = true;
        }

        public static void SendIdentificationFailedMessage(AuthClient client, IdentificationFailureReasonEnum reason)
        {
            client.Send(new IdentificationFailedMessage((sbyte) reason));
        }

        public static void SendIdentificationFailedForBadVersionMessage(AuthClient client, Version version)
        {
            client.Send(new IdentificationFailedForBadVersionMessage((sbyte) IdentificationFailureReasonEnum.BAD_VERSION, version));
        }

        public static void SendIdentificationFailedBannedMessage(AuthClient client, DateTime date)
        {
            client.Send(new IdentificationFailedBannedMessage((sbyte) IdentificationFailureReasonEnum.BANNED, date.GetUnixTimeStamp()));
        }

        #endregion

        #region Server Selection

        [AuthHandler(ServerSelectionMessage.Id)]
        public static void HandleServerSelectionMessage(AuthClient client, ServerSelectionMessage message)
        {
            WorldServer world = WorldServerManager.Instance.GetServerById(message.serverId);

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
            if (world.RequireSubscription && client.Account.SubscriptionEnd <= DateTime.Now)
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
            AccountManager.Instance.CacheAccount(client.Account);

            client.Account.LastConnection = DateTime.Now;
            client.Account.LastConnectedIp = client.IP;
            client.Account.LastConnectionWorld = world.Id;
            client.SaveNow();

            client.Send(new SelectedServerDataMessage(
                            (short) world.Id,
                            world.Address,
                            world.Port,
                            (client.Account.Role >= world.RequiredRole),
                            client.Key));

            client.Disconnect();
        }

        public static void SendSelectServerRefusedMessage(AuthClient client, WorldServer world, ServerConnectionErrorEnum reason)
        {
            client.Send(new SelectedServerRefusedMessage((short) world.Id, (sbyte) reason, (sbyte) world.Status));
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