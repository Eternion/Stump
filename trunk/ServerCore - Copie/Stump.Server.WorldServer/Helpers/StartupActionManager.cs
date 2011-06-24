
using System;
using Stump.Database;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.IPC;

namespace Stump.Server.WorldServer.Manager
{
    public static class StartupActionManager
    {
        public static AccountRecord GetAccountByTicket(string ticket)
        {
            return IpcAccessor.Instance.ProxyObject.GetAccountRecordByTicket(WorldServer.ServerInformation, ticket);
        }

        public static AccountRecord GetAccountByName(string accName)
        {
            if (!IpcAccessor.Instance.Connected ||
                IpcAccessor.Instance.ProxyObject == null)
                throw new Exception("Cannot acces to AuthServer, check that the server is running");

            return IpcAccessor.Instance.ProxyObject.GetAccountRecordByName(WorldServer.ServerInformation, accName);
        }

        public static RoleEnum GetRoleByName(string accName)
        {
            var acc = GetAccountByName(accName);

            return acc == null ? RoleEnum.None : acc.Role;
        }

        public static RoleEnum GetRoleWithCheckPassword(string accName, string accPass)
        {
            var acc = GetAccountByName(accName);

            return acc == null || acc.Password != accPass ? RoleEnum.None : acc.Role;
        }
    }
}
