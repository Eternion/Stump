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
using Stump.Database;
using Stump.Database.AuthServer;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.IPC;

namespace Stump.Server.WorldServer.Manager
{
    public static partial class AccountManager
    {
        public static AccountRecord GetAccountByTicket(string ticket)
        {
            return IpcAccessor.Instance.ProxyObject.GetAccountRecordByTicket(WorldServer.ServerInformation, ticket);
        }

        public static AccountRecord GetAccountByNickname(string nickName)
        {
            if (!IpcAccessor.Instance.Connected ||
                IpcAccessor.Instance.ProxyObject == null)
                throw new Exception("Cannot acces to AuthServer, check that the server is running");

            return IpcAccessor.Instance.ProxyObject.GetAccountRecordByNickname(WorldServer.ServerInformation, nickName);
        }

        public static RoleEnum GetRoleByNickName(string nickName)
        {
            var acc = GetAccountByNickname(nickName);

            return acc == null ? RoleEnum.None : acc.Role;
        }

        public static RoleEnum GetRoleWithCheckPassword(string nickName, string accPass)
        {
            var acc = GetAccountByNickname(nickName);

            return acc == null || acc.Password != accPass ? RoleEnum.None : acc.Role;
        }

        public static WorldAccountRecord CreateWorldAccount(WorldClient client)
        {
            /* Create Bank */
            var bank = new InventoryRecord
                           {
                               Kamas = 0
                           };
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
    }
}
