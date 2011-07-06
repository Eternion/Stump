
using System;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.WorldServer.Core.IPC;

namespace Stump.Server.WorldServer.World.Accounts
{
    public class AccountManager : Singleton<AccountManager>
    {
        public AccountData GetAccountByTicket(string ticket)
        {
            return IpcAccessor.Instance.ProxyObject.GetAccountByTicket(WorldServer.ServerInformation, ticket);
        }

        public AccountData GetAccountByNickname(string nickName)
        {
            return IpcAccessor.Instance.ProxyObject.GetAccountByNickname(WorldServer.ServerInformation, nickName);
        }
    }
}
