using Stump.Core.Reflection;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Accounts;

namespace Stump.Server.WorldServer.Worlds.Accounts
{
    public class AccountManager : Singleton<AccountManager>
    {
        public static WorldAccount CreateWorldAccount(WorldClient client)
        {
            /* Create WorldAccount */
            var worldAccount = new WorldAccount
            {
                Id = client.Account.Id,
                Nickname = client.Account.Nickname,
            };
            worldAccount.Create();

            return worldAccount;
        }

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
