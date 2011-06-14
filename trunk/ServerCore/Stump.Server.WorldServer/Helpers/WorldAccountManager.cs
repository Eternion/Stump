
using System;
using Stump.Database.AuthServer;
using Stump.Database.WorldServer;
using Stump.Database.WorldServer.StartupAction;
using Stump.Database.WorldServer.Storage;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.IPC;

namespace Stump.Server.WorldServer.Helpers
{
    public static class WorldAccountManager
    {

        public static WorldAccountRecord CreateWorldAccount(WorldClient client)
        {
            /* Create Bank */
            var bank = new InventoryRecord { Kamas = 0 };
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

        public static void DeleteWorldAccount(uint worldAccountId)
        {
            var record = WorldAccountRecord.FindById(worldAccountId);
            record.Delete();
        }

        public static bool AddStartupAction(WorldAccountRecord account, StartupActionRecord startupAction)
        {
            if (account.StartupActions.Contains(startupAction))
                return false;

            account.StartupActions.Add(startupAction);

            return true;
        }

        public static bool RemoveStartupAction(WorldAccountRecord account, StartupActionRecord startupAction)
        {
            if (!account.StartupActions.Contains(startupAction))
                return false;

            account.StartupActions.Remove(startupAction);

            return true;
        }
    }
}
