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
    public class ConnectionHandler : AuthHandlerContainer
    {
        public ConnectionHandler()
        {
            Predicates = new Dictionary<Type, Predicate<AuthClient>>
                {
                    {typeof (ServerSelectionMessage), Handlers.Predicates.IsLookingOfServers},
                };
        }

        [AuthHandler(typeof (IdentificationMessage))]
        public static void HandleIdentificationMessage(AuthClient client, IdentificationMessage message)
        {
            if (!ClientVersion.ClientVersionRequired.CompareVersion(message.version))
            {
                SendIdentificationFailedMessage(client, IdentificationFailureReasonEnum.BAD_VERSION);
                client.Disconnect();
            }

            client.Login = StringUtils.EscapeString(message.login);
            client.Password = StringUtils.EscapeString(message.password);

            AccountRecord account = AccountManager.GetAccountByName(client.Login);

            if (account != null &&
                StringUtils.EncryptPassword(account.Password, client.Key) == client.Password)
            {
                bool wasAlreadyConnected = AuthentificationServer.Instance.DisconnectClientsUsingAccount(account);

                if (account.Banned)
                {
                    SendIdentificationFailedMessage(client, IdentificationFailureReasonEnum.BANNED);
                    return;
                }

                client.Account = account;
                client.Characters = AccountManager.GetCharactersByAccount(account.Id);

                /* Propose at client to give à nickname */
                if (client.Account.Nickname == "")
                {
                    client.Send(new NicknameRegistrationMessage());
                    return;
                }


                SendIdentificationSuccessMessage(client, wasAlreadyConnected);


                if (client.AutoConnect && WorldServerManager.Worlds.Count > 0 && client.Account.LastServer != 0)
                {
                    SendSelectServerData(client);

                    client.Disconnect();
                }
                else
                    SendServersListMessage(client);


                client.LookingOfServers = true;
            }
            else
            {
                SendIdentificationFailedMessage(client, IdentificationFailureReasonEnum.WRONG_CREDENTIALS);
            }
        }

        // called when client choose option "change character"
        [AuthHandler(typeof (IdentificationMessageWithServerIdMessage))]
        public static void HandleIdentificationMessageWithServerIdMessage(AuthClient client,
                                                                          IdentificationMessageWithServerIdMessage
                                                                              message)
        {
            if (!ClientVersion.ClientVersionRequired.CompareVersion(message.version))
            {
                SendIdentificationFailedMessage(client, IdentificationFailureReasonEnum.BAD_VERSION);
                client.Disconnect();
            }

            client.Login = StringUtils.EscapeString(message.login);
            client.Password = StringUtils.EscapeString(message.password);
            
            AccountRecord account = AccountManager.GetAccountByName(client.Login);

            if (account != null &&
                StringUtils.EncryptPassword(account.Password, client.Key) == client.Password)
            {
                bool wasAlreadyConnected = AuthentificationServer.Instance.DisconnectClientsUsingAccount(account);

                if (account.Banned)
                {
                    SendIdentificationFailedMessage(client, IdentificationFailureReasonEnum.BANNED);
                    return;
                }

                client.Account = account;
                client.Characters = AccountManager.GetCharactersByAccount(account.Id);

                SendIdentificationSuccessMessage(client, wasAlreadyConnected);

                if (WorldServerManager.HasWorld(message.serverId))
                {
                    client.SelectedServerId = message.serverId;
                    SendSelectServerData(client);

                    client.Disconnect();
                }
                else
                    SendServersListMessage(client);


                client.LookingOfServers = true;
            }
            else
            {
                SendIdentificationFailedMessage(client, IdentificationFailureReasonEnum.WRONG_CREDENTIALS);
            }
        }

        [AuthHandler(typeof (ServerSelectionMessage))]
        public static void HandleServerSelectionMessage(AuthClient client, ServerSelectionMessage message)
        {
            client.LookingOfServers = false;

            client.SelectedServerId = message.serverId;
            if (WorldServerManager.HasWorld(client.SelectedServerId))
            {
                SendSelectServerData(client);
            }

            client.Disconnect();
        }

        public static void SendSelectServerData(AuthClient client)
        {
            client.LookingOfServers = false;

            WorldRecord record = WorldRecord.FindWorldRecordById(client.SelectedServerId);

            if (record == null)
                throw new Exception(string.Format("Could not find WorldRecord <id:{0}>", client.SelectedServerId));

            // save the ticket in the database
            client.Account.Ticket = client.Key;
            client.Account.LastServer = client.SelectedServerId;
            client.Save();

            client.Send(new SelectedServerDataMessage(
                            record.Id,
                            record.Ip,
                            record.Port,
                            !(client.Account.Role == RoleEnum.Player && record.BlockedToPlayer),
                            client.Key));
        }

        public static void SendServersListMessage(AuthClient client)
        {
            List<GameServerInformations> servers =
                WorldServerManager.Realmlist.Values.Select(
                    record =>
                    new GameServerInformations((uint) record.Id, (uint) record.Status,
                                               (uint) record.Completion,
                                               record.ServerSelectable,
                                               client.GetCharactersCount(record.Id))).ToList();

            client.Send(new ServersListMessage(servers));
        }

        public static void SendIdentificationSuccessMessage(AuthClient client, bool wasAlreadyConnected)
        {
            client.Send(new IdentificationSuccessMessage(
                            client.Account.Nickname,
                            (uint) client.Account.Id,
                            0, // community ID ? ( se trouve dans le d2p, utilisé pour trouver les serveurs de la communauté )
                            client.Account.Role >= RoleEnum.Moderator,
                            client.Account.SecretQuestion,
                            client.Account.GetRegistrationRemainingTime(), // remaining time
                            wasAlreadyConnected));
        }

        public static void SendIdentificationFailedMessage(AuthClient client, IdentificationFailureReasonEnum reason)
        {
            client.Send(new IdentificationFailedMessage((uint) reason));
        }

        [AuthHandler(typeof(NicknameChoiceRequestMessage))]
        public static void HandleNicknameChoiceRequestMessage(AuthClient client, NicknameChoiceRequestMessage message)
        {
            string nickname = message.nickname;

            /* Check the Username */
            if (!CheckNickName(nickname))
            {
                client.Send(new NicknameRefusedMessage((uint)NicknameErrorEnum.INVALID_NICK));
                return;
            }

            /* Same as Login */
            if (nickname == client.Account.Login)
            {
                client.Send(new NicknameRefusedMessage((uint)NicknameErrorEnum.SAME_AS_LOGIN));
                return;
            }

            /* Look like Login */
            if (client.Account.Login.Contains(nickname))
            {
                client.Send(new NicknameRefusedMessage((uint)NicknameErrorEnum.TOO_SIMILAR_TO_LOGIN));
                return;
            }

            /* Already Used */
            if (AccountRecord.FindByNickname(nickname) != null)
            {
                client.Send(new NicknameRefusedMessage((uint)NicknameErrorEnum.ALREADY_USED));
                return;
            }

            /* Ok, it's good */
            client.Account.Nickname = nickname;
            client.Save();
            client.Send(new NicknameAcceptedMessage());
            SendIdentificationSuccessMessage(client, false);
            SendServersListMessage(client);
        }

        public static bool CheckNickName(string nickName)
        {
            string nickNamePattern = @"^[a-zA-Z]{4,18}$";
            return System.Text.RegularExpressions.Regex.IsMatch(nickName, nickNamePattern);
        }
       

    }
}