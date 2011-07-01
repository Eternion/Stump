﻿
using System;

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
        AccountData GetAccountByTicket(WorldServerInformation wsi, string ticket);

        /// <summary>
        ///   Get and return an account record with a given account name.
        /// </summary>
        /// <returns></returns>
        AccountData GetAccountByNickname(WorldServerInformation wsi, string nickname);

        /// <summary>
        ///   Get and return an account record with a given account name.
        /// </summary>
        /// <returns>success</returns>
        bool ModifyAccountRecordByNickname(WorldServerInformation wsi, string name, AccountData modifiedRecord);

        /// <summary>
        ///   Create a new account and cache it.
        /// </summary>
        /// <returns></returns>
        bool CreateAccountRecord(WorldServerInformation wsi, AccountData accrecord);

        /// <summary>
        ///   Delete an account and remove it from the cache if necessary.
        /// </summary>
        /// <returns></returns>
        bool DeleteAccountRecord(WorldServerInformation wsi, string accountname);

        /// <summary>
        ///   Find and returns all account records existing.
        /// </summary>
        /// <returns></returns>
        AccountData[] GetAllAccounts(WorldServerInformation wsi);

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
        /// <returns></returns>
        bool BlamAccount(WorldServerInformation wsi, uint accountId, DateTime endDate, string reason);

        /// <summary>
        /// Check if account exceeds the quota of day character deletion
        /// </summary>
        /// <param name="wsi"></param>
        /// <param name="accountid"></param>
        /// <returns></returns>
        int GetDeletedCharactersNumber(uint accountId);
    }
}