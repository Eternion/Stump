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
using Stump.BaseCore.Framework.Attributes;
using Stump.Database;
using Stump.Server.AuthServer.Accounts;
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
            try
            {
                AccountRecord acc = AccountRecord.FindAccountByNickname(name.ToLower());

                if (acc == null)
                    return false;

                acc.LastIp = modifiedRecord.LastIp;
                acc.LastConnection = modifiedRecord.LastConnection;
                acc.Password = modifiedRecord.Password;
                acc.SecretQuestion = modifiedRecord.SecretQuestion;
                acc.SecretAnswer = modifiedRecord.SecretAnswer;
                acc.Role = modifiedRecord.Role;
                acc.BanEndDate = modifiedRecord.BanEndDate;

                acc.UpdateAndFlush();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public AccountRecord[] GetAllAccountsRecords(WorldServerInformation wsi)
        {
            return AccountRecord.FindAll();
        }

        public bool CreateAccountRecord(WorldServerInformation wsi, AccountRecord accrecord)
        {
            try
            {
                return AccountManager.CreateAccount(accrecord);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteAccountRecord(WorldServerInformation wsi, string accountname)
        {
            try
            {
                AccountRecord record = AccountRecord.FindAccountByLogin(accountname);

                if (record != null)
                {
                    AuthentificationServer.Instance.DisconnectClientsUsingAccount(record);

                    return AccountManager.DeleteAccount(record);
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public uint[] GetAccountCharacters(WorldServerInformation wsi, uint accountid)
        {
            return AccountRecord.FindAccountById(accountid).Characters.Select(record => record.CharacterId).ToArray();
        }

        public int GetAccountCharacterCount(WorldServerInformation wsi, uint accountid)
        {
            return AccountRecord.FindAccountById(accountid).Characters.Length;
        }

        public void AddAccountCharacter(WorldServerInformation wsi, uint accountid, uint characterId)
        {
            var record = new WorldCharacterRecord
            {
                WorldId = wsi.Id,
                AccountId = accountid,
                CharacterId = characterId,
            };

            record.SaveAndFlush();
        }

        public void DeleteAccountCharacter(WorldServerInformation wsi, uint accountid, uint characterId)
        {
           var record= WorldCharacterRecord.FindCharacterByServerIdAndCharacterId(wsi.Id, characterId);

           if (record.AccountId != accountid) return;

           record.DeleteAndFlush();
        }

        public bool CheckWorldServerSecretKey(WorldServerInformation wsi, string secretKey)
        {
            return (IpcServer.IpcSecretKey == secretKey);
        }

        #endregion

        public override object InitializeLifetimeService()
        {
            // Mean it's illimited.
            return null;
        }
    }
}