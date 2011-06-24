
using System;
using Stump.Database;
using Stump.Database.AuthServer;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.IPC;

namespace Stump.Server.WorldServer.Manager
{
    public static partial class AccountManager
    {
        public static AccountRecord GetAccountByTicket(string ticket)
        {
            return IpcAccessor.Instance.ProxyObject.GetAccountRecordByTicket(WorldServer.ServerInformation, ticket);
        }

        public static AccountRecord GetAccountByNickname(string nickName)
        {
            if (!IpcAccessor.Instance.Connected ||
                IpcAccessor.Instance.ProxyObject == null)
                throw new Exception("Cannot acces to AuthServer, check that the server is running");

            return IpcAccessor.Instance.ProxyObject.GetAccountRecordByNickname(WorldServer.ServerInformation, nickName);
        }

        public static RoleEnum GetRoleByNickName(string nickName)
        {
            var acc = GetAccountByNickname(nickName);

            return acc == null ? RoleEnum.None : acc.Role;
        }

        public static RoleEnum GetRoleWithCheckPassword(string nickName, string accPass)
        {
            var acc = GetAccountByNickname(nickName);

            return acc == null || acc.Password != accPass ? RoleEnum.None : acc.Role;
        }

        public static WorldAccountRecord CreateWorldAccount(WorldClient client)
        {
            /* Create Bank */
            var bank = new InventoryRecord
                           {
                               Kamas = 0
                           };
            bank.Create();

            /* Create WorldAccount */
            var worldAccount = new WorldAccountRecord
                                   {
                                       Id = client.Account.Id,
                                       Nickname = client.Account.Nickname,
                                       Bank = bank
                                   };
            worldAccount.Create();
            return worldAccount;
        }
    }
}
