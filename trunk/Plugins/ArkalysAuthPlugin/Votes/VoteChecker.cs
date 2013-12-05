using System;
using System.Linq;
using Stump.Core.Reflection;
using Stump.Server.AuthServer;
using Stump.Server.AuthServer.Database;
using Stump.Server.AuthServer.Managers;
using Stump.Server.BaseServer.Initialization;

namespace ArkalysAuthPlugin.Votes
{
    public class VoteChecker : Singleton<VoteChecker>
    {
        [Initialization(InitializationPass.Last)]
        public void Initialize()
        {
            AuthServer.Instance.IOTaskPool.CallPeriodically(10000, CheckVotes);
        }

        private static void CheckVotes()
        {
            var query =
                string.Format("SELECT * FROM `accounts` WHERE `LastConnectionWorld` IS NOT NULL AND `LastVote` IS NULL OR `LastVote` < '{0}'",
                    (DateTime.Now - TimeSpan.FromHours(3)).ToString("yyyy-MM-dd hh:mm:ss"));

            var accounts = AuthServer.Instance.DBAccessor.Database.Query<Account>(query);

            foreach (var world in accounts.ToArray().Select(account => WorldServerManager.Instance.GetServerById(account.LastConnectionWorld.Value)).Where(world => world.IPCClient != null))
            {
                world.IPCClient.Send(new VoteNotificationMessage(accounts.Select(x => x.Id).ToArray()));
            }
        }
    }
}