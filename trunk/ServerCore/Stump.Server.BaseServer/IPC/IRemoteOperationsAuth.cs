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
using System.Collections.Generic;
using Stump.Database;

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
        AccountRecord GetAccountRecordByName(WorldServerInformation wsi, string name);

        /// <summary>
        ///   Get and return an account record with a given account name.
        /// </summary>
        /// <returns>success</returns>
        bool ModifyAccountRecordByName(WorldServerInformation wsi, string name, AccountRecord modifiedRecord);

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
        int GetAccountCharacterCount(WorldServerInformation wsi, uint accountid);

        /// <summary>
        /// Get the list of CharacterId of the specified Account
        /// </summary>
        /// <param name="wsi"></param>
        /// <param name="accountid"></param>
        /// <returns></returns>
        uint[] GetAccountCharacters(WorldServerInformation wsi, uint accountid);

        /// <summary>
        /// Add a new Character to the account
        /// </summary>
        /// <param name="wsi"></param>
        /// <param name="accountid"></param>
        /// <param name="characterId"></param>
        void AddAccountCharacter(WorldServerInformation wsi, uint accountid, uint characterId);

        /// <summary>
        /// Delete a character of the account
        /// </summary>
        /// <param name="wsi"></param>
        /// <param name="accountid"></param>
        /// <param name="characterId"></param>
        void DeleteAccountCharacter(WorldServerInformation wsi, uint accountid, uint characterId);

    }
}