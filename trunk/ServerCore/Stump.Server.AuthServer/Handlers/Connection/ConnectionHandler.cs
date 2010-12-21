// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.BaseCore.Framework.Utils;
using Stump.Database;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.AuthServer.Accounts;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.AuthServer.Handlers
{
    public partial class ConnectionHandler : AuthHandlerContainer
    {
        public ConnectionHandler()
        {
            Predicates = new Dictionary<Type, Predicate<AuthClient>>
                {
                    {typeof (ServerSelectionMessage), Handlers.PredicatesDefinitions.IsLookingOfServers},
                };
        }

        #region Identification

        [AuthHandler(typeof(IdentificationMessage))]
        public static void HandleIdentificationMessage(AuthClient client, IdentificationMessage message)
        {
            /* Handle common identification */
            if (!HandleIndentification(client, message))
                return;

            /* If autoconnect, send to the lastServer */
            if (message.autoconnect && client.Account.LastServer.HasValue && WorldServerManager.CanAccessToWorld(client,(int)client.Account.LastServer))
            {
                SendSelectServerData(client, WorldServerManager.GetWorldRecord((int)client.Account.Id));
            }
            else
            {
                SendServersListMessage(client);
            }

        }

        [AuthHandler(typeof(IdentificationWithServerIdMessage))]
        public static void HandleIdentificationMessageWithServerIdMessage(AuthClient client, IdentificationWithServerIdMessage message)
        {
            /* Handle common identification */
            HandleIndentification(client, message);

            /* If world exist and connected */
            if (WorldServerManager.CanAccessToWorld(client,message.serverId))
            {
                SendSelectServerData(client,WorldServerManager.GetWorldRecord(message.serverId));
            }
            else
            {
                SendServersListMessage(client);
            }

        }

        public static bool HandleIndentification(AuthClient client, IdentificationMessage message)
        {
            /* Wrong Version */
            if (!ClientVersion.ClientVersionRequired.CompareVersion(message.version))
            {
                SendIdentificationFailedForBadVersionMessage(client,ClientVersion.ClientVersionRequired.ToVersion());
                client.DisconnectLater(1000);
                return false;
            }

            /* Bind Login and Pass to Client */
            client.Login = StringUtils.EscapeString(message.login);
            client.Password = StringUtils.EscapeString(message.password);

            /* Get corresponding account */
            AccountRecord account = AccountManager.GetAccountByName(client.Login);

            /* Invalid password */
            if (account == null || StringUtils.EncryptPassword(account.Password, client.Key) != client.Password)
            {
                SendIdentificationFailedMessage(client, IdentificationFailureReasonEnum.WRONG_CREDENTIALS);
                client.DisconnectLater(1000);
                return false;
            }

            /*Banni */
            if (account.Banned)
            {
                /* Ban indéterminé */
                if (!account.BanDate.HasValue)
                {
                    SendIdentificationFailedMessage(client, IdentificationFailureReasonEnum.BANNED);
                }
                /* Avec durée */
                else
                {
                    SendIdentificationFailedBannedMessage(client);
                }
                client.DisconnectLater(1000);
                return false;
            }


            bool wasAlreadyConnected = AuthentificationServer.Instance.DisconnectClientsUsingAccount(account);

            /* Already connected on this account */
            if (wasAlreadyConnected)
            {
                client.Send(new AlreadyConnectedMessage());
                return false;
            }

            client.Account = account;
            client.Characters = AccountManager.GetCharactersByAccount(account.Id);

            /* Propose at client to give a nickname */
            if (client.Account.Nickname == "")
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
                            client.Account.Nickname,
                            client.Account.Id,
                            0, // community ID ? ( se trouve dans le d2p, utilisé pour trouver les serveurs de la communauté )
                            client.Account.Role >= RoleEnum.Moderator,
                            client.Account.SecretQuestion,
                            client.Account.GetRegistrationRemainingTime(),
                            wasAlreadyConnected));

            client.LookingOfServers = true;
        }

        public static void SendIdentificationFailedMessage(AuthClient client, IdentificationFailureReasonEnum reason)
        {
            client.Send(new IdentificationFailedMessage((uint)reason));
        }

        public static void SendIdentificationFailedForBadVersionMessage(AuthClient client,DofusProtocol.Classes.Version version)
        {
            client.Send(new IdentificationFailedForBadVersionMessage((uint)IdentificationFailureReasonEnum.BAD_VERSION,version));
        }

        public static void SendIdentificationFailedBannedMessage(AuthClient client)
        {
            client.Send(new IdentificationFailedBannedMessage(
                                        (uint)IdentificationFailureReasonEnum.BANNED, (uint)client.Account.GetBanRemainingTime()));
        }

        #endregion

        #region Server Selection

        [AuthHandler(typeof(ServerSelectionMessage))]
        public static void HandleServerSelectionMessage(AuthClient client, ServerSelectionMessage message)
        {
            var wr = WorldServerManager.GetWorldRecord(message.serverId);
           
            /* World not exist */
            if (wr == null)
            {
                SendSelectServerRefusedMessage(client, wr, ServerConnectionErrorEnum.SERVER_CONNECTION_ERROR_NO_REASON);
                return;
            }

            /* Wrong state */
            if (wr.Status != ServerStatusEnum.ONLINE)
            {
                SendSelectServerRefusedMessage(client, wr, ServerConnectionErrorEnum.SERVER_CONNECTION_ERROR_DUE_TO_STATUS);
                return;
            }

            /* not suscribe */
            if (wr.RequireSubscription && client.Account.GetRegistrationRemainingTime() <= 0)
            {
                SendSelectServerRefusedMessage(client, wr, ServerConnectionErrorEnum.SERVER_CONNECTION_ERROR_SUBSCRIBERS_ONLY);
                return;
            }

            /* not the rights */
            if (wr.RequiredRole > client.Account.Role)
            {
                SendSelectServerRefusedMessage(client, wr, ServerConnectionErrorEnum.SERVER_CONNECTION_ERROR_ACCOUNT_RESTRICTED);
                return;
            }

            /* Send client to the server */
            SendSelectServerData(client, wr);

        }

        public static void SendSelectServerData(AuthClient client, WorldRecord world)
        {
            /* Check if is null */
            if (world == null)
                return;

            client.LookingOfServers = false;

            // save the ticket in the database
            client.Account.Ticket = client.Key;
            client.Account.LastServer = client.SelectedServerId;
            client.Save();

            client.Send(new SelectedServerDataMessage(
                            world.Id,
                            world.Ip,
                            world.Port,
                            ( client.Account.Role >= world.RequiredRole),
                            client.Key));

            client.Disconnect();
        }

        public static void SendSelectServerRefusedMessage(AuthClient client, WorldRecord world, ServerConnectionErrorEnum reason)
        {
            client.Send(new SelectedServerRefusedMessage(world.Id, (uint)reason, (uint)world.Status));
        }

        public static void SendServersListMessage(AuthClient client)
        {
            client.Send(new ServersListMessage(WorldServerManager.GetServersInformationList(client)));
        }

        #endregion

    }
}