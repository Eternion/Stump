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
        [Variable]
        public const double CharCountUpdateTimer = 30000; // 30 seconds

        private DateTime m_lastdbupdate = DateTime.Now;

        #region IRemoteOperationsAuth Members

        public bool RegisterWorld(WorldServerInformation wsi, int channelPort)
        {
            wsi.LastPing = DateTime.Now;

            // reset characters count
            if (WorldServerManager.Realmlist.ContainsKey(wsi.Id))
            {
                WorldServerManager.Realmlist[wsi.Id].CharsCount = 0;
                WorldServerManager.Realmlist[wsi.Id].SaveAndFlush();
            }

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

        public void IncrementConnectedChars(WorldServerInformation wsi)
        {
            if (WorldServerManager.Realmlist.ContainsKey(wsi.Id))
            {
                WorldServerManager.Realmlist[wsi.Id].CharsCount++;

                if ((DateTime.Now - m_lastdbupdate).TotalMilliseconds >= CharCountUpdateTimer)
                {
                    m_lastdbupdate = DateTime.Now;
                    WorldServerManager.Realmlist[wsi.Id].SaveAndFlush();
                }
            }
        }

        public void DecrementConnectedChars(WorldServerInformation wsi)
        {
            if (WorldServerManager.Realmlist.ContainsKey(wsi.Id))
            {
                WorldServerManager.Realmlist[wsi.Id].CharsCount--;

                if ((DateTime.Now - m_lastdbupdate).TotalMilliseconds >= CharCountUpdateTimer)
                {
                    m_lastdbupdate = DateTime.Now;
                    WorldServerManager.Realmlist[wsi.Id].SaveAndFlush();
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

        public AccountRecord GetAccountRecord(WorldServerInformation wsi, string ticket)
        {
            AccountRecord acc = AccountManager.GetAccountByTicket(ticket);
            return acc;
        }

        public AccountRecord GetAccountRecordByName(WorldServerInformation wsi, string name)
        {
            AccountRecord acc = AccountManager.GetAccountByName(name.ToLower());
            return acc;
        }

        public bool ModifyAccountRecordByName(WorldServerInformation wsi, string name, AccountRecord modifiedRecord)
        {
            try
            {
                AccountRecord acc = AccountManager.GetAccountByName(name.ToLower());

                if (acc == null)
                    return false;

                acc.LastIP = modifiedRecord.LastIP;
                acc.LastLogin = modifiedRecord.LastLogin;
                acc.Password = modifiedRecord.Password;
                acc.SecretQuestion = modifiedRecord.SecretQuestion;
                acc.SecretAnswer = modifiedRecord.SecretAnswer;
                acc.Role = modifiedRecord.Role;
                acc.Banned = modifiedRecord.Banned;
                acc.BanDate = modifiedRecord.BanDate;

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
                AccountRecord record = AccountRecord.FindByLogin(accountname);

                if (record != null)
                {
                    AuthentificationServer.Instance.DisconnectClientsUsingAccount(record);

                    return AccountManager.DeleteAccount(record);
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetCharacterAccountCount(WorldServerInformation wsi, uint accountid)
        {
            return AccountManager.GetCharactersByAccount(accountid).Length;
        }

        public void AddAccountCharacter(WorldServerInformation wsi, uint accountid, uint characterId)
        {
            var record = new WorldCharacterRecord
            {
                ServerId = wsi.Id,
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

        #endregion

        public override object InitializeLifetimeService()
        {
            // Mean it's illimited.
            return null;
        }
    }
}