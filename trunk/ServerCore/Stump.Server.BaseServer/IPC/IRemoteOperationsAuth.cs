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
using Stump.Database;
using Stump.Database.AuthServer;

namespace Stump.Server.BaseServer.IPC
{
    public interface IRemoteOperationsAuth
    {
        /// <summary>
        ///   Register a game server when he gets online.
        /// </summary>
        /// <param name = "rsi"></param>
        /// <param name = "channelPort"></param>
        bool RegisterWorld(WorldServerInformation rsi, int channelPort);

        /// <summary>
        ///   Unregister our game server. Called on shutdown process.
        /// </summary>
        /// <param name = "rsi"></param>
        void UnRegisterWorld(WorldServerInformation rsi);

        /// <summary>
        /// Change Server State
        /// </summary>
        /// <param name="wsi"></param>
        /// <param name="state"></param>
        void ChangeState(WorldServerInformation wsi, DofusProtocol.Enums.ServerStatusEnum state);

        /// <summary>
        ///   Increment the counter of connected characters
        /// </summary>
        /// <param name="wsi"></param>
        void IncrementConnectedChars(WorldServerInformation wsi);

        /// <summary>
        ///   Decrement the counter of connected characters
        /// </summary>
        void DecrementConnectedChars(WorldServerInformation wsi);

        /// <summary>
        ///   Ping the remote connection.
        ///   Throws a RemotingException if no Pong was received.
        /// </summary>
        /// <returns>Return false if the world server has been disconnected</returns>
        bool PingConnection(WorldServerInformation wsi);

        /// <summary>
        ///   Get and return an account record with a given ticket.
        /// </summary>
        /// <returns></returns>
        AccountRecord GetAccountRecordByTicket(WorldServerInformation wsi, string ticket);

        /// <summary>
        ///   Get and return an account record with a given account name.
        /// </summary>
        /// <returns></returns>
        AccountRecord GetAccountRecordByNickname(WorldServerInformation wsi, string nickname);

        /// <summary>
        ///   Get and return an account record with a given account name.
        /// </summary>
        /// <returns>success</returns>
        bool ModifyAccountRecordByNickname(WorldServerInformation wsi, string name, AccountRecord modifiedRecord);

        /// <summary>
        ///   Create a new account and cache it.
        /// </summary>
        /// <returns></returns>
        bool CreateAccountRecord(WorldServerInformation wsi, AccountRecord accrecord);

        /// <summary>
        ///   Delete an account and remove it from the cache if necessary.
        /// </summary>
        /// <returns></returns>
        bool DeleteAccountRecord(WorldServerInformation wsi, string accountname);

        /// <summary>
        ///   Find and returns all account records existing.
        /// </summary>
        /// <returns></returns>
        AccountRecord[] GetAllAccountsRecords(WorldServerInformation wsi);

        /// <summary>
        ///   Get the number of characters from an account
        /// </summary>
        /// <returns></returns>
        //    int GetAccountCharacterCount(WorldServerInformation wsi, AccountRecord account);

        /// <summary>
        /// Get the list of CharacterId of the specified Account
        /// </summary>
        /// <param name="wsi"></param>
        /// <param name="accountid"></param>
        /// <returns></returns>
        //   uint[] GetAccountCharacters(WorldServerInformation wsi, AccountRecord account);

        /// <summary>
        /// Add a new Character to the account
        /// </summary>
        /// <param name="wsi"></param>
        /// <param name="accountid"></param>
        /// <param name="characterId"></param>
        bool AddAccountCharacter(WorldServerInformation wsi, uint accountId, uint characterId);

        /// <summary>
        /// Delete a character of the account
        /// </summary>
        /// <param name="wsi"></param>
        /// <param name="accountid"></param>
        /// <param name="characterId"></param>
        bool DeleteAccountCharacter(WorldServerInformation wsi, uint accountId, uint characterId);

        /// <summary>
        /// Check the world server's secret key
        /// </summary>
        /// <param name="wsi"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        bool CheckWorldServerSecretKey(WorldServerInformation wsi, string secretKey);

        /// <summary>
        /// Ban an account
        /// </summary>
        /// <param name="wsi"></param>
        /// <param name="accountId"></param>
        /// <param name="banEndDate"></param>
        /// <returns></returns>
        bool BanAccount(WorldServerInformation wsi, uint accountId, DateTime banEndDate);

        /// <summary>
        /// Ban an IP
        /// </summary>
        /// <param name="wsi"></param>
        /// <param name="ipBanned"></param>
        void BanIp(WorldServerInformation wsi, IpBannedRecord ipBanned);

        /// <summary>
        /// Check if account exceeds the quota of day character deletion
        /// </summary>
        /// <param name="wsi"></param>
        /// <param name="accountid"></param>
        /// <returns></returns>
        int GetDeletedCharactersNumber(uint accountId);
    }
}