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

        private void CheckVotes()
        {

            // select all votes between 1 week and 3 hours ago, limited by 1 vote by account (the one with the last vote)
            var votes =
                AuthServer.Instance.DBAccessor.Database.Fetch<VoteRecord>(
                    string.Format("SELECT * FROM Votes INNER JOIN (SELECT UserID, MAX(Date) as MaxDate FROM Votes GROUP BY UserID)" +
                    " grp ON Votes.UserID = grp.UserID AND Votes.Date = grp.MaxDate WHERE grp.MaxDate BETWEEN '{0}' AND '{1}'",
                    (DateTime.Now - TimeSpan.FromDays(7)).ToString("yyyy-MM-dd hh:mm:ss"), (DateTime.Now - TimeSpan.FromHours(3)).ToString("yyyy-MM-dd hh:mm:ss")));

            if (votes.Count == 0)
                return;

            var accounts = AuthServer.Instance.DBAccessor.Database.Query<Account>(
                string.Format("SELECT * FROM accounts WHERE Id IN ({0})",
                    string.Join(",", votes.Select(x => x.AccountId)))).ToDictionary(x => x.Id);

            var groupedVotes = from vote in votes
                where accounts.ContainsKey(vote.AccountId)
                let acc = accounts[vote.AccountId]
                where acc.LastConnectionWorld != null
                group vote by acc.LastConnectionWorld.Value;

            foreach (var group in groupedVotes)
            {
                var world = WorldServerManager.Instance.GetServerById(group.Key);

                if (world.IPCClient == null)
                    continue;

                world.IPCClient.Send(new VoteNotificationMessage(group.Select(x => x.AccountId).ToArray()));
            }
        }
    }
}