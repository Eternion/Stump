// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using Stump.Database.AuthServer;
using Stump.Database.WorldServer;
using Stump.Database.WorldServer.StartupAction;
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
