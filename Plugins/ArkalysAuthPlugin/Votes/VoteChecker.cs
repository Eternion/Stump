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
            var votes =
                AuthServer.Instance.DBAccessor.Database.Query<Account>(
                    string.Format(
                        "SELECT Id,LastConnectionWorld FROM `accounts` WHERE `LastConnectionWorld` IS NOT NULL AND `LastVote` IS NULL OR `LastVote` < '{0}'",
                        (DateTime.Now - TimeSpan.FromHours(3)).ToString("yyyy-MM-dd HH:mm:ss")));

            var groupedAccounts = from vote in votes
                                  where vote.LastConnectionWorld != null
                                  group vote by vote.LastConnectionWorld.Value;

            foreach (var group in groupedAccounts)
            {
                var world = WorldServerManager.Instance.GetServerById(group.Key);

                if (world.IPCClient == null)
                    continue;

                var send = group.Select(x => x.Id).ToArray();
                world.IPCClient.Send(new VoteNotificationMessage(send));
            }
        }
    }
}