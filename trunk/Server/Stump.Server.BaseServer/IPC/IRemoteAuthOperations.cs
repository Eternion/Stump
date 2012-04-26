using System;
using System.ServiceModel;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.IPC.Objects;

namespace Stump.Server.BaseServer.IPC
{
    [ServiceContract]
    public interface IRemoteAuthOperations
    {
        [OperationContract]
        RegisterResultEnum RegisterWorld(WorldServerData serverData, string remoteIpcAddress);

        [OperationContract]
        void UnRegisterWorld();

        [OperationContract]
        void ChangeState(ServerStatusEnum state);

        [OperationContract]
        void UpdateConnectedChars(int value);

        [OperationContract]
        AccountData GetAccountByTicket(string ticket);

        [OperationContract]
        AccountData GetAccountByNickname(string nickname);

        /// <remarks>It only considers password, secret question & answer and role</remarks>
        [OperationContract]
        bool ModifyAccountByNickname(string name, AccountData modifiedRecord);

        [OperationContract]
        bool CreateAccount(AccountData accountData);

        [OperationContract]
        bool DeleteAccount(string accountname);

        [OperationContract]
        bool AddAccountCharacter(uint accountId, uint characterId);

        [OperationContract]
        bool DeleteAccountCharacter(uint accountId, uint characterId);

        [OperationContract]
        bool BlamAccountFrom(uint victimAccountId, uint bannerAccountId, TimeSpan duration, string reason);

        [OperationContract]
        bool BlamAccount(uint victimAccountId, TimeSpan duration, string reason);

        [OperationContract]
        bool BanIp(string ipToBan, uint bannerAccountId, TimeSpan duration, string reason);
    }
}