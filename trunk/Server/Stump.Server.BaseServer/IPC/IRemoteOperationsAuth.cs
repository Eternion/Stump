
using System;
using Stump.Server.BaseServer.IPC.Objects;

namespace Stump.Server.BaseServer.IPC
{
    public interface IRemoteOperationsAuth
    {
        /// <summary>
        ///   Register a game server when he gets online.
        /// </summary>
        /// <param name = "wsi"></param>
        /// <param name = "channelPort"></param>
        bool RegisterWorld(ref WorldServerData wsi, int channelPort);
        /// <summary>
        ///   Unregister our game server. Called on shutdown process.
        /// </summary>
        /// <param name = "wsi"></param>
        void UnRegisterWorld(WorldServerData wsi);
        /// <summary>
        /// Change Server State
        /// </summary>
        /// <param name="wsi"></param>
        /// <param name="state"></param>
        void ChangeState(WorldServerData wsi, DofusProtocol.Enums.ServerStatusEnum state);
        void UpdateConnectedChars(WorldServerData wsi, int value);
        /// <summary>
        ///   Ping the remote connection.
        ///   Throws a RemotingException if no Pong was received.
        /// </summary>
        /// <returns>Return false if the world server has been disconnected</returns>
        bool PingConnection(WorldServerData wsi);

        /// <summary>
        ///   Get and return an account record with a given ticket.
        /// </summary>
        /// <returns></returns>
        AccountData GetAccountByTicket(WorldServerData wsi, string ticket);
        /// <summary>
        ///   Get and return an account record with a given account name.
        /// </summary>
        /// <returns></returns>
        AccountData GetAccountByNickname(WorldServerData wsi, string nickname);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="wsi"></param>
        /// <param name="name"></param>
        /// <param name="modifiedRecord"></param>
        /// <returns></returns>
        /// <remarks>It only considers password, secret question & answer and role</remarks>
        bool ModifyAccountByNickname(WorldServerData wsi, string name, AccountData modifiedRecord);
        /// <summary>
        ///   Create a new account and cache it.
        /// </summary>
        /// <returns></returns>
        bool CreateAccount(WorldServerData wsi, AccountData account);
        /// <summary>
        ///   Delete an account and remove it from the cache if necessary.
        /// </summary>
        /// <returns></returns>
        bool DeleteAccount(WorldServerData wsi, string accountname);
        /// <summary>
        /// Add a new Character to the account
        /// </summary>
        /// <param name="characterId"></param>
        bool AddAccountCharacter(WorldServerData wsi, uint accountId, uint characterId);
        /// <summary>
        /// Delete a character of the account
        /// </summary>
        /// <param name="wsi"></param>
        /// <param name="characterId"></param>
        bool DeleteAccountCharacter(WorldServerData wsi, uint accountId, uint characterId);
        /// <summary>
        /// Check if account exceeds the quota of day character deletion
        /// </summary>
        /// <param name="wsi"></param>
        /// <returns></returns>
        int GetDeletedCharactersNumber(WorldServerData wsi, uint accountId);
        /// <summary>
        /// Ban an account
        /// </summary>
        /// <param name="wsi"></param>
        /// <returns></returns>
        bool BlamAccount(WorldServerData wsi, uint victimAccountId, uint bannerAccountId, TimeSpan duration, string reason);
        
        bool BanIp(WorldServerData wsi, string ipToBan, uint bannerAccountId, TimeSpan duration, string reason);
    }
}