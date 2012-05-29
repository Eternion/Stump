using System;
using System.ServiceModel;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.IPC.Objects;

namespace Stump.Server.BaseServer.IPC
{
    [ServiceContract(CallbackContract = typeof(IRemoteWorldOperations), SessionMode = SessionMode.Required)]
    public interface IRemoteAuthOperations
    {
        [OperationContract(IsInitiating = true)]
        RegisterResultEnum RegisterWorld(WorldServerData serverData);

        [OperationContract(IsOneWay=true, IsTerminating = true)]
        void UnRegisterWorld();

        [OperationContract(IsOneWay=true)]
        void ChangeState(ServerStatusEnum state);

        [OperationContract(IsOneWay = true)]
        void UpdateConnectedChars(int value);

        [OperationContract]
        AccountData GetAccountByTicket(string ticket);

        [OperationContract]
        AccountData GetAccountByNickname(string nickname);

        /// <remarks>It only considers password, secret question & answer and role</remarks>
        [OperationContract]
        bool UpdateAccount(AccountData modifiedRecord);

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