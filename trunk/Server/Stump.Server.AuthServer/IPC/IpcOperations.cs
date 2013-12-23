#region License GNU GPL
// IPCOperations.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using System.Collections.Generic;
using System.Reflection;
using NLog;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.AuthServer.Database;
using Stump.Server.AuthServer.Managers;
using Stump.Server.BaseServer.IPC;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.BaseServer.Network;

namespace Stump.Server.AuthServer.IPC
{
    public class IPCOperations
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        private readonly Dictionary<Type, Action<object, IPCMessage>> m_handlers = new Dictionary<Type, Action<object, IPCMessage>>();

        public IPCOperations(IPCClient ipcClient)
        {
            Client = ipcClient;

            InitializeHandlers();
            InitializeDatabase();
        }

        public IPCClient Client
        {
            get;
            private set;
        }

        public WorldServer WorldServer
        {
            get { return Client.Server; }
        }

        private ORM.Database Database
        {
            get;
            set;
        }

        private AccountManager AccountManager
        {
            get;
            set;
        }

        private void InitializeHandlers()
        {
            foreach (var method in GetType().GetMethods(BindingFlags.Instance| BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (method.Name == "HandleMessage")
                    continue;

                var parameters = method.GetParameters();

                if (parameters.Length != 1 || !parameters[0].ParameterType.IsSubclassOf(typeof(IPCMessage)))
                    continue;

                m_handlers.Add(parameters[0].ParameterType, (Action<object, IPCMessage>)DynamicExtension.CreateDelegate(method, typeof(IPCMessage)));
            }
        }

        private void InitializeDatabase()
        {
            logger.Info("Opening Database connection for '{0}' server", WorldServer.Name);
            Database = new ORM.Database(AuthServer.DatabaseConfiguration.GetConnectionString(),
                                       AuthServer.DatabaseConfiguration.ProviderName)
                                       {
                                           KeepConnectionAlive = true
                                       };
            Database.OpenSharedConnection();

            AccountManager = new AccountManager();
            AccountManager.ChangeDataSource(Database);
            AccountManager.Initialize();
        }

        public void HandleMessage(IPCMessage message)
        {
            Action<object, IPCMessage> handler;
            if (!m_handlers.TryGetValue(message.GetType(), out handler))
            {
                logger.Error("Received message {0} but no method handle it !", message.GetType());
                return;
            }

            handler(this, message);
        }

        private void Handle(AccountRequestMessage message)
        {
            if (!string.IsNullOrEmpty(message.Ticket))
            {
                // no DB action here
                Account account = AccountManager.Instance.FindCachedAccountByTicket(message.Ticket);
                if (account == null)
                {
                    Client.SendError(string.Format("Account not found with ticket {0}", message.Ticket), message);
                    return;
                }
                AccountManager.Instance.UnCacheAccount(account);
                

                Client.ReplyRequest(new AccountAnswerMessage(account.Serialize()), message);
            }
            else if (!string.IsNullOrEmpty(message.Nickname))
            {
                Account account = AccountManager.FindAccountByNickname(message.Nickname);

                if (account == null)
                {
                    Client.SendError(string.Format("Account not found with nickname {0}", message.Nickname), message);
                    return;
                }

                Client.ReplyRequest(new AccountAnswerMessage(account.Serialize()), message);
            }
            else
            {
                Client.SendError("Ticket and Nickname null or empty", message);
            }
        }

        private void Handle(ChangeStateMessage message)
        {
            WorldServerManager.Instance.ChangeWorldState(WorldServer, message.State);
            Client.ReplyRequest(new CommonOKMessage(), message);
        }

        private void Handle(ServerUpdateMessage message)
        {
            if (WorldServer.CharsCount == message.CharsCount)
            {
                Client.ReplyRequest(new CommonOKMessage(), message);
                return;
            }

            WorldServer.CharsCount = message.CharsCount;

            if (WorldServer.CharsCount >= WorldServer.CharCapacity &&
                WorldServer.Status == ServerStatusEnum.ONLINE)
            {
                WorldServerManager.Instance.ChangeWorldState(WorldServer, ServerStatusEnum.FULL);
            }

            if (WorldServer.CharsCount < WorldServer.CharCapacity &&
                WorldServer.Status == ServerStatusEnum.FULL)
            {
                WorldServerManager.Instance.ChangeWorldState(WorldServer, ServerStatusEnum.ONLINE);
            }

            Database.Update(WorldServer);
            Client.ReplyRequest(new CommonOKMessage(), message);
        }

        private void Handle(CreateAccountMessage message)
        {
            var accountData = message.Account;

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

            if (AccountManager.CreateAccount(account))
                Client.ReplyRequest(new CommonOKMessage(), message);
            else
                Client.SendError(string.Format("Login {0} already exists", accountData.Login), message);
        }

        private void Handle(UpdateAccountMessage message)
        {
            var account = AccountManager.FindAccountById(message.Account.Id);

            if (account == null)
            {
                Client.SendError(string.Format("Account {0} not found", message.Account.Id), message);
                return;
            }

            account.PasswordHash = message.Account.PasswordHash;
            account.SecretQuestion = message.Account.SecretQuestion;
            account.SecretAnswer = message.Account.SecretAnswer;
            account.Role = message.Account.Role;
            account.Tokens = message.Account.Tokens;

            Database.Update(account);
            Client.ReplyRequest(new CommonOKMessage(), message);
        }

        private void Handle(DeleteAccountMessage message)
        {
            Account account;
            if (message.AccountId != null)
                account = AccountManager.FindAccountById((int) message.AccountId);
            else if (!string.IsNullOrEmpty(message.AccountName))
                account = AccountManager.FindAccountByLogin(message.AccountName);
            else
            {
                Client.SendError("AccoundId and AccountName are null or empty", message);
                return;
            }

            if (account == null)
            {
                Client.SendError(string.Format("Account {0}{1} not found", message.AccountId, message.AccountName), message);
                return;
            }

            AccountManager.Instance.DisconnectClientsUsingAccount(account);

            if (AccountManager.DeleteAccount(account))
                Client.ReplyRequest(new CommonOKMessage(), message);
            else
                Client.SendError(string.Format("Cannot delete {0}", account.Login), message);
        }

        private void Handle(AddCharacterMessage message)
        {
            var account = AccountManager.FindAccountById(message.AccountId);

            if (account == null)
            {
                Client.SendError(string.Format("Account {0} not found", message.AccountId), message);
                return;
            }

            if (AccountManager.AddAccountCharacter(account, WorldServer, message.CharacterId))
                Client.ReplyRequest(new CommonOKMessage(), message);
            else
                Client.SendError(string.Format("Cannot add {0} character to {1} account", message.CharacterId, message.AccountId), message);
        }

        private void Handle(DeleteCharacterMessage message)
        {
            var account = AccountManager.FindAccountById(message.AccountId);

            if (account == null)
            {
                Client.SendError(string.Format("Account {0} not found", message.AccountId), message);
                return;
            }

            if (AccountManager.DeleteAccountCharacter(account, WorldServer, message.CharacterId))
                Client.ReplyRequest(new CommonOKMessage(), message);
            else
                Client.SendError(string.Format("Cannot delete {0} character from {1} account", message.CharacterId, message.AccountId), message);
        }

        private void Handle(BanAccountMessage message)
        {
            Account victimAccount;
            if (message.AccountId != null)
                victimAccount = AccountManager.FindAccountById((int)message.AccountId);
            else if (!string.IsNullOrEmpty(message.AccountName))
                victimAccount = AccountManager.FindAccountByLogin(message.AccountName);
            else
            {
                Client.SendError("AccoundId and AccountName are null or empty", message);
                return;
            }

            if (victimAccount == null)
            {
                Client.SendError(string.Format("Account {0}{1} not found", message.AccountId, message.AccountName), message);
                return;
            }

            victimAccount.IsBanned = true;
            victimAccount.BanReason = message.BanReason;
            victimAccount.BanEndDate = message.BanEndDate;
            victimAccount.BannerAccountId = message.BannerAccountId;

            Database.Update(victimAccount);
            Client.ReplyRequest(new CommonOKMessage(), message);
        }

        private void Handle(UnBanAccountMessage message)
        {
            Account victimAccount;
            if (message.AccountId != null)
                victimAccount = AccountManager.FindAccountById((int)message.AccountId);
            else if (!string.IsNullOrEmpty(message.AccountName))
                victimAccount = AccountManager.FindAccountByLogin(message.AccountName);
            else
            {
                Client.SendError("AccoundId and AccountName are null or empty", message);
                return;
            }

            if (victimAccount == null)
            {
                Client.SendError(string.Format("Account {0}{1} not found", message.AccountId, message.AccountName), message);
                return;
            }

            victimAccount.IsBanned = false;
            victimAccount.BanEndDate = null;
            victimAccount.BanReason = null;
            victimAccount.BannerAccountId = null;

            Database.Update(victimAccount);
            Client.ReplyRequest(new CommonOKMessage(), message);
        }

        private void Handle(BanIPMessage message)
        {
            var ipBan = AccountManager.FindIpBan(message.IPRange);
            var ip = IPAddressRange.Parse(message.IPRange);
            if (ipBan != null)
            {
                ipBan.BanReason = message.BanReason;
                ipBan.BannedBy = message.BannerAccountId;
                ipBan.Duration = message.BanEndDate.HasValue ? (int?)(message.BanEndDate - DateTime.Now).Value.TotalMinutes : null;
                ipBan.Date = DateTime.Now;

                Database.Update(ipBan);
            }
            else
            {
                var record = new IpBan
                {
                    IP = ip,
                    BanReason = message.BanReason,
                    BannedBy = message.BannerAccountId,
                    Duration = message.BanEndDate.HasValue ? (int?)( message.BanEndDate - DateTime.Now ).Value.TotalMinutes : null,
                    Date = DateTime.Now
                };

                Database.Insert(record);
                AccountManager.Instance.AddIPBan(record);
            }

            Client.ReplyRequest(new CommonOKMessage(), message);
        }

        private void Handle(UnBanIPMessage message)
        {
            IpBan ipBan = AccountManager.FindIpBan(message.IPRange);
            if (ipBan == null)
            {
                Client.SendError(string.Format("IP ban {0} not found", message.IPRange), message);
            }
            else
            {
                Database.Delete(ipBan);
                Client.ReplyRequest(new CommonOKMessage(), message);
            }
        }

        public void Dispose()
        {
            if (Database != null)
                Database.CloseSharedConnection();

            m_handlers.Clear();
        }
    }
}