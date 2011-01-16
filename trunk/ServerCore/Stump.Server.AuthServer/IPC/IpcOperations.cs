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
using Stump.Database;
using Stump.Server.AuthServer.Managers;
using Stump.Server.BaseServer.IPC;

namespace Stump.Server.AuthServer.IPC
{
    public class IpcOperations : MarshalByRefObject, IRemoteOperationsAuth
    {

        #region IRemoteOperationsAuth Members

        public bool RegisterWorld(WorldServerInformation wsi, int channelPort)
        {
            wsi.LastPing = DateTime.Now;

            if (WorldServerManager.AddWorld(wsi))
            {
                IpcServer.Instance.RegisterTcpClient(wsi, channelPort);
                return true;
            }
            return false;
        }

        public void UnRegisterWorld(WorldServerInformation wsi)
        {
            WorldServerManager.RemoveWorld(wsi);
        }

        public void ChangeState(WorldServerInformation wsi, DofusProtocol.Enums.ServerStatusEnum state)
        {
            WorldServerManager.ChangeWorldState(wsi.Id, state);
        }

        public void IncrementConnectedChars(WorldServerInformation wsi)
        {
            if (WorldServerManager.Realmlist.ContainsKey(wsi.Id))
            {
                WorldServerManager.Realmlist[wsi.Id].CharsCount++;

                if (WorldServerManager.Realmlist[wsi.Id].CharsCount >= WorldServerManager.Realmlist[wsi.Id].CharCapacity && WorldServerManager.Realmlist[wsi.Id].Status == DofusProtocol.Enums.ServerStatusEnum.ONLINE)
                {
                    WorldServerManager.ChangeWorldState(wsi.Id, DofusProtocol.Enums.ServerStatusEnum.FULL);
                }
            }
        }

        public void DecrementConnectedChars(WorldServerInformation wsi)
        {
            if (WorldServerManager.Realmlist.ContainsKey(wsi.Id))
            {
                WorldServerManager.Realmlist[wsi.Id].CharsCount--;

                if (WorldServerManager.Realmlist[wsi.Id].CharsCount < WorldServerManager.Realmlist[wsi.Id].CharCapacity && WorldServerManager.Realmlist[wsi.Id].Status == DofusProtocol.Enums.ServerStatusEnum.FULL)
                {
                    WorldServerManager.ChangeWorldState(wsi.Id, DofusProtocol.Enums.ServerStatusEnum.ONLINE);
                }
            }
        }

        public bool PingConnection(WorldServerInformation wsi)
        {
            // Do nothing. The framework will throw an exception on the remote end if we didn't pong.

            IEnumerable<WorldServerInformation> worlds = WorldServerManager.Worlds.Where(entry => entry.Id == wsi.Id);

            if (worlds.Count() != 1)
            {
                return false;
            }

            worlds.First().LastPing = DateTime.Now;
            return true;
        }

        public AccountRecord GetAccountRecordByTicket(WorldServerInformation wsi, string ticket)
        {
            return AccountRecord.FindAccountByTicket(ticket);
        }

        public AccountRecord GetAccountRecordByNickname(WorldServerInformation wsi, string nickname)
        {
            return AccountRecord.FindAccountByNickname(nickname.ToLower());
        }

        public bool ModifyAccountRecordByNickname(WorldServerInformation wsi, string name, AccountRecord modifiedRecord)
        {
            AccountRecord account = AccountRecord.FindAccountByNickname(name.ToLower());

            if (account == null)
                return false;

            account.LastIp = modifiedRecord.LastIp;
            account.LastConnection = modifiedRecord.LastConnection;
            account.Password = modifiedRecord.Password;
            account.SecretQuestion = modifiedRecord.SecretQuestion;
            account.SecretAnswer = modifiedRecord.SecretAnswer;
            account.Role = modifiedRecord.Role;
            account.BanEndDate = modifiedRecord.BanEndDate;

            account.UpdateAndFlush();

            return true;
        }

        public AccountRecord[] GetAllAccountsRecords(WorldServerInformation wsi)
        {
            return AccountRecord.FindAll();
        }

        public bool CreateAccountRecord(WorldServerInformation wsi, AccountRecord account)
        {
            return AccountManager.CreateAccount(account);
        }

        public bool DeleteAccountRecord(WorldServerInformation wsi, string accountname)
        {
            AccountRecord account = AccountRecord.FindAccountByLogin(accountname);

            if (account == null)
                return false;

            AuthentificationServer.Instance.DisconnectClientsUsingAccount(account);

            return AccountManager.DeleteAccount(account);
        }

        public bool AddAccountCharacter(WorldServerInformation wsi, uint accountId, uint characterId)
        {
            var account = AccountRecord.FindAccountById(accountId);
            var world = WorldServerManager.GetWorldRecord(wsi.Id);

            if (account == null || world == null)
                return false;

            return AccountManager.AddAccountCharacter(account, world, characterId);
        }

        public bool DeleteAccountCharacter(WorldServerInformation wsi, uint accountId, uint characterId)
        {
            var account = AccountRecord.FindAccountById(accountId);
            var world = WorldServerManager.GetWorldRecord(wsi.Id);

            if (account == null || world == null)
                return false;

            return AccountManager.DeleteAccountCharacter(account, world, characterId);
        }

        public bool ExceedsDeletedCharactersQuota(uint accountId)
        {
            var account = AccountRecord.FindAccountById(accountId);

            if (account == null)
                return false;

            return account.DeletedCharacters.Count > 5;
        }

        public bool CheckWorldServerSecretKey(WorldServerInformation wsi, string secretKey)
        {
            return (IpcServer.IpcSecretKey == secretKey);
        }

        public bool BanAccount(WorldServerInformation wsi, uint accountId, DateTime banEndDate)
        {
            var account = AccountRecord.FindAccountById(accountId);

            if (account == null)
                return false;

            account.Banned = true;
            account.BanEndDate = banEndDate;

            account.UpdateAndFlush();

            return true;
        }

        public void BanIp(WorldServerInformation wsi, IpBannedRecord ipBanned)
        {
            if (IpBannedRecord.Exists(ipBanned.Ip))
                IpBannedRecord.Find(ipBanned.Ip).DeleteAndFlush();
            ipBanned.CreateAndFlush();
        }

        #endregion

        public override object InitializeLifetimeService()
        {
            // Mean it's illimited.
            return null;
        }
    }
}