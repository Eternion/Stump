using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Accounts;

namespace Stump.Server.WorldServer.Game.Accounts
{
    public class AccountManager : DataManager<AccountManager>
    {
        public WorldAccount CreateWorldAccount(WorldClient client)
        {
            /* Create WorldAccount */
            var worldAccount = new WorldAccount
            {
                Id = client.Account.Id,
                Nickname = client.Account.Nickname,
            };
            Database.Insert(worldAccount);

            return worldAccount;
        }

        /// <summary>
        /// Returns null if not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WorldAccount FindById(int id)
        {
            return Database.FirstOrDefault<WorldAccount>(string.Format(WorldAccountRelator.FetchById, id));
        }

        /// <summary>
        /// Returns null if not found
        /// </summary>
        /// <returns></returns>
        public WorldAccount FindByNickname(string nickname)
        {
            return Database.FirstOrDefault<WorldAccount>(WorldAccountRelator.FetchByNickname, nickname);
        }

        public bool DoesExist(int id)
        {
            return Database.ExecuteScalar<bool>(string.Format("SELECT EXISTS(SELECT 1 FROM accounts WHERE Id={0})", id));
        }
    }
}
