using System;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Accounts;

namespace Stump.Server.WorldServer.Game.Accounts
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

        public void BanLater(AccountData banned, AccountData banner, TimeSpan duration, string reason)
        {
            WorldServer.Instance.IOTaskPool.AddMessage(() =>
                IpcAccessor.Instance.ProxyObject.BlamAccountFrom(banned.Id, banner.Id, duration, reason));
        }

        public void BanLater(AccountData banned, TimeSpan duration, string reason)
        {
            WorldServer.Instance.IOTaskPool.AddMessage(() =>
                IpcAccessor.Instance.ProxyObject.BlamAccount(banned.Id, duration, reason));
        }

        public AccountData GetAccountByTicket(string ticket)
        {
            return IpcAccessor.Instance.ProxyObject.GetAccountByTicket(ticket);
        }

        public AccountData GetAccountByNickname(string nickName)
        {
            return IpcAccessor.Instance.ProxyObject.GetAccountByNickname(nickName);
        }
    }
}
