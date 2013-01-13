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


        private Dictionary<Type, Action<object, IPCMessage>> m_handlers = new Dictionary<Type, Action<object, IPCMessage>>();

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

                m_handlers.Add(parameters[0].ParameterType, (Action<object, IPCMessage>)method.CreateDelegate(typeof(IPCMessage)));
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
                    Client.SendError(string.Format("Account not found with ticket {0}", message.Ticket));
                    return;
                }

                Client.Send(new AccountAnswerMessage(account.Serialize()));
            }
            else if (!string.IsNullOrEmpty(message.Nickname))
            {
                Account account = AccountManager.FindAccountByNickname(message.Nickname);

                if (account == null)
                {
                    Client.SendError(string.Format("Account not found with nickname {0}", message.Nickname));
                    return;
                }

                Client.Send(new AccountAnswerMessage(account.Serialize()));
            }
            else
            {
                Client.SendError("Ticket and Nickname null or empty");
            }
        }

        private void Handle(ChangeStateMessage message)
        {
            WorldServerManager.Instance.ChangeWorldState(WorldServer, message.State);
            Client.Send(new CommonOKMessage());
        }

        private void Handle(ServerUpdateMessage message)
        {
            if (WorldServer.CharsCount == message.CharsCount)
            {
                Client.Send(new CommonOKMessage());
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
            Client.Send(new CommonOKMessage());
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
                Client.Send(new CommonOKMessage());
            else
                Client.SendError(string.Format("Login {0} already exists", accountData.Login));
        }

        private void Handle(UpdateAccountMessage message)
        {
            Account account = AccountManager.FindAccountById(message.Account.Id);

            if (account == null)
            {
                Client.SendError(string.Format("Account {0} not found", message.Account.Id));
                return;
            }

            account.PasswordHash = message.Account.PasswordHash;
            account.SecretQuestion = message.Account.SecretQuestion;
            account.SecretAnswer = message.Account.SecretAnswer;
            account.Role = message.Account.Role;
            account.Tokens = message.Account.Tokens;

            Database.Update(account);
            Client.Send(new CommonOKMessage());
        }

        private void Handle(DeleteAccountMessage message)
        {
            Account account = null;
            if (message.AccountId != null)
                account = AccountManager.FindAccountById((int) message.AccountId);
            else if (!string.IsNullOrEmpty(message.AccountName))
                account = AccountManager.FindAccountByLogin(message.AccountName);
            else
            {
                Client.SendError("AccoundId and AccountName are null or empty");
                return;
            }

            if (account == null)
            {
                Client.SendError(string.Format("Account {0}{1} not found", message.AccountId, message.AccountName));
                return;
            }

            AccountManager.Instance.DisconnectClientsUsingAccount(account);

            if (AccountManager.DeleteAccount(account))
                Client.Send(new CommonOKMessage());
            else
                Client.SendError(string.Format("Cannot delete {0}", account.Login));
        }

        private void Handle(AddCharacterMessage message)
        {
            Account account = AccountManager.FindAccountById(message.AccountId);

            if (account == null)
            {
                Client.SendError(string.Format("Account {0} not found", message.AccountId));
                return;
            }

            if (AccountManager.AddAccountCharacter(account, WorldServer, message.CharacterId))
                Client.Send(new CommonOKMessage());
            else
                Client.SendError(string.Format("Cannot add {0} character to {1} account", message.CharacterId, message.AccountId));
        }

        private void Handle(DeleteCharacterMessage message)
        {
            Account account = AccountManager.FindAccountById(message.AccountId);

            if (account == null)
            {
                Client.SendError(string.Format("Account {0} not found", message.AccountId));
                return;
            }

            if (AccountManager.DeleteAccountCharacter(account, WorldServer, message.CharacterId))
                Client.Send(new CommonOKMessage());
            else
                Client.SendError(string.Format("Cannot delete {0} character from {1} account", message.CharacterId, message.AccountId));
        }

        private void Handle(BanAccountMessage message)
        {
            Account victimAccount = null;
            if (message.AccountId != null)
                victimAccount = AccountManager.FindAccountById((int)message.AccountId);
            else if (!string.IsNullOrEmpty(message.AccountName))
                victimAccount = AccountManager.FindAccountByLogin(message.AccountName);
            else
            {
                Client.SendError("AccoundId and AccountName are null or empty");
                return;
            }

            if (victimAccount == null)
            {
                Client.SendError(string.Format("Account {0}{1} not found", message.AccountId, message.AccountName));
                return;
            }

            victimAccount.IsBanned = true;
            victimAccount.BanReason = message.BanReason;
            victimAccount.BanEndDate = message.BanEndDate;
            victimAccount.BannerAccountId = message.BannerAccountId;

            Database.Update(victimAccount);
            Client.Send(new CommonOKMessage());
        }

        private void Handle(UnBanAccountMessage message)
        {
            Account victimAccount = null;
            if (message.AccountId != null)
                victimAccount = AccountManager.FindAccountById((int)message.AccountId);
            else if (!string.IsNullOrEmpty(message.AccountName))
                victimAccount = AccountManager.FindAccountByLogin(message.AccountName);
            else
            {
                Client.SendError("AccoundId and AccountName are null or empty");
                return;
            }

            if (victimAccount == null)
            {
                Client.SendError(string.Format("Account {0}{1} not found", message.AccountId, message.AccountName));
                return;
            }

            victimAccount.IsBanned = false;
            victimAccount.BanEndDate = null;
            victimAccount.BanReason = null;
            victimAccount.BannerAccountId = null;

            Database.Update(victimAccount);
            Client.Send(new CommonOKMessage());
        }

        private void Handle(BanIPMessage message)
        {
            IpBan ipBan = AccountManager.FindIpBan(message.IPRange);
            IPAddressRange ip = IPAddressRange.Parse(message.IPRange);
            if (ipBan != null)
            {
                ipBan.BanReason = message.BanReason;
                ipBan.BannedBy = message.BannerAccountId;
                ipBan.Duration = message.BanEndDate.HasValue ? message.BanEndDate - DateTime.Now : null;
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
                    Duration = message.BanEndDate.HasValue ? message.BanEndDate - DateTime.Now : null,
                    Date = DateTime.Now
                };

                Database.Insert(record);
            }

            Client.Send(new CommonOKMessage());
        }

        private void Handle(UnBanIPMessage message)
        {
            IpBan ipBan = AccountManager.FindIpBan(message.IPRange);
            if (ipBan == null)
            {
                Client.SendError(string.Format("IP ban {0} not found", message.IPRange));
            }
            else
            {
                Database.Delete(ipBan);
                Client.Send(new CommonOKMessage());
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